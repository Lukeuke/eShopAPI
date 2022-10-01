using Application.Api.Data;

namespace Application.Api.Services.Authentication;

public sealed class LoginHelper : ILoginHelper
{
    private readonly ApplicationContext _context;

    public LoginHelper(ApplicationContext context)
    {
        _context = context;
    }

    public string GetEmailByUsername(string username)
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == username);

        return user?.Email ?? string.Empty;
    }
}