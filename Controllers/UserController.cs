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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/register
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("register")]
        public async Task<ActionResult<UserReadDto>> CreateUser(
            [FromBody] UserCreateDto userCreateDto
        )
        {
            return StatusCode(
                StatusCodes.Status201Created,
                ResponseHelper.Success(data: await _userService.CreateUserAsync(userCreateDto))
            );
        }

        // GET: api/useers/employees
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("employees")]
        public async Task<ActionResult<ApiResponse>> GetAllEmployees()
        {
            return Ok(ResponseHelper.Success(data: await _userService.GetAllEmployeesAsync()));
        }

        // PATCH: api/users/{id:Guid}
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult<ApiResponse>> UpdateUserByIdAsync(
            [FromRoute] Guid id,
            [FromBody] UserUpdateDto userUpdateDto
        )
        {
            return Ok(
                ResponseHelper.Success(
                    data: await _userService.UpdateUserByIdAsync(id, userUpdateDto)
                )
            );
        }

        // DELETE: api/users/{id:Guid}
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<ApiResponse>> DeleteUserByIdAsync([FromRoute] Guid id)
        {
            await _userService.DeactivateUserByIdAsync(id);
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
