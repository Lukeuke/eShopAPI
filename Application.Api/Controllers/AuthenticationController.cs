using Application.Api.DTOs.Authentication;
using Application.Api.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthenticationController(IAuthenticationService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public IActionResult Register(AuthenticationRequestDto requestDto)
    {
        var (success, content) = _authService.Register(requestDto.Username, requestDto.Password, requestDto.Name, requestDto.Surname);
        if (!success) return BadRequest();

        return Login(requestDto);
    }

    [HttpPost("login")]
    public IActionResult Login(AuthenticationRequestDto requestDto)
    {
        var (success, content) = _authService.Login(requestDto.Username, requestDto.Password);
        if (!success) return BadRequest(content);

        return Ok(new TokenDto
        {
            Token = content
        });
    }
}