namespace CryptoApp.Server.Exceptions;

public class WrongPasswordException : BaseException
{
    public WrongPasswordException() : base("Wrong password")
    {

    }
}

