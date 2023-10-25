using CryptoApp.Server.Services.Interfaces;
using CryptoApp.Server.Utils;
using CryptoApp.Shared.Commands;
using System.Security.Cryptography;

namespace CryptoApp.Server.Services;

internal class EncryptionService : IEncryptionService
{
    public string SymetricAlgorithmOperation(BaseEncryptionCommand command)
    {
        bool isEncryption = command is EncryptionCommand;
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

        return Convert.ToBase64String(result);
    }

}
