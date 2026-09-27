using MediVora.DTO;
using MediVora.Extensions.RateLimiting;
using MediVora.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace MediVora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        [EnableRateLimiting(RateLimitPolicies.Login)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);
            return Ok(response);
        }


        [HttpPost("register")]
        [EnableRateLimiting(RateLimitPolicies.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetLoggedInUser()
        {
            var vEmail = User.FindFirstValue(ClaimTypes.Email);

            if (vEmail == null)
            {
                return Unauthorized("Email claim not found.");
            }

            var vResponse = await _authService.GetLoggedInUserAsync(vEmail);
            return Ok(vResponse);
        }
    }
}
