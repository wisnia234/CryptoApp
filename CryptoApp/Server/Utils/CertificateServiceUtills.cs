using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using CryptoApp.Shared.Commands;
using CryptoApp.Server.Exceptions;

namespace CryptoApp.Server.Utils;

internal static class CertificateServiceUtills
{
    public static X509Certificate2 ImportPrivateKeyPemToCert(X509Certificate2 certificate, string privateKey)
    {
        Oid keyAlgorithmOID = new(certificate.GetKeyAlgorithm());
        string algorithmKeyName = keyAlgorithmOID.FriendlyName;
        string privateKeyPem = privateKey;

        try
        {
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
        }
        catch (ArgumentException) 
        {
            throw new InvalidAsymmetricKeysException();
        }

        return certificate;
    }
    public static byte[] ExportCertificate(X509Certificate2 certificate, string certExtension, string? password) => certExtension switch
    {
        ".pem" => Encoding.UTF8.GetBytes(certificate.ExportCertificatePem()),
        ".pfx" or ".p12" => certificate.Export(GetCertificateContentType(certExtension), password),
        _ => certificate.Export(GetCertificateContentType(certExtension))
    };


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
            {"Microsoft Encrypted File System", "1.3.6.1.4.1.311.10.3.4"},
            {"KDC Authentication", "1.3.6.1.5.2.3.5"},
            {"File Recovery", "1.3.6.1.4.1.311.10.3.4.1"},
            {"Windows Update", "1.3.6.1.4.1.311.76.6.1"},
            {"Key Recovery", "1.3.6.1.4.1.311.10.3.11"},
            {"Windows Third Party Application Component", "1.3.6.1.4.1.311.10.3.25"},
            {"Key Recovery Agent", "1.3.6.1.4.1.311.21.6"},
            {"Windows System Component Verification", "1.3.6.1.4.1.311.10.3.6"},
            {"Early Launch Antimalware Driver", "1.3.6.1.4.1.311.61.4.1"},
            {"Windows TCB Component", "1.3.6.1.4.1.311.10.3.23"},
            {"Kernel Mode Code Signing", "1.3.6.1.4.1.311.61.1.1"},
            {"Windows Software Extension Verification", "1.3.6.1.4.1.311.10.3.26"},
            {"Windows Store", "1.3.6.1.4.1.311.76.3.1"},
            {"Smart Card Logon", "1.3.6.1.4.1.311.20.2.2"},
            {"IP security user", "1.3.6.1.5.5.7.3.7"},
            {"Embedded Windows System Component Verification", "1.3.6.1.4.1.311.10.3.8"},
            {"Windows Kits Component", "1.3.6.1.4.1.311.10.3.20"},
            {"IP security tunnel termination", "1.3.6.1.5.5.7.3.6"},
            {"IP security IKE intermediate", "1.3.6.1.5.5.8.2.2"},
            {"License Server Verification", "1.3.6.1.4.1.311.10.6.2"},
            {"Dynamic Code Generator", "1.3.6.1.4.1.311.76.5.1"},
            {"SpcRelaxedPEMarkerCheck", "1.3.6.1.4.1.311.2.6.1"},
            {"SpcEncryptedDigestRetryCount", "1.3.6.1.4.1.311.2.6.2"},
            {"Endorsement Key Certificate", "2.23.133.8.1"},
            {"Encrypting File System", "1.3.6.1.4.1.311.10.3.4"},
            {"HAL Extension", "1.3.6.1.4.1.311.61.5.1"},
            {"IP security end system", "1.3.6.1.5.5.7.3.5"},
            {"Root List Signer", "1.3.6.1.4.1.311.10.3.9"},
            {"Revoked List Signer", "1.3.6.1.4.1.311.10.3.19"},
            {"Qualified Subordination", "1.3.6.1.4.1.311.10.3.10"},
            {"Protected Process Verification", "1.3.6.1.4.1.311.10.3.24"},
            {"Protected Process Light Verification", "1.3.6.1.4.1.311.10.3.22"},
            {"Private Key Archival", "1.3.6.1.4.1.311.21.5"},
            {"Preview Build Signing", "1.3.6.1.4.1.311.10.3.27"},
            {"Platform Certificate", "2.23.133.8.2"},
            {"Microsoft Trust List Signing", "1.3.6.1.4.1.311.10.3.1"},
            {"Microsoft Publisher", "1.3.6.1.4.1.311.76.8.1"},
            {"Lifetime Signing", "1.3.6.1.4.1.311.10.3.13"},
            {"Domain Name System (DNS) Server Trust", "1.3.6.1.4.1.311.64.1.1"},
            {"OEM Windows System Component Verification", "1.3.6.1.4.1.311.10.3.7"}

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
        _ => throw new ExtensionNotExist(extension)
    };

    public static byte[] GetPrivateKeyFromCert(string algorithm, X509Certificate2 certificate) => algorithm switch
    {
        "RSA" => Encoding.UTF8.GetBytes(certificate.GetRSAPrivateKey().ExportRSAPrivateKeyPem()),
        "ECDSA" => Encoding.UTF8.GetBytes(certificate.GetECDsaPrivateKey().ExportECPrivateKeyPem()),
        _ => throw new AlgorithmNotExist(algorithm)
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
            throw new AlgorithmNotExist(command.AsymetricCipher);
        }
    }

    public static void ValidateDateCertficates(X509Certificate2 issuer, DateTimeOffset notBefore, DateTimeOffset notAfter)
    {
        if((issuer.NotBefore > notBefore)
            || issuer.NotAfter < notAfter)
        {
            throw new SubjectDateIsNotInIssuerException();
        }
    }

    public static void ValidateIssuer(CertificateRequest subjectRequest, X509Certificate2 x509Certificate)
    {
        if(!IsIssuerCA(x509Certificate))
        {
            throw new IssuerIsNotCAException();
        }

        if(!IsKeyCertSign(x509Certificate))
        {
            throw new KeyCertSignException();
        }

        if(!ValidateIssuerSubjectKeyTypes(subjectRequest, x509Certificate))
        {
            throw new InvalidIssuerAlgorithmKeyException();
        }
    }

    private static bool IsIssuerCA(X509Certificate2 x509Certificate) =>
        x509Certificate.Extensions
            .OfType<X509BasicConstraintsExtension>()
            .Any(x => x.CertificateAuthority);

    private static bool IsKeyCertSign(X509Certificate2 x509Certificate) =>
        x509Certificate.Extensions
            .OfType<X509KeyUsageExtension>()
            .Any(extension => extension.KeyUsages.HasFlag(X509KeyUsageFlags.KeyCertSign));


    private static bool ValidateIssuerSubjectKeyTypes(CertificateRequest subjectRequest, X509Certificate issuer)
    {
        Oid subjectAlgorithmOID = new(subjectRequest.PublicKey.Oid);
        Oid issuerAlgorithmOID = new(issuer.GetKeyAlgorithm());

        bool result = string.Equals(issuerAlgorithmOID.FriendlyName, subjectAlgorithmOID.FriendlyName);

        return result;
    }

}
