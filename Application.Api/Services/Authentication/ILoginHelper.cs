namespace Application.Api.Services.Authentication;


public interface ILoginHelper
{
    string GetEmailByUsername(string username);
}