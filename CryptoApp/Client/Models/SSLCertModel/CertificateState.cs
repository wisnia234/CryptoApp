using CryptoApp.Client.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.Models.SSLCertModel;

internal class CertificateState
{
    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; }
    [Required(ErrorMessage = "Start data is required")]
    public DateTime? NotBefore { get; set; }

    [Required(ErrorMessage = "Expiration data is required")]
    public DateTime? NotAfter { get; set; }
    public bool IsCa { get; set; } = false;
    [Required(ErrorMessage = "Asymetric algorithm is required")]
    public string AsymetricCipher { get; set; }
    [RequiredIfSelectedProperty(nameof(AsymetricCipher), "RSA", ErrorMessage = "RSA Padding is required")]
    public string RSAPadding { get; set; }

    [RequiredIfSelectedProperty(nameof(AsymetricCipher), "RSA", ErrorMessage = "RSA key size is required")]
    public int RSAKeySize { get; set; } = 2048;

    [RequiredIfSelectedProperty(nameof(AsymetricCipher), "ECDSA", ErrorMessage = "ECC curve is required")]
    public string ECCCurveName { get; set; }
    [Required(ErrorMessage = "Hash algoritm is required")]
    public string HashAlgorithm { get; set; }

    [Required(ErrorMessage = "Key usages are required")]
    public IEnumerable<string> KeyUsageFlags { get; set; }
    public bool IsKeyUsageFlagsCritical { get; set; } = false;
    public IEnumerable<string> EnhancedKeyUsageExtensions { get; set; } = new List<string>();
    public bool? EnhancedKeyUsageExtensionsCritical { get; set; } = false;
    public string CertificateExtension { get; set; } = ".der";

    public string UserCerificatePassword { get; set; }
    public string UploadedCertificateName {  get; set; }
    public string UploadedCertificateExtension { get; set; }
    public byte[] SigningCertificateData { get; set; }

    public string IssuerCertificatePassword { get; set; }
    [RequiredIfSelectedProperty(nameof(CertificateExtension), ".der", ".pem", ".cer",ErrorMessage = "Issuer private key is required")]

    public byte[] IssuerPrivateKey { get; set; }
    public string UploadedPrivateKeyFileName { get; set; }


    public byte[] PrivateKeyData { get; set; }
    public byte[] CertificateData { get; set;}
}
