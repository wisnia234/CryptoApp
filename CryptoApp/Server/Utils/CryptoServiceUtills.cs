using System.Security.Cryptography;


namespace CryptoApp.Server.Utils;

internal static class CryptoServiceUtills
{
    public static HashAlgorithmName GetHashAlgorithmName(string hashAlgorithm) => hashAlgorithm switch
    {
        "MD5" => HashAlgorithmName.MD5,
        "SHA1" => HashAlgorithmName.SHA1,
        "SHA256" => HashAlgorithmName.SHA256,
        "SHA384" => HashAlgorithmName.SHA384,
        "SHA512" => HashAlgorithmName.SHA512,
        _ => throw new ArgumentException("Nieznana nazwa algorytmu skrótu")
    };
    public static ECCurve GetECCurveByName(string curveName) => curveName switch
    {
        "brainpoolP160r1" => ECCurve.NamedCurves.brainpoolP160r1,
        "brainpoolP160t1" => ECCurve.NamedCurves.brainpoolP160t1,
        "brainpoolP192r1" => ECCurve.NamedCurves.brainpoolP192r1,
        "brainpoolP192t1" => ECCurve.NamedCurves.brainpoolP192t1,
        "brainpoolP224r1" => ECCurve.NamedCurves.brainpoolP224r1,
        "brainpoolP224t1" => ECCurve.NamedCurves.brainpoolP224t1,
        "brainpoolP256r1" => ECCurve.NamedCurves.brainpoolP256r1,
        "brainpoolP256t1" => ECCurve.NamedCurves.brainpoolP256t1,
        "brainpoolP320r1" => ECCurve.NamedCurves.brainpoolP320r1,
        "brainpoolP320t1" => ECCurve.NamedCurves.brainpoolP320t1,
        "brainpoolP384r1" => ECCurve.NamedCurves.brainpoolP384r1,
        "brainpoolP384t1" => ECCurve.NamedCurves.brainpoolP384t1,
        "brainpoolP512r1" => ECCurve.NamedCurves.brainpoolP512r1,
        "brainpoolP512t1" => ECCurve.NamedCurves.brainpoolP512t1,
        "nistP256" => ECCurve.NamedCurves.nistP256,
        "nistP384" => ECCurve.NamedCurves.nistP384,
        "nistP521" => ECCurve.NamedCurves.nistP521,
        _ => throw new ArgumentException($"Unknown curve name: {curveName}")
    };

    public static byte[] GenerateSaltBytes(int length)
    {
        byte[] saltBytes = new byte[length];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return saltBytes;
    }


    
}
