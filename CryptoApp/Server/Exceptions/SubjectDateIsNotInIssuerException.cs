namespace CryptoApp.Server.Exceptions;

public class SubjectDateIsNotInIssuerException : BaseException
{
    public SubjectDateIsNotInIssuerException() : base("The subject's date is not included in the issuer date")
    {
    }
}
