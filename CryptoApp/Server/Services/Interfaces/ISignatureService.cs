using CryptoApp.Shared.Commands;
using CryptoApp.Shared.DTOs;

namespace CryptoApp.Server.Services.Interfaces;

public
    interface ISignatureService
{
    SignatureAndKeysDTO GenerateSignature(SignatureCommand command);
    bool VerifySignature(VerificationCommand command);
}
