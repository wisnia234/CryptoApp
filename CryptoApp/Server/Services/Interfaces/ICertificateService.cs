using CryptoApp.Shared.Commands;
using CryptoApp.Shared.DTOs;

namespace CryptoApp.Server.Services.Interfaces;

public interface ICertificateService
{
    CertificateAndKeyDTO GenerateCertificate(CertificateCommand command);
}
