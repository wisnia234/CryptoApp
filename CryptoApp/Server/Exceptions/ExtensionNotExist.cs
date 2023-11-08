namespace CryptoApp.Server.Exceptions
{
    public class ExtensionNotExist : BaseException
    {
        public string Extension { get; }
        public ExtensionNotExist(string extension) : base($"Provided extension {extension} is not supported")
        {
            Extension = extension;
        }
    }
}
