using Microsoft.AspNetCore.Mvc;
using Collections.Api.Authorization;
using Collections.Api.Models.Users;
using Collections.Api.Services;

namespace Collections.Api.Controllers;

[Authorize]
[OnlyAdmin]
[ApiController]
[Route("collections/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(LoginRequest model)
    {
        var response = await _userService.Login(model);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        var response = await _userService.Register(model);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(int page, int count)
    {
        var response = await _userService.GetUsers(page, count);
        return Ok(response);
    }

    [HttpPut("block")]
    public async Task<IActionResult> Block(List<int> ids)
    {
        await _userService.Block(ids);
        return Ok(new { message = "Users blocked successfully" });
    }

    [HttpPut("unblock")]
    public async Task<IActionResult> Unblock(List<int> ids)
    {
        await _userService.Unblock(ids);
        return Ok(new { message = "Users unblocked successfully" });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(List<int> ids)
    {
        await _userService.Delete(ids);
        return Ok(new { message = "Users deleted successfully" });
    }

    [HttpPut("promote")]
    public async Task<IActionResult> PromoteToAdmin(List<int> ids)
    {
        await _userService.PromoteToAdmin(ids);
        return Ok(new { message = "Users promoted successfully" });
    }

    [HttpPut("demote")]
    public async Task<IActionResult> DemoteToUser(List<int> ids)
    {
        await _userService.DemoteToUser(ids);
        return Ok(new { message = "Users promoted successfully" });
    }
}