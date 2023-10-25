using CryptoApp.Client.CustomAttributes;

namespace CryptoApp.Client.Models.EncryptionModel;

internal class DecryptionState : EncryptionBase
{
    [RequiredIfTextAndBase64(nameof(IsFile),false,ErrorMessage = "Encrypted text has to be in base64 format")]
    [RequiredIfBoolProperty(nameof(IsFile), false, ErrorMessage = "Encrypted text is required.")]
    public string EncryptedText { get; set; }

    [RequiredIfBoolProperty(nameof(IsFile), true, ErrorMessage = "Encrypted file is required.")]
    public byte[] EncryptedFile { get; set; }
    public byte[] DecryptedData { get; set; }
}
