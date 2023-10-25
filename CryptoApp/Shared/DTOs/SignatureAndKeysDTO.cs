using System.Security.Cryptography;

namespace CryptoApp.Shared.DTOs;

public class SignatureAndKeysDTO
{
    public byte[] Signature { get; set; }
    public byte[] PublicKey { get; set; }
    public byte[] PrivateKey { get; set; }

    ~SignatureAndKeysDTO()
    {
        CryptographicOperations.ZeroMemory(Signature);
        CryptographicOperations.ZeroMemory(PrivateKey);
        CryptographicOperations.ZeroMemory(PublicKey);  
    }
}
