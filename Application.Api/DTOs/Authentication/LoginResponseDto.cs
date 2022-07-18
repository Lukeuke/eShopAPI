namespace Application.Api.DTOs.Authentication;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public TokenDto Token { get; set; }
}