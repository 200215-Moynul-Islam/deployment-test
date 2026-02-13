using ELTBackend.Models;

namespace ELTBackend.Repositories
{
    public interface ILeaveRepository : IRepositoryBase<Leave>
    {
        Task<IEnumerable<Leave>> GetAllLeavesWithEmployeeAsync();
    }
}
