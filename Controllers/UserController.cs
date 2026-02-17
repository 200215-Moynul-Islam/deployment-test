using ELTBackend.Constants;
using ELTBackend.DTOs;
using ELTBackend.Services;
using ELTBackend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ELTBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // POST: api/users/register
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("register")]
        public async Task<ActionResult<UserReadDto>> CreateUser(
            [FromBody] UserCreateDto userCreateDto
        )
        {
            _logger.LogInformation(
                "User creation requested by Admin. Email={Email}",
                userCreateDto.Email
            );

            var createdUser = await _userService.CreateUserAsync(userCreateDto);

            _logger.LogInformation("User created successfully. UserId={UserId}", createdUser.Id);

            return StatusCode(
                StatusCodes.Status201Created,
                ResponseHelper.Success(data: createdUser)
            );
        }

        // GET: api/useers/employees
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("employees")]
        public async Task<ActionResult<ApiResponse>> GetAllEmployees()
        {
            _logger.LogInformation("Employee list requested by Admin");
            var employees = await _userService.GetAllEmployeesAsync();

            return Ok(ResponseHelper.Success(data: employees));
        }

        // PATCH: api/users/{id:Guid}
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ApiResponse>> UpdateUserByIdAsync(
            [FromRoute] Guid id,
            [FromBody] UserUpdateDto userUpdateDto
        )
        {
            _logger.LogInformation("User update requested. UserId={UserId}", id);

            var updatedUser = await _userService.UpdateUserByIdAsync(id, userUpdateDto);

            _logger.LogInformation("User updated successfully. UserId={UserId}", id);

            return Ok(ResponseHelper.Success(data: updatedUser));
        }

        // DELETE: api/users/{id:Guid}
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ApiResponse>> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            _logger.LogWarning("User deactivation requested. UserId={UserId}", id);

            await _userService.DeactivateUserByIdAsync(id);

            _logger.LogInformation("User deactivated successfully. UserId={UserId}", id);

            return Ok(ResponseHelper.Success());
        }

        // GET:api/users/{id:Guid}
        [Authorize(Roles = Roles.Employee)]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ApiResponse>> GetUserByIdWithLeavesAsync([FromRoute] Guid id)
        {
            return Ok(ResponseHelper.Success(await _userService.GetUserByIdWithLeavesAsync(id)));
        }

        // PATCH: api/users/{id:Guid}/update-password
        [Authorize(Roles = Roles.Employee)]
        [HttpPatch("{id:guid}/update-password")]
        public async Task<ActionResult<ApiResponse>> UpdatePasswordAsync(
            [FromRoute] Guid id,
            [FromBody] PasswordUpdateDto passwordUpdateDto
        )
        {
            await _userService.UpdatePasswordByIdAsync(id, passwordUpdateDto);
            return Ok(ResponseHelper.Success());
        }
    }
}
