using CryptoApp.Server.Services.Interfaces;
using CryptoApp.Shared.Commands;
using CryptoApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoApp.Server.Controllers;

[Route("api/Crypto")]
[ApiController]
[Authorize]
public class CryptoController : ControllerBase
{
    private readonly IHashService _hashService;
    private readonly IEncryptionService _encryptionService;
    private readonly ISignatureService _signatureService;
    private readonly ICertificateService _certificateService;
    public CryptoController(IHashService hashService, IEncryptionService encryptionService, 
                            ISignatureService signatureService, ICertificateService certificateService) 
    { 
        _hashService = hashService;
        _encryptionService = encryptionService;
        _signatureService = signatureService;
        _certificateService = certificateService;
    }

    [HttpPost("hash")]
    public ActionResult<string> GenerateHash([FromBody] HashCommand command)
    {
        var result = _hashService.GenerateHash(command);
        return Ok(result);
    }

    [HttpPost("sign")] 
    public ActionResult<SignatureAndKeysDTO> GenerateSignature([FromBody] SignatureCommand command)
    {  
        var result = _signatureService.GenerateSignature(command);
        return Ok(result);         
    }

    [HttpPost("verify")]
    public ActionResult<bool> VerifySignature([FromBody] VerificationCommand command)
    {
        var verify = _signatureService.VerifySignature(command);
        return Ok(verify);  
    }

    [HttpPost("encrypt")]
    public ActionResult<string> EncryptData([FromBody] EncryptionCommand command)
    {
        string result = _encryptionService.SymetricAlgorithmOperation(command);
        return Ok(result);
    }

    [HttpPost("decrypt")]
    public ActionResult<string> DecryptData([FromBody] DecryptionCommand command)
    {
         string result = _encryptionService.SymetricAlgorithmOperation(command);
         return Ok(result);
    }

    [HttpPost("certificate")]
    public ActionResult<CertificateAndKeyDTO> GenerateCertficate([FromBody] CertificateCommand command)
    {
        var result = _certificateService.GenerateCertificate(command);
        return Ok(result);
    }

}
