using System.Security.Cryptography;

namespace CryptoApp.Server.Utils;

internal static class EncryptionServiceUtils
{
    public static CipherMode GetCipherModeFromString(string cipherMode) => cipherMode.ToUpper() switch
    {
        "CBC" => CipherMode.CBC,
        "ECB" => CipherMode.ECB,
        "CFB" => CipherMode.CFB,
        _ => throw new NotImplementedException()
    };
    public static int GetSymetricAlgorithmIVSize(string algorithm) => algorithm switch
    {
        "AES" => 16,
        "DES" or "3DES" => 8,
        _ => throw new Exception("Provided algorithm does not exist")
    };
    public static SymmetricAlgorithm CreateSymmetricAlgorithm(string algorithm) => algorithm.ToUpper() switch
    {
        "AES" => Aes.Create(),
        "DES" => DES.Create(),
        "3DES" => TripleDES.Create(),
        _ => throw new NotImplementedException()
    };

}
