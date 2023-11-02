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

        using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(command.Password, salt, 100_000, HashAlgorithmName.SHA256);
        using SymmetricAlgorithm symmetricAlgorithm = EncryptionServiceUtils.CreateSymmetricAlgorithm(command.Algorithm);

        if (command.Algorithm.Equals("AES"))
        {
            symmetricAlgorithm.KeySize = 256;
        }
        symmetricAlgorithm.Key = pdb.GetBytes(symmetricAlgorithm.KeySize / 8);
        symmetricAlgorithm.GenerateIV();
        symmetricAlgorithm.Mode = EncryptionServiceUtils.GetCipherModeFromString(command.CipherMode);
        symmetricAlgorithm.Padding = PaddingMode.PKCS7;

        
        using MemoryStream ms = new MemoryStream();
        using CryptoStream cs = new CryptoStream(ms, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(plainBytes, 0, plainBytes.Length);
        cs.FlushFinalBlock();
        byte[] cipherBytes = ms.ToArray();

        HMACSHA512 hmac = new(symmetricAlgorithm.Key);
        byte[] saltedCipherBytes = new byte[salt.Length + symmetricAlgorithm.IV.Length + cipherBytes.Length];

        Array.Copy(salt, 0, saltedCipherBytes, 0, salt.Length);
        Array.Copy(symmetricAlgorithm.IV, 0, saltedCipherBytes, salt.Length, symmetricAlgorithm.IV.Length);
        Array.Copy(cipherBytes, 0, saltedCipherBytes, symmetricAlgorithm.IV.Length + salt.Length, cipherBytes.Length);


        byte[] hmacHash = hmac.ComputeHash(saltedCipherBytes);
        byte[] resultWithHMAC = new byte[saltedCipherBytes.Length + hmacHash.Length];

        Array.Copy(saltedCipherBytes, 0, resultWithHMAC, 0, saltedCipherBytes.Length);
        Array.Copy(hmacHash, 0, resultWithHMAC, saltedCipherBytes.Length, hmacHash.Length);
        symmetricAlgorithm.Clear();

        return Convert.ToBase64String(resultWithHMAC);
     }

    private string DecryptData(BaseEncryptionCommand command)
    {
        byte[] resultWithHmac = command.ContentData;
        byte[] hmacHash = new byte[64];

        Array.Copy(resultWithHmac, resultWithHmac.Length - 64, hmacHash, 0, hmacHash.Length);
        byte[] saltedCipherBytes = new byte[resultWithHmac.Length - 64];
        Array.Copy(resultWithHmac, 0, saltedCipherBytes, 0, resultWithHmac.Length - 64);


        byte[] salt = new byte[128];
        byte[] iv = new byte[EncryptionServiceUtils.GetSymetricAlgorithmIVSize(command.Algorithm)];
        byte[] cipherBytes = new byte[saltedCipherBytes.Length - iv.Length - salt.Length];

        Array.Copy(saltedCipherBytes, 0, salt, 0, salt.Length);
        Array.Copy(saltedCipherBytes, salt.Length, iv, 0, iv.Length);
        Array.Copy(saltedCipherBytes, salt.Length + iv.Length, cipherBytes, 0, saltedCipherBytes.Length - salt.Length - iv.Length);

        using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(command.Password, salt, 100_000, HashAlgorithmName.SHA256);
        using SymmetricAlgorithm symmetricAlgorithm = EncryptionServiceUtils.CreateSymmetricAlgorithm(command.Algorithm);

        if (command.Algorithm.Equals("AES"))
        {
            symmetricAlgorithm.KeySize = 256;
        }
        symmetricAlgorithm.Key = pdb.GetBytes(symmetricAlgorithm.KeySize / 8);
        symmetricAlgorithm.IV = iv;
        symmetricAlgorithm.Padding = PaddingMode.PKCS7;
        symmetricAlgorithm.Mode = EncryptionServiceUtils.GetCipherModeFromString(command.CipherMode);

        HMACSHA512 hMACSHA512 = new(symmetricAlgorithm.Key);
        byte[] messageHash = hMACSHA512.ComputeHash(saltedCipherBytes);
        if (!HashCompare(hmacHash, messageHash))
        {
            throw new Exception("Wrong password");
        }

        byte[] result;
        try
        {
            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.FlushFinalBlock();
            result = ms.ToArray();
            symmetricAlgorithm.Clear();
        }
        catch(CryptographicException)
        {
            throw new Exception("Error, please check if you provided correct algorithm o cipher mode");
        }

        return Convert.ToBase64String(result);
    }

    private bool HashCompare(byte[] hash1, byte[] hash2) => hash1.SequenceEqual(hash2);


}






/*bool isEncryption = command is EncryptionCommand;
        byte[] salt;
        byte[] saltedCipherBytes;
        byte[] iv = new byte[EncryptionServiceUtils.GetSymetricAlgorithmIVSize(command.Algorithm)];
        byte[] bytesToProcess;

        if (isEncryption)
        {
            salt = CryptoServiceUtills.GenerateSaltBytes(128);
            bytesToProcess = command.ContentData;
        }
        else
        {
            saltedCipherBytes = command.ContentData;
            salt = new byte[128];
            bytesToProcess = new byte[saltedCipherBytes.Length - iv.Length - salt.Length];

            Array.Copy(saltedCipherBytes, 0, salt, 0, salt.Length);
            Array.Copy(saltedCipherBytes, salt.Length, iv, 0, iv.Length);
            Array.Copy(saltedCipherBytes, salt.Length + iv.Length, bytesToProcess, 0, saltedCipherBytes.Length - salt.Length - iv.Length);
        }

        using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(command.Password, salt, 100_000, HashAlgorithmName.SHA256);
        using SymmetricAlgorithm symmetricAlgorithm = EncryptionServiceUtils.CreateSymmetricAlgorithm(command.Algorithm);

        if (command.Algorithm.Equals("AES"))
        {
            symmetricAlgorithm.KeySize = 256;
        }
        symmetricAlgorithm.Key = pdb.GetBytes(symmetricAlgorithm.KeySize / 8);

        if (isEncryption)
        {
            symmetricAlgorithm.GenerateIV();
        }
        else
        {
            symmetricAlgorithm.IV = iv;
        }
        symmetricAlgorithm.Padding = PaddingMode.PKCS7;
        symmetricAlgorithm.Mode = EncryptionServiceUtils.GetCipherModeFromString(command.CipherMode);

        ICryptoTransform cryptoTransform = isEncryption ? symmetricAlgorithm.CreateEncryptor() : symmetricAlgorithm.CreateDecryptor();
        byte[] result;

        try
        {
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cs.Write(bytesToProcess, 0, bytesToProcess.Length);
                cs.FlushFinalBlock();
                result = ms.ToArray();
            }
        }
        catch (CryptographicException) 
        {
            throw new Exception("Wrong password or parameters were provided");
        }

        if (isEncryption)
        {
            byte[] combinedBytes = new byte[salt.Length + symmetricAlgorithm.IV.Length + result.Length];
            Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
            Array.Copy(symmetricAlgorithm.IV, 0, combinedBytes, salt.Length, symmetricAlgorithm.IV.Length);
            Array.Copy(result, 0, combinedBytes, symmetricAlgorithm.IV.Length + salt.Length, result.Length);
            result = combinedBytes;
        }

        symmetricAlgorithm.Clear();

        return Convert.ToBase64String(result);*/