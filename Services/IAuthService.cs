using ELTBackend.DTOs;

namespace ELTBackend.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
