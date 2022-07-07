using Microsoft.AspNetCore.Mvc;
using Task4Back.Authorization;
using Task4Back.Models.Users;
using Task4Back.Services;

namespace Task4Back.Controllers
{
    [Authorize]
    [ApiController]
    [Route("task4/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate(LoginRequest model)
        {
            AuthenticationResponse response = _userService.Login(model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            AuthenticationResponse response = _userService.Register(model);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetPage(int page, int count)
        {
            GetPageResponse response = _userService.GetPage(page, count);
            return Ok(response);
        }

        [HttpPut("block")]
        public IActionResult Block(List<int> ids)
        {
            _userService.Block(ids);
            return Ok(new { message = "Users blocked successfully" });
        }

        [HttpPut("unblock")]
        public IActionResult Unblock(List<int> ids)
        {
            _userService.Unblock(ids);
            return Ok(new { message = "Users unblocked successfully" });
        }

        [HttpDelete]
        public IActionResult Delete(List<int> ids)
        {
            _userService.Delete(ids);
            return Ok(new { message = "Users deleted successfully" });
        }
    }
}