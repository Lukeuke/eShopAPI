using Application.Api.Authorization;

namespace Application.Api.Services.Data;

public interface IDatabaseService
{
    Guid GetIdFromDb(string username);
    List<ERoles> GetRolesFromDb(Guid id);
}