using Application.Api.DTOs.Authentication;
using Application.Api.Services.Authentication;
using Application.Api.Services.Data;
using Application.Api.Services.Generators;
using Application.Api.Services.MailingService;
using Application.Api.Services.Newsletter;
using Application.SMTP.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IDatabaseService _dbService;
    private readonly IMailingService _smtpService;
    private readonly ILoginHelper _helper;

    public AuthenticationController(IAuthenticationService authService, 
        IDatabaseService dbService,
        IMailingService smtpService, 
        ILoginHelper helper)
    {
        _authService = authService;
        _dbService = dbService;
        _smtpService = smtpService;
        _helper = helper;
    }
    
    [HttpPost("register")]
    public IActionResult Register(RegisterRequestDto requestDto, int code)
    {
        var codes = CodesGeneratorService.GetInstance().Codes;

        var (key, email) = codes.First(x => x.Key == code);
        
        if (key != code) return BadRequest(new {Message = "Wrong code"});
        
        var (success, content) = _authService.Register(requestDto.Username, requestDto.Password, requestDto.Name, requestDto.Surname, email);
        if (!success) return BadRequest(content);

        var loginDto = new LoginRequestDto
        {
            Username = requestDto.Username,
            Password = requestDto.Password
        };

        codes.Remove(key); // Removes the code
        
        return Login(loginDto);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequestDto requestDto)
    {
        var (success, content) = _authService.Login(requestDto.Username!, requestDto.Password);
        if (!success) return BadRequest(content);

        var id = _dbService.GetIdFromDb(requestDto.Username!);
        
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
    public IActionResult RegisterRequest([FromBody] string email)
    {
        var code = Random.Shared.Next();
        
        var request = new RequestDto
        {
            ToAddress = email,
            Subject = MailHelper.GenerateRegisterSubject(),
            Body = MailHelper.GenerateRegisterBody(code)
        };
        
        var (success, content) = _smtpService.SendMail(request);
        
        if (!success) return BadRequest(content);

        CodesGeneratorService.GetInstance().Codes.Add(code, email);
        
        return Ok(content);
    }
    
    [HttpPost("login/request")]
    public IActionResult LoginRequest([FromBody] LoginRequestDto loginRequest)
    {
        var code = Random.Shared.Next();

        if (string.IsNullOrEmpty(loginRequest.Email) && string.IsNullOrEmpty(loginRequest.Username))
            return BadRequest(new { Message = "Email and Username cannot be empty!" });
        
        string email;
        
        RequestDto request;
        
        if (loginRequest.Email is not null)
        {
            email = loginRequest.Email;
            
            request = new RequestDto
            {
                ToAddress = email,
                Subject = MailHelper.GenerateRegisterSubject(),
                Body = MailHelper.GenerateRegisterBody(code)
            };
        }
        else
        {
            email = _helper.GetEmailByUsername(loginRequest.Username!);
            
            request = new RequestDto
            {
                ToAddress = email,
                Subject = MailHelper.GenerateRegisterSubject(),
                Body = MailHelper.GenerateRegisterBody(code)
            };
        }
        
        var (success, content) = _smtpService.SendMail(request);
        
        if (!success) return BadRequest(content);

        CodesGeneratorService.GetInstance().Codes.Add(code, email);
        
        return Ok(content);
    }
}