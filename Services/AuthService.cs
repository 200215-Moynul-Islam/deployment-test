using System.Security.Claims;
using ELTBackend.Constants;
using ELTBackend.DTOs;
using ELTBackend.Exceptions;
using ELTBackend.Models;
using ELTBackend.Repositories;
using ELTBackend.Utilities;

namespace ELTBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IConfiguration _config;

        public AuthService(
            IUserRepository userRepository,
            IJwtHelper jwtHelper,
            IConfiguration configuration
        )
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _config = configuration;
        }

        public async Task<string> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await GetUserByEmailOrThrowAsync(loginRequestDto.Email!);
            EnsurePasswordIsCorrectOrThrowAsync(loginRequestDto.Password!, user.PasswordHash!);

            // Generate the claims for the token
            ICollection<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var expiredInMinutes = Convert.ToDouble(_config["JwtSettings:ExpiresInMinutes"]);
            var token = _jwtHelper.GenerateToken(claims, expiredInMinutes);

            return token;
        }

        #region Private Methods
        private async Task<User> GetUserByEmailOrThrowAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidCredentialsException(BusinessErrorMessages.UserInvalidEmail);
            }
            return user;
        }

        private void EnsurePasswordIsCorrectOrThrowAsync(string password, string passwordHash)
        {
            if (!BCrypt.Net.BCrypt.Verify(password, passwordHash))
            {
                throw new InvalidCredentialsException(BusinessErrorMessages.UserInvalidPassword);
            }
            return;
        }
        #endregion
    }
}
