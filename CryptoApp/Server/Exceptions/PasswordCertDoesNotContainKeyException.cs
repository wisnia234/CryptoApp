namespace CryptoApp.Server.Exceptions;

public class PasswordCertDoesNotContainKeyException : BaseException
{
    public PasswordCertDoesNotContainKeyException() : base("Provided certficate does not contain private key")
    {
    }
}
