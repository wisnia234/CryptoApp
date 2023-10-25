namespace CryptoApp.Shared.Commands;

public record SignatureCommand(string Algorithm, int? KeySize, string? Padding,
                                string? Curve, string HashAlgorithm, byte[] Data);

