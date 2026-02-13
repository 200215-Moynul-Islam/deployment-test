using ELTBackend.Models;

namespace ELTBackend.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email, Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<User?> GetUserByIdWithLeavesAsync(Guid id);
    }
}
