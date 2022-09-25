using Application.Api.DTOs.Authentication;
using Application.Api.Services.Authentication;
using Application.Api.Services.Data;
using Application.Api.Services.MailingService;
using Application.Api.Services.Newsletter;
using Application.Api.Singletons;
using Application.SMTP.Dtos;
using Application.SMTP.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IDatabaseService _dbService;
    private readonly IMailingService _smtpService;
    
    public AuthenticationController(IAuthenticationService authService, IDatabaseService dbService, IMailingService smtpService)
    {
        _authService = authService;
        _dbService = dbService;
        _smtpService = smtpService;
    }
    
    [HttpPost("register")]
    public IActionResult Register(RegisterRequestDto requestDto, int code)
    {
        var codes = CodesGeneratorService.GetInstance().Codes;
       
        var find = codes.Keys.First(x => x == requestDto.Email);
        
        if (codes[find] != code) return BadRequest(new {Message = "Wrong code"});
        
        var (success, content) = _authService.Register(requestDto.Username, requestDto.Password, requestDto.Name, requestDto.Surname, requestDto.Email);
        if (!success) return BadRequest(content);

        var loginDto = new LoginRequestDto
        {
            Username = requestDto.Username,
            Password = requestDto.Password
        };

        codes.Remove(find); // Removes the code
        
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

    [HttpPost("register/request")]
    public IActionResult RegisterRequest(RegisterRequestDto requestDto)
    {
        var code = Random.Shared.Next();
        
        var request = new RequestDto
        {
            ToAddress = requestDto.Email,
            Subject = MailHelper.GenerateRegisterSubject(),
            Body = MailHelper.GenerateRegisterBody(code)
        };
        
        var (success, content) = _smtpService.SendMail(request);
        
        if (!success) return BadRequest(content);

        CodesGeneratorService.GetInstance().Codes.Add(requestDto.Email, code);
        
        return Ok(content);
    }
}