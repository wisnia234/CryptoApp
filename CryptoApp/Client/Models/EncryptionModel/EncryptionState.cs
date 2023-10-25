using CryptoApp.Client.CustomAttributes;

namespace CryptoApp.Client.Models.EncryptionModel;

internal class EncryptionState : EncryptionBase
{
    [RequiredIfBoolProperty(nameof(IsFile), false, ErrorMessage = "Text is required.")]
    public string Text { get; set; }

    [RequiredIfBoolProperty(nameof(IsFile), true, ErrorMessage = "File is required.")]
    public byte[] FileData { get; set; }
    public byte[] EncryptedData { get; set; }
}
