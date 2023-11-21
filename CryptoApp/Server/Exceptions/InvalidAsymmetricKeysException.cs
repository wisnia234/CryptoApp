namespace CryptoApp.Server.Exceptions;

public class InvalidAsymmetricKeysException : BaseException
{
    public InvalidAsymmetricKeysException() : base("The issuer's private key does not match the certificate's public key")
    {
    }
}
