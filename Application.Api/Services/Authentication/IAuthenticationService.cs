namespace Application.Api.Services.Authentication;

public interface IAuthenticationService
{
    (bool success, string content) Register(string username, string password, string name, string surname);
    (bool success, string content) Login(string username, string password);
}