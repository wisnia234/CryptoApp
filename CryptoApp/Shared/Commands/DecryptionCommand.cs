namespace CryptoApp.Shared.Commands;

public record DecryptionCommand(string Algorithm, string CipherMode, 
                string Password, byte[] ContentData) : BaseEncryptionCommand(Algorithm,CipherMode,Password, ContentData);
