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
    public class StoryRepository : IStoryRepository
    {
        private readonly AppDbContext _context;

        public StoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Story>> GetAllAsync()
        {
            return await _context.Stories
                .Include(s => s.Scenarios)
                .OrderBy(s => s.OrderIndex)
                .ToListAsync();
        }
        public async Task<(IEnumerable<Story> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Stories.CountAsync();
            var data = await _context.Stories
                .Include(s => s.Scenarios)
                .OrderBy(s => s.OrderIndex)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalCount);
        }
        public async Task<(IEnumerable<Story> Data, int TotalCount)> GetPagedByLevelIdAsync(int levelId, int pageNumber, int pageSize)
        {
            var totalCount = await _context.Stories.Where(s => s.LevelId == levelId).CountAsync();
            var data = await _context.Stories
                .Include(s => s.Scenarios)
                .Where(s => s.LevelId == levelId)
                .OrderBy(s => s.OrderIndex)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalCount);
        }
        public async Task<IEnumerable<Story>> GetByLevelIdAsync(int levelId)
        {
            return await _context.Stories
                .Include(s => s.Scenarios)
                .Where(s => s.LevelId == levelId)
                .OrderBy(s => s.OrderIndex)
                .ToListAsync();
        }
        public async Task<Story?> GetByIdAsync(int id)
        {
            return await _context.Stories
                .Include(s => s.Scenarios)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Story story)
        {
            _context.Stories.Update(story);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Story story)
        {
            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();
        }
    }
}
