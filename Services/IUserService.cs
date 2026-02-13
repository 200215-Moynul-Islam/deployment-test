using ELTBackend.DTOs;

namespace ELTBackend.Services
{
    public interface IUserService
    {
        Task<UserReadDto> CreateUserAsync(UserCreateDto userCreateDto);
        Task<IEnumerable<UserReadDto>> GetAllEmployeesAsync();
        Task<UserReadDto> UpdateUserByIdAsync(Guid id, UserUpdateDto userUpdateDto);
        Task DeactivateUserByIdAsync(Guid id);
        Task<UserWithLeavesDto> GetUserByIdWithLeavesAsync(Guid userId);
        Task UpdatePasswordByIdAsync(Guid id, PasswordUpdateDto passwordUpdateDto);
    }
}
