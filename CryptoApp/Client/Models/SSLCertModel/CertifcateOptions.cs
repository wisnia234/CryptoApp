namespace CryptoApp.Client.Models.SSLCertModel;

internal static class CertifcateOptions
{
    public static List<string> KeyUsages => new()
    {
        "None",
        "NonRepudiation",
        "CrlSign",
        "DataEncipherment",
        "DecipherOnly",
        "EncipherOnly",
        "KeyAgreement",
        "KeyEncipherment",
        "DigitalSignature",
        "KeyCertSign"
    };

    public static List<string> EnhancedKeyUsageFlags => new()
    {
        "Server authentication",
        "Client authentication",
        "Code signing",
        "Email",
        "Timestamping",
        "OCSP Signing",
        "Certificate trust list signing",
        "Microsoft Encrypted File System"

    };

    public static List<string> AsymetricAlgorithms => new()
    {
        "RSA", "ECDSA"
    };

    public static List<string> CertificateExtensions => new()
    {
        ".pem",
        ".der",
        ".pfx",
        ".p12",
        ".cer"
    };


}
