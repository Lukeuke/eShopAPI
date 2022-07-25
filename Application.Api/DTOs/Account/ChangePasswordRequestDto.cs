namespace Application.Api.DTOs.Account;

public class ChangePasswordRequestDto
{
    public string Password { get; set; }    
    public string NewPassword { get; set; }
}