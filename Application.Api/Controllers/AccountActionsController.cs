using Application.Api.DTOs.Account;
using Application.Api.Services.Account;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers;

[ApiController]
[Route("Account")]
public class AccountActionsController : ControllerBase
{
    private readonly IAccountActionsService _actionsService;

    public AccountActionsController(IAccountActionsService actionsService)
    {
        _actionsService = actionsService;
    }

    [HttpPatch]
    [Route("ChangePassword")]
    public IActionResult ChangePassword([FromQuery] Guid id, [FromBody] ChangePasswordRequestDto requestDto)
    {
        var password = requestDto.Password;
        var newPassword = requestDto.NewPassword;
        
        var (success, content) = _actionsService.ChangePassword(id, password, newPassword);
        if (!success) return BadRequest(content);

        return Ok(content);
    }

    [HttpDelete]
    [Route("Delete")]
    public IActionResult DeleteAccount([FromQuery] Guid id, [FromBody] PasswordRequestDto requestDto)
    {
        var (success, content) = _actionsService.DeleteAccount(id, requestDto.Password);
        if (!success) return BadRequest(content);

        return Ok(content);
    }

    [HttpPatch]
    [Route("ChangeUsername")]
    public IActionResult ChangeUsername(Guid id, string username, [FromBody] PasswordRequestDto requestDto)
    {
        var (success, content) = _actionsService.ChangeUsername(id, requestDto.Password, username);
        if (!success) return BadRequest(content);

        return Ok(content);
    }
}