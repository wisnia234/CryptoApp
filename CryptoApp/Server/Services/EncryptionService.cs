using CryptoApp.Server.Exceptions;
using CryptoApp.Server.Services.Interfaces;
using CryptoApp.Server.Utils;
using CryptoApp.Shared.Commands;
using System.Security.Cryptography;

namespace CryptoApp.Server.Services;

internal class EncryptionService : IEncryptionService
{
    public string SymetricAlgorithmOperation(BaseEncryptionCommand command) => command switch
    {
        EncryptionCommand => EncryptData(command),
        DecryptionCommand => DecryptData(command),
        _ => throw new NotImplementedException() 
    };

     private string EncryptData(BaseEncryptionCommand command)
     {
        byte[] salt = CryptoServiceUtills.GenerateSaltBytes(128);
        byte[] plainBytes = command.ContentData;

        using Rfc2898DeriveBytes rfcDeriveBytes = new(command.Password, salt, 100_000, HashAlgorithmName.SHA256);
        using SymmetricAlgorithm symmetricAlgorithm = EncryptionServiceUtils.CreateSymmetricAlgorithm(command.Algorithm);

        if (command.Algorithm.Equals("AES"))
        {
            symmetricAlgorithm.KeySize = 256;
        }
        symmetricAlgorithm.Key = rfcDeriveBytes.GetBytes(symmetricAlgorithm.KeySize / 8);
        symmetricAlgorithm.GenerateIV();
        symmetricAlgorithm.Mode = EncryptionServiceUtils.GetCipherModeFromString(command.CipherMode);
        symmetricAlgorithm.Padding = PaddingMode.PKCS7;

        
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(plainBytes, 0, plainBytes.Length);
        cs.FlushFinalBlock();
        byte[] encryptedBytes = ms.ToArray();

        HMACSHA512 hmac = new(symmetricAlgorithm.Key);
        byte[] saltIVCipherBytes = new byte[salt.Length + symmetricAlgorithm.IV.Length + encryptedBytes.Length];

        Array.Copy(salt, 0, saltIVCipherBytes, 0, salt.Length);
        Array.Copy(symmetricAlgorithm.IV, 0, saltIVCipherBytes, salt.Length, symmetricAlgorithm.IV.Length);
        Array.Copy(encryptedBytes, 0, saltIVCipherBytes, symmetricAlgorithm.IV.Length + salt.Length, encryptedBytes.Length);


        byte[] hmacEncryption = hmac.ComputeHash(saltIVCipherBytes);
        byte[] finalResult = new byte[saltIVCipherBytes.Length + hmacEncryption.Length];

        Array.Copy(saltIVCipherBytes, 0, finalResult, 0, saltIVCipherBytes.Length);
        Array.Copy(hmacEncryption, 0, finalResult, saltIVCipherBytes.Length, hmacEncryption.Length);
        symmetricAlgorithm.Clear();

        return Convert.ToBase64String(finalResult);
     }

    private string DecryptData(BaseEncryptionCommand command)
    {
        byte[] userCryptogram = command.ContentData;
        byte[] userHMAC = new byte[64];

        Array.Copy(userCryptogram, userCryptogram.Length - 64, userHMAC, 0, userHMAC.Length);
        byte[] saltIVEncryptedBytes = new byte[userCryptogram.Length - 64];
        Array.Copy(userCryptogram, 0, saltIVEncryptedBytes, 0, userCryptogram.Length - 64);

        CryptographicOperations.ZeroMemory(command.ContentData);

        byte[] salt = new byte[128];
        byte[] iv = new byte[EncryptionServiceUtils.GetSymetricAlgorithmIVSize(command.Algorithm)];
        byte[] dataToDecrypt = new byte[saltIVEncryptedBytes.Length - iv.Length - salt.Length];

        Array.Copy(saltIVEncryptedBytes, 0, salt, 0, salt.Length);
        Array.Copy(saltIVEncryptedBytes, salt.Length, iv, 0, iv.Length);
        Array.Copy(saltIVEncryptedBytes, salt.Length + iv.Length, dataToDecrypt, 0, saltIVEncryptedBytes.Length - salt.Length - iv.Length);
        using Rfc2898DeriveBytes rfcDeriveBytes = new(command.Password, salt, 100_000, HashAlgorithmName.SHA256);
        using SymmetricAlgorithm symmetricAlgorithm = EncryptionServiceUtils.CreateSymmetricAlgorithm(command.Algorithm);

        if (command.Algorithm.Equals("AES"))
        {
            symmetricAlgorithm.KeySize = 256;
        }
        symmetricAlgorithm.Key = rfcDeriveBytes.GetBytes(symmetricAlgorithm.KeySize / 8);
        symmetricAlgorithm.IV = iv;
        symmetricAlgorithm.Padding = PaddingMode.PKCS7;
        symmetricAlgorithm.Mode = EncryptionServiceUtils.GetCipherModeFromString(command.CipherMode);

        HMACSHA512 userCryptogramHMAC = new(symmetricAlgorithm.Key);
        byte[] verificationHash = userCryptogramHMAC.ComputeHash(saltIVEncryptedBytes);

        if (!HashCompare(userHMAC, verificationHash))
        {
            throw new WrongPasswordException();
        }

        byte[] result;
        try
        {
            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(dataToDecrypt, 0, dataToDecrypt.Length);
            cs.FlushFinalBlock();
            result = ms.ToArray();
            symmetricAlgorithm.Clear();
        }
        catch(CryptographicException)
        {
            throw new EncryptionException();
        }

        return Convert.ToBase64String(result);
    }

    private bool HashCompare(byte[] hash1, byte[] hash2) 
        => hash1.SequenceEqual(hash2);


}
