using CryptoApp.Server.Services.Interfaces;
using CryptoApp.Server.Utils;
using CryptoApp.Shared.Commands;
using System.Security.Cryptography;

namespace CryptoApp.Server.Services;

internal class HashService : IHashService
{
    public string GenerateHash(HashCommand command)
    {
        using HashAlgorithm algorithm = HashServiceUtills.CreateHashAlgorithm(command.AlgorithmName);

        byte[] hashBytes = algorithm.ComputeHash(command.Data);
        algorithm.Clear();
        CryptographicOperations.ZeroMemory(command.Data);

        return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
    }
}
