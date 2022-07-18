using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Api.Data;
using Application.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace Application.Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly Settings _settings;
    private readonly ApplicationContext _context;

    public AuthenticationService(Settings settings, ApplicationContext context)
    {
        _settings = settings;
        _context = context;
    }
    
    public (bool success, string content) Register(string username, string password, string name, string surname)
    {
        if (_context.Users.Any(u => u.Username == username))
            return (false, "Username not available");

        var user = new User
        {
            Username = username,
            PasswordHash = password,
            Name = name,
            Surname = surname
        };

        user.ProvideSaltAndHash();

        _context.Add(user);
        _context.SaveChanges();

        return (true, string.Empty);
    }

    public (bool success, string content) Login(string username, string password)
    {
        var user = _context.Users.SingleOrDefault(p => p.Username == username);

        if (user == null) return (false, "Invalid username");

        if (user.PasswordHash != AuthenticationHelper.GenerateHash(password, user.Salt))
            return (false, "Invalid password");

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