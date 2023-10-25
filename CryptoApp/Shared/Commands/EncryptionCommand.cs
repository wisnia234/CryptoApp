namespace CryptoApp.Shared.Commands;

public record EncryptionCommand( string Algorithm,  string CipherMode, 
                                string Password, byte[] ContentData) : BaseEncryptionCommand(Algorithm, CipherMode, Password, ContentData);

