namespace Application.Api.Services.Account;

public interface IAccountActionsService
{
    (bool success, object content) ChangePassword(Guid id, string password, string newPassword);
    (bool success, object content) DeleteAccount(Guid id, string password);
}