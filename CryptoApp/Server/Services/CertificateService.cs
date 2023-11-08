using CryptoApp.Server.Utils;
using CryptoApp.Shared.Commands;
using CryptoApp.Shared.DTOs;
using System.Security.Cryptography.X509Certificates;
using CryptoApp.Server.Services.Interfaces;
using System.Text;
using CryptoApp.Server.Exceptions;

namespace CryptoApp.Server.Services;

internal class CertificateService : ICertificateService
{
    public CertificateAndKeyDTO GenerateCertificate(CertificateCommand command)
    {
        var (certRequest, requestPrivateKey) = CertificateServiceUtills.CreateCertficateRequest(command);
        CertificateRequest certificateRequest = certRequest;
        X509KeyUsageFlags aggregateFlags = CertificateServiceUtills.GetX509MergedKeyUsageFlag(command.KeyUsageFlags);

        certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(aggregateFlags, command.IsKeyUsageFlagsCritical));
        certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(command.IsCA, false, 0, true));

        if (command.EnhancedKeyUsageExtensions.Any())
        {
            var oidCollection = CertificateServiceUtills.GetOIDsCollection(command.EnhancedKeyUsageExtensions);

            X509EnhancedKeyUsageExtension eku = new(oidCollection, (bool)command.EnhancedKeyUsageExtensionsCritical);
            certificateRequest.CertificateExtensions.Add(eku);
        }

        DateTimeOffset notBeforer = command.NotBefore;
        DateTimeOffset notAfter = command.NotAfter;
        X509Certificate2 finalCertificate;

        if (command.IssuerCertificateData is not null
            && command.IssuerCertificateData.Length != 0)
        {

            if(!string.IsNullOrEmpty(command.IssuerCertificatePassword) 
                && command.IssuerPrivateKey.Length != 0)
            {
                throw new PasswordAndKeyProvidedException();
            }


            var serialNumber = CryptoServiceUtills.GenerateSaltBytes(12);
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

            finalCertificate = certificateRequest.Create(issuerCertificate, notBeforer, notAfter, serialNumber);
            finalCertificate = CertificateServiceUtills.ImportPrivateKeyPemToCert(finalCertificate, requestPrivateKey);
        }
        else
        {
            finalCertificate = certificateRequest.CreateSelfSigned(notBeforer, notAfter);
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
