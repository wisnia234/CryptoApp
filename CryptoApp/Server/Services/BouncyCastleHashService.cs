using CryptoApp.Server.Exceptions;
using CryptoApp.Server.Services.Interfaces;
using CryptoApp.Shared.Commands;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using System.Security.Cryptography;

namespace CryptoApp.Server.Services;

public class BouncyCastleHashService : IHashService
{
    public string GenerateHash(HashCommand command)
    {
        IDigest digest = GetHashType(command.AlgorithmName);
        byte[] hashBytes = new byte[digest.GetDigestSize()];

        digest.BlockUpdate(command.Data, 0, command.Data.Length);
        digest.DoFinal(hashBytes,0);

        CryptographicOperations.ZeroMemory(command.Data);

        return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
    }

    private IDigest GetHashType(string algoritm) => algoritm switch
    {
        "MD2" => new MD2Digest(),
        "MD4" => new MD4Digest(),
        "MD5" => new MD5Digest(),
        "SHA-1" => new Sha1Digest(),
        "SHA-224" => new Sha224Digest(),
        "SHA-256" => new Sha256Digest(),
        "SHA-384" => new Sha384Digest(),
        "SHA-512" => new Sha512Digest(),
        "SHA3-224" => new Sha3Digest(224),
        "SHA3-256" => new Sha3Digest(256),
        "SHA3-384" => new Sha3Digest(384),
        "SHA3-512" => new Sha3Digest(512),
        "SHAKE128" => new ShakeDigest(128),
        "SHAKE256" => new ShakeDigest(256),
        "RIPEMD-128" => new RipeMD128Digest(),
        "RIPEMD-160" => new RipeMD160Digest(),
        "RIPEMD-256" => new RipeMD256Digest(),
        "RIPEMD-320" => new RipeMD320Digest(),
        "Whirlpool" => new WhirlpoolDigest(),
        "Tiger" => new TigerDigest(),
        "Blake2b-160" => new Blake2bDigest(160),
        "Blake2b-256" => new Blake2bDigest(256),
        "Blake2b-384" => new Blake2bDigest(384),
        "Blake2b-512" => new Blake2bDigest(512),
        "Blake2s-128" => new Blake2sDigest(128),
        "Blake2s-160" => new Blake2sDigest(160),
        "Blake2s-224" => new Blake2sDigest(224),
        "Blake2s-256" => new Blake2sDigest(256),
        "Skein-256-128" => new SkeinDigest(256, 128),
        "Skein-256-160" => new SkeinDigest(256, 160),
        "Skein-256-224" => new SkeinDigest(256, 224),
        "Skein-256-256" => new SkeinDigest(256, 256),
        "Skein-512-128" => new SkeinDigest(512, 128),
        "Skein-512-160" => new SkeinDigest(512, 160),
        "Skein-512-224" => new SkeinDigest(512, 224),
        "Skein-512-256" => new SkeinDigest(512, 256),
        "Skein-512-384" => new SkeinDigest(512, 384),
        "Skein-512-512" => new SkeinDigest(512, 512),
        "Skein-1024-384" => new SkeinDigest(1024, 384),
        "Skein-1024-512" => new SkeinDigest(1024, 512),
        "Skein-1024-1024" => new SkeinDigest(1024, 1024),
        "SM3" => new SM3Digest(),
        _ => throw new AlgorithmNotExist("Provided algorithm does not exist")
    };


}
