using CryptoApp.Client.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace CryptoApp.Client.Models.HashModel;

internal class HashState
{
    [Required(ErrorMessage = "Algorithm must be selected.")]
    public string SelectedAlgorithm { get; set; }
    public bool IsFile { get; set; } = false;

    [RequiredIfBoolProperty(nameof(IsFile), false, ErrorMessage = "Text is required.")]
    public string Text { get; set; }

    [RequiredIfBoolProperty(nameof(IsFile), true, ErrorMessage = "File is required.")]
    public byte[] FileData { get; set; }
    public string HashResult { get; set; } = string.Empty;
}



