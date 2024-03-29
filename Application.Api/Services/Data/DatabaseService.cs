using Application.Api.Authorization;
using Application.Api.Data;

namespace Application.Api.Services.Data;

public class DatabaseService : IDatabaseService
{
    private readonly ApplicationContext _context;

    public DatabaseService(ApplicationContext context)
    {
        _context = context;
    }
    
    public Guid GetIdFromDb(string username)
    {
        var user = _context.Users.First(u => u.Username == username);
        return user.Id;
    }

    public List<ERoles> GetRolesFromDb(Guid id)
    {
        var user = _context.Users.First(u => u.Id == id);
        return user.Roles;
    }
}