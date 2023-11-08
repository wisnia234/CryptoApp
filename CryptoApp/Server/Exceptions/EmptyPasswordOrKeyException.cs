namespace CryptoApp.Server.Exceptions;

public class EmptyPasswordOrKeyException : BaseException
{
    public EmptyPasswordOrKeyException() : base("Password or private key was not provided")
    {

    }
}
