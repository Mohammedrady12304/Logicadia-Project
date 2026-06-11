using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Repositories
{
    public interface IScenarioRepository
    {
        Task<IEnumerable<Scenario>> GetAllAsync();
        Task<(IEnumerable<Scenario> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Scenario> Data, int TotalCount)> GetPagedByStoryIdAsync(int storyId, int pageNumber, int pageSize);
        Task<IEnumerable<Scenario>> GetByStoryIdAsync(int storyId);
        Task<Scenario?> GetByIdAsync(int id);
        Task AddAsync(Scenario scenario);
        Task UpdateAsync(Scenario scenario);
        Task DeleteAsync(Scenario scenario);
    }
}
