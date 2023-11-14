namespace CryptoApp.Server.Exceptions;

public class InvalidIssuerAlgorithmKeyException : BaseException
{
    public InvalidIssuerAlgorithmKeyException() : base("Issuer key algorithm is diffrent from subject")
    {
    }
}
