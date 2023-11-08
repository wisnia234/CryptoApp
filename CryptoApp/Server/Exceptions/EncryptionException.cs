namespace CryptoApp.Server.Exceptions;

public class EncryptionException : BaseException
{
    public EncryptionException() : base("Error, please check if you provided correct algorithm or cipher mode")
    {

    }
}
