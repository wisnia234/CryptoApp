using CryptoApp.Shared.Commands;

namespace CryptoApp.Server.Services.Interfaces;

public interface IHashService
{
    string GenerateHash(HashCommand command);
}
