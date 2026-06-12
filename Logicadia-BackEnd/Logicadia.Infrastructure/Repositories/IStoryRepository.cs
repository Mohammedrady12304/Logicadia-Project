using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Repositories
{
    public interface IStoryRepository
    {
        Task<IEnumerable<Story>> GetAllAsync();
        Task<(IEnumerable<Story> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Story> Data, int TotalCount)> GetPagedByLevelIdAsync(int levelId, int pageNumber, int pageSize);
        Task<IEnumerable<Story>> GetByLevelIdAsync(int levelId);
        Task<Story?> GetByIdAsync(int id);
        Task AddAsync(Story story);
        Task UpdateAsync(Story story);
        Task DeleteAsync(Story story);
    }
}
