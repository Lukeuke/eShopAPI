namespace Application.Api.Services.Authentication;

public interface IAuthenticationService
{
    (bool success, object content) Register(string username, string password, string name, string surname, string email);
    (bool success, object content) Login(string username, string password);
}