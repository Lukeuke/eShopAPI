using Application.Api.Models;
using Application.Api.Services.Newsletter;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsletterController : ControllerBase
{
    private readonly INewsletterService _newsletterService;

    public NewsletterController(INewsletterService newsletterService)
    {
        _newsletterService = newsletterService;
    }

    [HttpPost("Send")]
    public IActionResult Send([FromBody] Product product)
    {
        var (success, content) = _newsletterService.SendNotificationToAll(product);
        if (success) return Ok(content);

        return BadRequest(content);
    }

    [HttpPost("Add")]
    public IActionResult Add([FromQuery] Guid id)
    {
        var (success, content) = _newsletterService.AddUserToNewsletter(id);
        
        if (success) return Ok(content);

        return BadRequest(content);
    }
}