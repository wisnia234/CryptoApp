using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using CryptoApp.Shared.Commands;

namespace CryptoApp.Server.Utils;

internal static class CertificateServiceUtills
{
    public static X509Certificate2 ImportPrivateKeyPemToCert(X509Certificate2 certificate, string privateKey)
    {
        Oid keyAlgorithmOID = new(certificate.GetKeyAlgorithm());
        string algorithmKeyName = keyAlgorithmOID.FriendlyName;
        string privateKeyPem = privateKey;

        if (algorithmKeyName.Equals("ECC"))
        {
            ECDsa eCDsa = ECDsa.Create();
            eCDsa.ImportFromPem(privateKeyPem);
            certificate = certificate.CopyWithPrivateKey(eCDsa);
        }
        else if (algorithmKeyName.Equals("RSA"))
        {
            RSA rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);
            certificate = certificate.CopyWithPrivateKey(rsa);
        }

        return certificate;
    }

    public static byte[] ExportCertificate(X509Certificate2 certificate, string certExtension, string? password)
    {
        if (certExtension.Equals(".pem"))
        {
            return Encoding.UTF8.GetBytes(certificate.ExportCertificatePem());
        }
        else if (certExtension.Equals(".pfx") || certExtension.Equals(".p12"))
        {
            return certificate.Export(GetCertificateContentType(certExtension), password);
        }

        return certificate.Export(GetCertificateContentType(certExtension));

    }

    public static OidCollection GetOIDsCollection(IEnumerable<string> enhancedKeyUsages)
    {
        Dictionary<string, string> oidNamesAndId = new()
        {
            {"Server authentication", "1.3.6.1.5.5.7.3.1"},
            {"Client authentication", "1.3.6.1.5.5.7.3.2"},
            {"Code signing", "1.3.6.1.5.5.7.3.3"},
            {"Email", "1.3.6.1.5.5.7.3.4"},
            {"Timestamping", "1.3.6.1.5.5.7.3.8"},
            {"OCSP Signing", "1.3.6.1.5.5.7.3.9"},
            {"Certificate trust list signing", "1.3.6.1.4.1.311.10.3.1"},
            {"Microsoft Encrypted File System", "1.3.6.1.4.1.311.10.3.4"}
        };

        OidCollection oids = new();

        enhancedKeyUsages
            .Select(x => oidNamesAndId[x])
            .ToList()
            .ForEach(a => oids.Add(new Oid(a)));

        return oids;

    }

    public static X509KeyUsageFlags GetX509MergedKeyUsageFlag(IEnumerable<string> usageFlags)
    => usageFlags
        .Select(x => (X509KeyUsageFlags)Enum.Parse(typeof(X509KeyUsageFlags), x, true))
        .Aggregate((current, next) => current | next);

    private static X509ContentType GetCertificateContentType(string extension) => extension switch
    {
        ".pfx" => X509ContentType.Pfx,
        ".p12" => X509ContentType.Pkcs12,
        ".cer" or ".der" or ".pem" => X509ContentType.Cert,
        _ => throw new Exception("Invalid extension")
    };

    public static byte[] GetPrivateKeyFromCert(string algorithm, X509Certificate2 certificate) => algorithm switch
    {
        "RSA" => Encoding.UTF8.GetBytes(certificate.GetRSAPrivateKey().ExportRSAPrivateKeyPem()),
        "ECDSA" => Encoding.UTF8.GetBytes(certificate.GetECDsaPrivateKey().ExportECPrivateKeyPem()),
        _ => throw new Exception("Provided algorithm does not exist")
    };


    public static (CertificateRequest, string) CreateCertficateRequest(CertificateCommand command)
    {
        HashAlgorithmName hashAlgorithm = CryptoServiceUtills.GetHashAlgorithmName(command.HashAlgorithm);

        if (command.AsymetricCipher.Equals("ECDSA"))
        {
            ECDsa ecdsa = ECDsa.Create(CryptoServiceUtills.GetECCurveByName(command.ECCCurveName));

            return (new CertificateRequest(command.Subject, ecdsa, hashAlgorithm),
                    ecdsa.ExportECPrivateKeyPem());
        }
        else if (command.AsymetricCipher.Equals("RSA"))
        {
            RSA rsa = RSA.Create((int)command.KeySize);
            var rsaSignaturePadding = command.RSAPadding.Contains("Pss") ? RSASignaturePadding.Pss : RSASignaturePadding.Pkcs1;

            return (new CertificateRequest(command.Subject, rsa, hashAlgorithm, rsaSignaturePadding),
                rsa.ExportRSAPrivateKeyPem());
        }
        else
        {
            throw new Exception("Asymetric Cipher algorithm does not exist");
        }
    }
}
