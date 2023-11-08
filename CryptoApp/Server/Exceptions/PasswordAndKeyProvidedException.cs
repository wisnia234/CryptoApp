namespace CryptoApp.Server.Exceptions;

public class PasswordAndKeyProvidedException : BaseException
{
    public PasswordAndKeyProvidedException() : base("Either the password or the private key can be used.") 
    {
        
    }
}
