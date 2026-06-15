using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly ApplicationDbContext _context;

        public AchievementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Achievement>> GetAllAsync()
        {
            return await _context.Achievements
                .Include(a => a.UserAchievements)
                .ToListAsync();
        }
        public async Task<(IEnumerable<Achievement> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Achievements.CountAsync();
            var data = await _context.Achievements
                .Include(a => a.UserAchievements)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalCount);
        }
        public async Task<Achievement?> GetByIdAsync(int id)
        {
            return await _context.Achievements
                .Include(a => a.UserAchievements)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Achievement achievement)
        {
            await _context.Achievements.AddAsync(achievement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Achievement achievement)
        {
            _context.Achievements.Update(achievement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Achievement achievement)
        {
            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();
        }
    }
}
