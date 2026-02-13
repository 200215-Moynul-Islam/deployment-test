using ELTBackend.Models;

namespace ELTBackend.Repositories
{
    public interface IRepositoryBase<T>
        where T : EntityBase
    {
        Task CreateAsync(T entity);
        Task SaveChangesAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<bool> ExistsByIdAsync(Guid id);
    }
}
