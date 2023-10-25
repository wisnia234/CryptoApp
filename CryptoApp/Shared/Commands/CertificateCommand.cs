namespace CryptoApp.Shared.Commands;

public record CertificateCommand(
    string Subject,
    bool IsCA,
    DateTime NotBefore,
    DateTime NotAfter,
    string AsymetricCipher,
    string HashAlgorithm,
    string? RSAPadding,
    string? ECCCurveName,
    int? KeySize,
    List<string> KeyUsageFlags,
    bool IsKeyUsageFlagsCritical,
    List<string>? EnhancedKeyUsageExtensions,
    bool? EnhancedKeyUsageExtensionsCritical,
    string CertifcateExtension,
    string? UserCertificatePassword,
    string? IssuerCertificatePassword,
    byte[]? IssuerCertificateData,
    byte[]? IssuerPrivateKey

);
