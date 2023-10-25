using CryptoApp.Client.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.Models.SignatureModel;

internal abstract class SignatureBase
{
    [Required(ErrorMessage = "Signature algorithm is required.")]
    public string SelectedAlgorithm { get; set; }

    [Required(ErrorMessage = "Hashing algorithm is required.")]
    public string SelectedHashingAlgorithm { get; set; }

    [RequiredIfSelectedProperty(nameof(SelectedAlgorithm), "RSA", ErrorMessage = "Padding is required")]
    public string SelectedRSAPadding { get; set; }
    public bool IsFile { get; set; } = false;

    [RequiredIfBoolProperty(nameof(IsFile), false, ErrorMessage = "Input text is required")]
    public string Text { get; set; }
    [RequiredIfBoolProperty(nameof(IsFile), true, ErrorMessage = "Input file is required")]
    public byte[] FileData { get; set; }
    public string UploadedFileName { get; set; }
    public string Signature { get; set; }
}
