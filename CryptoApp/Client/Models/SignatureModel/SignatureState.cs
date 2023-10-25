using CryptoApp.Client.CustomAttributes;

namespace CryptoApp.Client.Models.SignatureModel;

internal class SignatureState : SignatureBase
{
    [RequiredIfSelectedProperty(nameof(SelectedAlgorithm), "ECDSA", ErrorMessage = "Curve is required.")]
    public string SelectedECCCurveName { get; set; }
    [RequiredIfSelectedProperty(nameof(SelectedAlgorithm), "RSA", "DSA", ErrorMessage = "Curve is required.")]
    public int KeySize { get; set; } = 2048;
    public string Signature { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }

}
