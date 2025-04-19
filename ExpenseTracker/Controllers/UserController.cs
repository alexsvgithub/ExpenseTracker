using ExpenseTracker.Business.Interface;
using ExpenseTracker.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var result = await _userService.UpdateProfileAsync(dto);
            if (!result) return NotFound("User not found");

            return Ok("Profile updated successfully");
        }
    }
}
