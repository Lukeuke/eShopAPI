using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Api.Authorization;
using Application.Api.Data;
using Application.Api.Models;
using Application.Api.Services.MailingService;
using Application.Api.Services.Newsletter;
using Application.SMTP.Dtos;
using Application.SMTP.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace Application.Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly Settings _settings;
    private readonly ApplicationContext _context;
    private readonly IMailingService _mailingService;

    public AuthenticationService(Settings settings, ApplicationContext context, IMailingService mailingService)
    {
        _settings = settings;
        _context = context;
        _mailingService = mailingService;
    }
    
    public (bool success, object content) Register(string username, string password, string name, string surname, string email)
    {
        if (_context.Users.Any(u => u.Username == username))
            return (false, new { message = "Username not available"} );

        if (string.IsNullOrEmpty(username)) return (false, new {message = "Username cannot be empty"});

        if (password.Length < 8) return (false, new { message = "Password should be at least 8 characters long!" });

        if(!EmailValidator.Validate(email)) return (false, new { message = "Email is not correct" });
        
        var user = new User
        {
            Username = username,
            PasswordHash = password,
            Name = name,
            Email = email,
            Surname = surname,
            CreatedOn = DateTime.Now.ToString(CultureInfo.CurrentCulture),
            Roles = new List<ERoles> { ERoles.User }
        };

        user.ProvideSaltAndHash();

        _context.Add(user);
        _context.SaveChanges();

        _mailingService.SendMail(new RequestDto
        {
            ToAddress = email,
            Subject = MailHelper.GenerateAccountCreationSubject(),
            Body = MailHelper.GenerateAccountCreationBody()
        });
        
        return (true, string.Empty);
    }

    public (bool success, object content) Login(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(p => p.Username == username);

        if (user == null) return (false, new { message = "Couldn't find user with id username" });

        if (user.PasswordHash != AuthenticationHelper.GenerateHash(password, user.Salt))
            return (false, new { message = "Invalid password" });

        return (true, GenerateJwt(AssembleClaimsIdentity(user)));
    }

    private string GenerateJwt(ClaimsIdentity subject)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.BearerKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = subject,
            Expires = DateTime.Now.AddMinutes(5),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private ClaimsIdentity AssembleClaimsIdentity(User user)
    {
        var subject = new ClaimsIdentity(new[]
        {
            new Claim("id", user.Id.ToString()),
        });
        return subject;
    }
}