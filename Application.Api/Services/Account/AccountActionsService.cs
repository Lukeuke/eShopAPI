using Application.Api.Data;
using Application.Api.Services.Authentication;

namespace Application.Api.Services.Account;

public class AccountActionsService : IAccountActionsService
{
    private readonly ApplicationContext _context;

    public AccountActionsService(ApplicationContext context)
    {
        _context = context;
    }
    
    public (bool success, object content) ChangePassword(Guid id, string password, string newPassword)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user is null) return (false, new { message = $"user with this id: {id} does not exits"});
        
        if (user.PasswordHash != AuthenticationHelper.GenerateHash(password, user.Salt)) return ( false, new {message = "Passwords are not the same"});

        user.PasswordHash = newPassword;
        
        user.ProvideSaltAndHash();

        _context.SaveChanges();

        return (true, new {message = "Successfully changed password"});
    }

    public (bool success, object content) DeleteAccount(Guid id, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user is null) return (false, new { message = $"user with this id: {id} does not exits"});
        
        if (user.PasswordHash != AuthenticationHelper.GenerateHash(password, user.Salt)) return ( false, new {message = "Passwords are not the same"});

        _context.Users.Remove(user);

        _context.SaveChanges();

        return (true, new {message = "User was successfully removed"});
    }

    public (bool success, object content) ChangeUsername(Guid id, string password, string username)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user is null) return (false, new { message = $"user with this id: {id} does not exits"});
        
        if (user.PasswordHash != AuthenticationHelper.GenerateHash(password, user.Salt)) return ( false, new {message = "Passwords are not the same"});

        user.Username = username;

        _context.SaveChanges();
        
        return (true, new {message = "Username was successfully changed"});
    }
}