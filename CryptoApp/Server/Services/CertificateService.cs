using CryptoApp.Server.Utils;
using CryptoApp.Shared.Commands;
using CryptoApp.Shared.DTOs;
using System.Security.Cryptography.X509Certificates;
using CryptoApp.Server.Services.Interfaces;
using System.Text;
using CryptoApp.Server.Exceptions;
using System.Security.Cryptography;

namespace CryptoApp.Server.Services;

internal class CertificateService : ICertificateService
{
    public CertificateAndKeyDTO GenerateCertificate(CertificateCommand command)
    {
        DateTimeOffset notBefore = command.NotBefore;
        DateTimeOffset notAfter = command.NotAfter;

        if (notBefore > notAfter)
        {
            throw new WrongDataException();
        }


        var (certRequest, requestPrivateKey) = CertificateServiceUtills.CreateCertficateRequest(command);
        CertificateRequest certificateRequest = certRequest;
        X509KeyUsageFlags aggregateFlags = CertificateServiceUtills.GetX509MergedKeyUsageFlag(command.KeyUsageFlags);

        certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(aggregateFlags, command.IsKeyUsageFlagsCritical));
        certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(command.IsCA, false, 0, true));
        certificateRequest.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(certificateRequest.PublicKey, false));

        if (command.EnhancedKeyUsageExtensions.Any())
        {
            OidCollection oidCollection = CertificateServiceUtills.GetOIDsCollection(command.EnhancedKeyUsageExtensions);

            X509EnhancedKeyUsageExtension enhancedKeyUsage = new(oidCollection, (bool)command.EnhancedKeyUsageExtensionsCritical);
            certificateRequest.CertificateExtensions.Add(enhancedKeyUsage);
        }

        X509Certificate2 finalCertificate;

        if (command.IssuerCertificateData is not null
            && command.IssuerCertificateData.Length != 0)
        {

            if(!string.IsNullOrEmpty(command.IssuerCertificatePassword) 
                && command.IssuerPrivateKey.Length != 0)
            {
                throw new PasswordAndKeyProvidedException();
            }

            byte[] serialNumber = CryptoServiceUtills.GenerateSaltBytes(12);
            X509Certificate2 issuerCertificate;

            if (!string.IsNullOrEmpty(command.IssuerCertificatePassword))
            {
                issuerCertificate = new(command.IssuerCertificateData, command.IssuerCertificatePassword);
            }
            else if(command.IssuerPrivateKey.Length > 0)
            {
                string privateKeyPem = Encoding.UTF8.GetString(command.IssuerPrivateKey);
                issuerCertificate = CertificateServiceUtills.ImportPrivateKeyPemToCert(new(command.IssuerCertificateData), privateKeyPem);
            }
            else 
            {
                throw new EmptyPasswordOrKeyException();
            }

            if (!CertificateServiceUtills.ValidateIssuerSubjectKeyTypes(certRequest, issuerCertificate))
            {
                throw new InvalidIssuerAlgorithmKeyException();
            }

            var issuerKeyId = X509AuthorityKeyIdentifierExtension
                .CreateFromIssuerNameAndSerialNumber(issuerCertificate.IssuerName, Encoding.UTF8.GetBytes(issuerCertificate.SerialNumber));
            
            certificateRequest.CertificateExtensions.Add(issuerKeyId);

            finalCertificate = certificateRequest.Create(issuerCertificate, notBefore, notAfter, serialNumber);
            finalCertificate = CertificateServiceUtills.ImportPrivateKeyPemToCert(finalCertificate, requestPrivateKey);
        }
        else
        {
            finalCertificate = certificateRequest.CreateSelfSigned(notBefore, notAfter);
        }

        byte[] privateKey = CertificateServiceUtills.GetPrivateKeyFromCert(command.AsymetricCipher, finalCertificate);
        byte[] certificateBytes = CertificateServiceUtills.ExportCertificate(finalCertificate, command.CertifcateExtension, command.UserCertificatePassword);

        return new CertificateAndKeyDTO
        {
            Certificate = certificateBytes,
            PrivateKey = privateKey
        };
    }

}
