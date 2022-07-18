namespace Application.Api.Services.Data;

public interface IDatabaseService
{
    Guid GetIdFromDb(string username);
}