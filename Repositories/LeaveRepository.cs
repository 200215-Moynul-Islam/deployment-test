using ELTBackend.Constants;
using ELTBackend.Data;
using ELTBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ELTBackend.Repositories
{
    public class LeaveRepository : RepositoryBase<Leave>, ILeaveRepository
    {
        public LeaveRepository(EmployeeLeaveTrackerDbContext dbContext)
            : base(dbContext) { }

        public async Task<IEnumerable<Leave>> GetAllLeavesWithEmployeeAsync()
        {
            return await _dbSet
                .Where(l => !l.IsDeleted)
                .Include(l => l.User)
                .Where(l => l.User.Role == Roles.Employee)
                .ToListAsync();
        }
    }
}
