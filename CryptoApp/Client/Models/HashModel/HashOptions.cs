namespace CryptoApp.Client.Models.HashModel;

internal static class HashOptions
{
    public static List<string> Algorithms => new() { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" };

}
