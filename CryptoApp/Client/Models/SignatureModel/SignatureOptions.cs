namespace CryptoApp.Client.Models.SignatureModel;

internal static class SignatureOptions
{
    public static List<string> AvailableSigningAlgorithms => new() { "RSA", "ECDSA", "DSA" };
    public static List<int> AvailableRSAKeySizes => new() { 512, 1024, 2048, 3072, 4096 };
    public static List<int> AvailableDSAKeySizes => new() { 512, 1024, 2048, 3072 };
    public static List<string> RSAPadding => new() { "Pss", "PKCS1" };
    public static List<string> AvailableHashingAlgorithms => new() { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" };

    public static List<string> ECCCurveNamesList => new()
    {
        "brainpoolP160r1",
        "brainpoolP160t1",
        "brainpoolP192r1",
        "brainpoolP192t1",
        "brainpoolP224r1",
        "brainpoolP224t1",
        "brainpoolP256r1",
        "brainpoolP256t1",
        "brainpoolP320r1",
        "brainpoolP320t1",
        "brainpoolP384r1",
        "brainpoolP384t1",
        "brainpoolP512r1",
        "brainpoolP512t1",
        "nistP256",
        "nistP384",
        "nistP521"
    };
}
