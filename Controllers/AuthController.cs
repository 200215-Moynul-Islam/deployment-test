using ELTBackend.DTOs;
using ELTBackend.Services;
using ELTBackend.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace ELTBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> LoginAsync(
            [FromBody] LoginRequestDto loginRequestDto
        )
        {
            var token = await _authService.LoginAsync(loginRequestDto);
            return Ok(ResponseHelper.Success(new { Token = token }));
        }
    }
}
