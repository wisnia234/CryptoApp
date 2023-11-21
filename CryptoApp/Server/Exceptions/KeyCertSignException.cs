namespace CryptoApp.Server.Exceptions;

public class KeyCertSignException : BaseException
{
    public KeyCertSignException() : base("Issuer key has not sign usage")
    {
    }
}
