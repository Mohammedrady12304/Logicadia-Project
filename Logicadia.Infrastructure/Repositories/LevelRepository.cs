using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logicadia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Logicadia.Infrastructure.Data;


namespace Logicadia.Infrastructure.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private readonly AppDbContext _context;

        public LevelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Level>> GetAllAsync()
        {
            return await _context.Levels
                .Include(l => l.Stories)
                .OrderBy(l => l.OrderIndex)
                .ToListAsync();
        }
        public async Task<(IEnumerable<Level> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Levels.CountAsync();
            var data = await _context.Levels
                .Include(l => l.Stories)
                .OrderBy(l => l.OrderIndex)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }
        public async Task<Level?> GetByIdAsync(int id)
        {
            return await _context.Levels
                .Include(l => l.Stories)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(Level level)
        {
            await _context.Levels.AddAsync(level);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Level level)
        {
            _context.Levels.Update(level);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Level level)
        {
            _context.Levels.Remove(level);
            await _context.SaveChangesAsync();
        }
    }
}
