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
    public class ScenarioRepository    : IScenarioRepository
    {
        
            private readonly ApplicationDbContext _context;

        public ScenarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

            public async Task<IEnumerable<Scenario>> GetAllAsync()
            {
                return await _context.Scenarios
                    .Include(s => s.Choices)
                    .OrderBy(s => s.OrderIndex)
                    .ToListAsync();
            }
            public async Task<(IEnumerable<Scenario> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
            {
                var totalCount = await _context.Scenarios.CountAsync();
                var data = await _context.Scenarios
                    .Include(s => s.Choices)
                    .OrderBy(s => s.OrderIndex)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return (data, totalCount);
            }
            public async Task<(IEnumerable<Scenario> Data, int TotalCount)> GetPagedByStoryIdAsync(int storyId, int pageNumber, int pageSize)
            {
                var totalCount = await _context.Scenarios.Where(s => s.StoryId == storyId).CountAsync();
                var data = await _context.Scenarios
                    .Include(s => s.Choices)
                    .Where(s => s.StoryId == storyId)
                    .OrderBy(s => s.OrderIndex)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return (data, totalCount);
            }
            public async Task<IEnumerable<Scenario>> GetByStoryIdAsync(int storyId)
            {
                return await _context.Scenarios
                    .Include(s => s.Choices)
                    .Where(s => s.StoryId == storyId)
                    .OrderBy(s => s.OrderIndex)
                    .ToListAsync();
            }

            public async Task<Scenario?> GetByIdAsync(int id)
            {
                return await _context.Scenarios
                    .Include(s => s.Choices)
                    .FirstOrDefaultAsync(s => s.Id == id);
            }

            public async Task AddAsync(Scenario scenario)
            {
                await _context.Scenarios.AddAsync(scenario);
                await _context.SaveChangesAsync();
            }
            public async Task UpdateAsync(Scenario scenario)
            {
                _context.Scenarios.Update(scenario);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(Scenario scenario)
            {
                _context.Scenarios.Remove(scenario);
                await _context.SaveChangesAsync();
            }
        }
    }



