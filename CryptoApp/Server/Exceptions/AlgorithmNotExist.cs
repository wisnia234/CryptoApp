namespace CryptoApp.Server.Exceptions;

public class AlgorithmNotExist : BaseException
{
    public string AlgorithmName { get; }
    public AlgorithmNotExist(string algorithmName) : base($"Provided algorithm {algorithmName} is not supported")
    {
        AlgorithmName = algorithmName;
    }
}
