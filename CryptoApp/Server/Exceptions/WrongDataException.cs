namespace CryptoApp.Server.Exceptions;

public class WrongDataException : BaseException
{
    public WrongDataException() : base($"Provided not before data has greater value than not after")
    {
    }
}
