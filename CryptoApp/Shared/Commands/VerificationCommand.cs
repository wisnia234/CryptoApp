namespace CryptoApp.Shared.Commands;

public record VerificationCommand(byte[] Data, string SignatureAlgorithm, string Signature,
                                string HashAlgorithm, string PublicKey, string? Padding);
