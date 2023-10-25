namespace CryptoApp.Shared.Commands;

public abstract record BaseEncryptionCommand(string Algorithm, string CipherMode,
                string Password, byte[] ContentData);
