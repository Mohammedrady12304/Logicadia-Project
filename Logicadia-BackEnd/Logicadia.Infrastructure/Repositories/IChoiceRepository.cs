using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Repositories
{
    public interface IChoiceRepository
    {
        Task<IEnumerable<Choice>> GetAllAsync();
        Task<(IEnumerable<Choice> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<(IEnumerable<Choice> Data, int TotalCount)> GetPagedByScenarioIdAsync(int scenarioId, int pageNumber, int pageSize);
        Task<IEnumerable<Choice>> GetByScenarioIdAsync(int scenarioId);
        Task<Choice?> GetByIdAsync(int id);
        Task AddAsync(Choice choice);
        Task UpdateAsync(Choice choice);
        Task DeleteAsync(Choice choice);
    }
}
