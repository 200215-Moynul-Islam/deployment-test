using ELTBackend.Data;
using ELTBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ELTBackend.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(EmployeeLeaveTrackerDbContext dbContext)
            : base(dbContext) { }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email, Guid id)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.Id != id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _dbSet.Where(u => u.Role == role && !u.IsDeleted).ToListAsync();
        }

        public async Task<User?> GetUserByIdWithLeavesAsync(Guid id)
        {
            return await _dbSet
                .Include(u => u.Leaves.Where(l => !l.IsDeleted))
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
