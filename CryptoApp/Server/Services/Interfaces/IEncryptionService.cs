using CryptoApp.Shared.Commands;

namespace CryptoApp.Server.Services.Interfaces;

public interface IEncryptionService
{
    string SymetricAlgorithmOperation(BaseEncryptionCommand command);
}
