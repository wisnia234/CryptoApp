using CryptoApp.Client.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.Models.SignatureModel;

internal class VerificationState : SignatureBase
{
    [IsBase64String(ErrorMessage = "Signature has to be in base64 format")]
    [Required(ErrorMessage = "Signature is required.")]
    public string UserSignature { get; set; }
    public string UploadedPublicKeyName { get; set; }
    public string UploadedVerificationFileName { get; set; }
    public string? VerificationStatus { get; set; }
    [Required(ErrorMessage = "Public key is required")]
    public string PublicKeyData { get; set; }
}
