using CryptoApp.Server.Services.Interfaces;
using CryptoApp.Server.Utils;
using CryptoApp.Shared.Commands;
using CryptoApp.Shared.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace CryptoApp.Server.Services;

internal class SignatureService : ISignatureService
{
    public bool VerifySignature(VerificationCommand command)
    {

        if (command.PublicKey.Contains("PRIVATE"))
        {
            return false;
        }

        bool verify = false;

        if (command.SignatureAlgorithm.Equals("RSA"))
        {
            using var rsa = RSA.Create();
            var hashName = new HashAlgorithmName(command.HashAlgorithm);
            var rsaSignaturePadding = command.Padding.Contains("Pss") ? RSASignaturePadding.Pss : RSASignaturePadding.Pkcs1;
            rsa.ImportFromPem(command.PublicKey);
            verify = rsa.VerifyData(command.Data, Convert.FromBase64String(command.Signature), hashName, rsaSignaturePadding);
            rsa.Clear();
        }

        else if (command.SignatureAlgorithm.Equals("ECDSA"))
        {
            using var ecdsa = ECDsa.Create();
            var hashName = new HashAlgorithmName(command.HashAlgorithm);
            ecdsa.ImportFromPem(command.PublicKey);
            verify = ecdsa.VerifyData(command.Data, Convert.FromBase64String(command.Signature), hashName);
            ecdsa.Clear();
        }
        else if (command.SignatureAlgorithm.Equals("DSA"))
        {
            using var dsa = DSA.Create();
            var hashName = new HashAlgorithmName(command.HashAlgorithm);
            dsa.ImportFromPem(command.PublicKey);
            verify = dsa.VerifyData(command.Data, Convert.FromBase64String(command.Signature), hashName);
            dsa.Clear();
        }

        CryptographicOperations.ZeroMemory(command.Data);
        return verify;
    }

    public SignatureAndKeysDTO GenerateSignature(SignatureCommand command)
    {
        byte[] dataBytes = command.Data;
        HashAlgorithmName hashAlgorithmName = CryptoServiceUtills.GetHashAlgorithmName(command.HashAlgorithm.ToUpper());
        byte[] signature;
        string? publicKey = string.Empty;
        string? privateKey = string.Empty;

        if (command.Algorithm.Equals("RSA"))
        {
            using var rsa = RSA.Create((int)command.KeySize);
            RSASignaturePadding padding = command.Padding.Equals("Pss") ? RSASignaturePadding.Pss : RSASignaturePadding.Pkcs1;
            signature = rsa.SignData(dataBytes, hashAlgorithmName, padding);

            publicKey = rsa.ExportRSAPublicKeyPem();
            privateKey = rsa.ExportRSAPrivateKeyPem();
            rsa.Clear();

        }
        else if (command.Algorithm.Equals("ECDSA"))
        {
            using var ecdsa = ECDsa.Create(CryptoServiceUtills.GetECCurveByName(command.Curve));
            signature = ecdsa.SignData(dataBytes, hashAlgorithmName);

            privateKey = ecdsa.ExportECPrivateKeyPem();
            publicKey = ecdsa.ExportSubjectPublicKeyInfoPem();
            ecdsa.Clear();

        }
        else if (command.Algorithm.Equals("DSA"))
        {
            using var dsa = DSA.Create((int)command.KeySize);
            signature = dsa.SignData(dataBytes, hashAlgorithmName);

            privateKey = dsa.ExportPkcs8PrivateKeyPem();
            publicKey = dsa.ExportSubjectPublicKeyInfoPem();
            dsa.Clear();
        }
        else
        {
            throw new Exception("Wrong algorithm");
        }

        CryptographicOperations.ZeroMemory(command.Data);

        return new SignatureAndKeysDTO
        {
            Signature = Encoding.UTF8.GetBytes(Convert.ToBase64String(signature)),
            PublicKey = Encoding.UTF8.GetBytes(publicKey),
            PrivateKey = Encoding.UTF8.GetBytes(privateKey)
        };

    }
}
