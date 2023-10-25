namespace CryptoApp.Client.Models.EncryptionModel;

internal static class EncryptionOptions
{
    public static List<string> AvailableEncryptionAlgorithms => new() { "AES", "DES", "3DES" };
    public static List<string> AvailableCipherModes => new() { "CBC", "ECB", "CFB" };
    public static List<int> AvailableKeySizes => new() { 128, 256 };

}
