namespace CryptoApp.Server.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException(string message) : base(message) 
    {
        
    }
}
