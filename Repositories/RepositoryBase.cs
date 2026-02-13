using ELTBackend.Data;
using ELTBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ELTBackend.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : EntityBase
    {
        protected readonly EmployeeLeaveTrackerDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(EmployeeLeaveTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && !e.IsDeleted);
        }
    }
}
