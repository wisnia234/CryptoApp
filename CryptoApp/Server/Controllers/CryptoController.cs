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
        try
        { 
            var result = _hashService.GenerateHash(command);
            return Ok(result);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpPost("sign")] 
    public ActionResult<SignatureAndKeysDTO> GenerateSignature([FromBody] SignatureCommand command)
    {
        try
        {
            var result = _signatureService.GenerateSignature(command);
            return Ok(result);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
                
    }

    [HttpPost("verify")]
    public ActionResult<bool> VerifySignature([FromBody] VerificationCommand command)
    {
        try
        {
            var verify = _signatureService.VerifySignature(command);
            return Ok(verify);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpPost("encrypt")]
    public ActionResult<string> EncryptData([FromBody] EncryptionCommand command)
    {
       /* try
        {*/
            string result = _encryptionService.SymetricAlgorithmOperation(command);
            return Ok(result);
        /*}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }*/
    }

    [HttpPost("decrypt")]
    public ActionResult<string> DecryptData([FromBody] DecryptionCommand command)
    {
       /* try
        {*/
            string result = _encryptionService.SymetricAlgorithmOperation(command);
            return Ok(result);
        /*}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }*/
    }

    [HttpPost("certificate")]
    public ActionResult<CertificateAndKeyDTO> GenerateCertficate([FromBody] CertificateCommand command)
    {
        
        try
        {
            var result = _certificateService.GenerateCertificate(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}
