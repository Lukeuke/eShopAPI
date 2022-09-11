using Application.Api.Services.MailingService;
using Application.SMTP.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MailController : ControllerBase
{
    private readonly IMailingService _mailingService;

    public MailController(IMailingService mailingService)
    {
        _mailingService = mailingService;
    }
    
    [HttpPost("Send")]
    public IActionResult SendMail([FromBody] RequestDto requestDto)
    {
        var (success, content) = _mailingService.SendMail(requestDto);

        if (success) return Ok(content);

        return BadRequest(content);
    }
}