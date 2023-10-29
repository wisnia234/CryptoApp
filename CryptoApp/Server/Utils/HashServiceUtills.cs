using System.Security.Cryptography;

namespace CryptoApp.Server.Utils;

internal static class HashServiceUtills
{
    public static HashAlgorithm CreateHashAlgorithm(string AlgorithmName) => AlgorithmName switch
    {
        "MD5" => MD5.Create(),
        "SHA1" => SHA1.Create(),
        "SHA256" => SHA256.Create(),
        "SHA384" => SHA384.Create(),
        "SHA512" => SHA512.Create(),
        _ => throw new ArgumentException("Nieznana nazwa algorytmu skrótu")
    };
}
