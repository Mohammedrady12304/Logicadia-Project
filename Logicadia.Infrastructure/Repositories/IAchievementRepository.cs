using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Repositories
{
    public interface IAchievementRepository
    {
        
            Task<IEnumerable<Achievement>> GetAllAsync();
            Task<(IEnumerable<Achievement> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

            Task<Achievement?> GetByIdAsync(int id);
            Task AddAsync(Achievement achievement);
            Task UpdateAsync(Achievement achievement);
            Task DeleteAsync(Achievement achievement);
        }
    }

