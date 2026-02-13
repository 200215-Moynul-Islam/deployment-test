using AutoMapper;
using ELTBackend.Constants;
using ELTBackend.DTOs;
using ELTBackend.Exceptions;
using ELTBackend.Models;
using ELTBackend.Repositories;

namespace ELTBackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserReadDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            await EnsureEmailIsUniqueOrThrowAsync(userCreateDto.Email!);

            var userEntity = _mapper.Map<User>(userCreateDto);
            userEntity.Id = Guid.NewGuid();
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password);
            userEntity.Role = Roles.Employee; // Default role assignment

            await _userRepository.CreateAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserReadDto>(userEntity);
        }

        public async Task<IEnumerable<UserReadDto>> GetAllEmployeesAsync()
        {
            var employeeEntities = await _userRepository.GetUsersByRoleAsync(Roles.Employee);
            return _mapper.Map<IEnumerable<UserReadDto>>(employeeEntities);
        }

        public async Task<UserReadDto> UpdateUserByIdAsync(Guid id, UserUpdateDto userUpdateDto)
        {
            await EnsureEmailIsUniqueOrThrowAsync(id, userUpdateDto.Email!);
            var userEntity = await GetUserByIdOrThrowAsync(id);
            _mapper.Map(userUpdateDto, userEntity);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserReadDto>(userEntity);
        }

        public async Task DeactivateUserByIdAsync(Guid id)
        {
            var userEntity = await GetUserByIdOrThrowAsync(id);
            userEntity.IsDeleted = true;
            await _userRepository.SaveChangesAsync();
        }

        public async Task<UserWithLeavesDto> GetUserByIdWithLeavesAsync(Guid id)
        {
            return _mapper.Map<UserWithLeavesDto>(await GetUserByIdWithLeavesOrThrowAsync(id));
        }

        public async Task UpdatePasswordByIdAsync(Guid id, PasswordUpdateDto passwordUpdateDto)
        {
            var userEntity = await GetUserByIdOrThrowAsync(id);
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordUpdateDto.Password);
            await _userRepository.SaveChangesAsync();
        }

        #region Private Methods
        private async Task EnsureEmailIsUniqueOrThrowAsync(string email)
        {
            if (await _userRepository.ExistsByEmailAsync(email))
            {
                throw new ConflictException(BusinessErrorMessages.UserEmailAlreadyExists);
            }
        }

        private async Task EnsureEmailIsUniqueOrThrowAsync(Guid id, string email)
        {
            if (await _userRepository.ExistsByEmailAsync(email, id))
            {
                throw new ConflictException(BusinessErrorMessages.UserEmailAlreadyExists);
            }
        }

        private async Task<User> GetUserByIdOrThrowAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new NotFoundException(BusinessErrorMessages.UserNotFound);
            }
            return user;
        }

        private async Task<User> GetUserByIdWithLeavesOrThrowAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdWithLeavesAsync(id);
            if (user is null)
            {
                throw new NotFoundException(BusinessErrorMessages.UserNotFound);
            }
            return user;
        }
        #endregion
    }
}
