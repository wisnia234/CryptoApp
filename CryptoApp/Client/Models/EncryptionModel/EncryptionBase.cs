using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.Models.EncryptionModel;

internal abstract class EncryptionBase
{
    [Required(ErrorMessage = "Algorithm is required")]
    public string SelectedAlgorithm { get; set; }

    [Required(ErrorMessage = "Cipher mode is required")]
    public string SelectedCipherMode { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    public bool IsFile { get; set; } = false;
    public string UploadedFileName { get; set; }
    public string FileExtension { get; set; }
}
