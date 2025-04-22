using ExpenseTracker.Business.Interface;
using ExpenseTracker.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var returnValue = await _userService.RegisterUser(dto);

            return Ok(new { returnValue });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _userService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(token);
        }
    }
}
