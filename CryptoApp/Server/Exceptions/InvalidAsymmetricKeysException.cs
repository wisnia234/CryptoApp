namespace CryptoApp.Server.Exceptions;

public class InvalidAsymmetricKeysException : BaseException
{
    public InvalidAsymmetricKeysException() : base($"The issuer's private key does not match the certificate's public key{Environment.NewLine}" +
                                                    $"Please check if you provided correct key or encryption algorithm types")
    {
    }
}
