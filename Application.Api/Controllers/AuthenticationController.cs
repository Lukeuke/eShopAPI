using Application.Api.DTOs.Authentication;
using Application.Api.Services.Authentication;
using Application.Api.Services.Data;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IDatabaseService _dbService;

    public AuthenticationController(IAuthenticationService authService, IDatabaseService dbService)
    {
        _authService = authService;
        _dbService = dbService;
    }
    
    [HttpPost("register")]
    public IActionResult Register(RegisterRequestDto requestDto)
    {
        var (success, content) = _authService.Register(requestDto.Username, requestDto.Password, requestDto.Name, requestDto.Surname);
        if (!success) return BadRequest(content);

        var loginDto = new LoginRequestDto
        {
            Username = requestDto.Username,
            Password = requestDto.Password
        };
        
        return Login(loginDto);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequestDto requestDto)
    {
        var (success, content) = _authService.Login(requestDto.Username, requestDto.Password);
        if (!success) return BadRequest(content);

        var id = _dbService.GetIdFromDb(requestDto.Username);
        
        return Ok(new LoginResponseDto
        {
            Id = id,
            Roles = _dbService.GetRolesFromDb(id),
            Token = new TokenDto
            {
                Value = content.ToString()!
            }
        });
    }
}