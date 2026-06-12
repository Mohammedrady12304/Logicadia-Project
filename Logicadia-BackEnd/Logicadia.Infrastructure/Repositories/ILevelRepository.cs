using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Repositories
{
    public interface ILevelRepository
    {
        Task<IEnumerable<Level>> GetAllAsync();
        Task<(IEnumerable<Level> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize); // ✅

        Task<Level?> GetByIdAsync(int id);
        Task AddAsync(Level level);
        Task UpdateAsync(Level level);
        Task DeleteAsync(Level level);
    }
}
