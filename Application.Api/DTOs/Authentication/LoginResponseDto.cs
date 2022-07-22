using Application.Api.Authorization;

namespace Application.Api.DTOs.Authentication;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public List<ERoles> Roles { get; set; }
    public TokenDto Token { get; set; }
}