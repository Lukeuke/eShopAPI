namespace Application.Api.DTOs.Authentication;

public class RegisterRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}