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
        "KeyCertSign",
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
        "Microsoft Encrypted File System",
        "File Recovery",
        "KDC Authentication",
        "Windows Update",
        "Key Recovery",
        "Windows Third Party Application Component",
        "Key Recovery Agent",
        "Windows System Component Verification",
        "Early Launch Antimalware Driver",
        "Windows TCB Component",
        "Kernel Mode Code Signing",
        "Windows Software Extension Verification",
        "Windows Store",
        "Smart Card Logon",
        "IP security user",
        "Embedded Windows System Component Verification",
        "Windows Kits Component",
        "IP security tunnel termination",
        "IP security IKE intermediate",
        "License Server Verification",
        "Dynamic Code Generator",
        "SpcRelaxedPEMarkerCheck",
        "SpcEncryptedDigestRetryCount",
        "Endorsement Key Certificate",
        "Encrypting File System",
        "HAL Extension",
        "IP security end system",
        "Root List Signer",
        "Revoked List Signer",
        "Qualified Subordination",
        "Protected Process Verification",
        "Protected Process Light Verification",
        "Private Key Archival",
        "Preview Build Signing",
        "Platform Certificate",
        "Microsoft Trust List Signing",
        "Microsoft Publisher",
        "Lifetime Signing",
        "Domain Name System (DNS) Server Trust",
        "OEM Windows System Component Verification"
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
