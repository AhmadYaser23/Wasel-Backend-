using Microsoft.AspNetCore.Mvc;
using ProjectWasel21.Models.DTOs;
using ProjectWasel21.Services;

namespace ProjectWasel21.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST: api/v1/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/v1/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }

        // POST: api/v1/auth/refresh
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.Token);
            if (result == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(result);
        }

        // POST: api/v1/auth/logout
        [HttpPost("logout")]
        public async Task<ActionResult> Logout(RefreshTokenDto dto)
        {
            var success = await _authService.LogoutAsync(dto.Token);
            if (!success)
                return BadRequest(new { message = "Token not found" });

            return Ok(new { message = "Logged out successfully" });
        }
    }
}