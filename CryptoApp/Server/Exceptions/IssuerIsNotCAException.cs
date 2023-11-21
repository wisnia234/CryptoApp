namespace CryptoApp.Server.Exceptions;

public class IssuerIsNotCAException : BaseException
{
    public IssuerIsNotCAException() : base("Provided issuer is not certificate authority (CA)")
    {
    }
}
