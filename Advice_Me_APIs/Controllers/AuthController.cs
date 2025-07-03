using Advice_Me_APIs.DTOs;
using Advice_Me_APIs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Advice_Me_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _userService;

        public AuthController(IAuth userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDTO dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (result.Contains("success", StringComparison.OrdinalIgnoreCase))
                return Ok(new { message = result });
            return BadRequest(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _userService.LoginAsync(dto);
            if (result.StartsWith("Invalid"))
                return Unauthorized(new { message = result });

            return Ok(new { token = result });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim);
            var userDto = await _userService.GetCurrentUserAsync(userId);

            if (userDto == null)
                return NotFound("User not found.");

            return Ok(userDto);
        }

    }


}