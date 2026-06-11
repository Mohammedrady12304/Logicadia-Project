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
    public class ChoiceRepository : IChoiceRepository
    {
        private readonly AppDbContext _context;

        public ChoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Choice>> GetAllAsync()
        {
            return await _context.Choices
                .OrderBy(c => c.ScenarioId)
                .ToListAsync();
        }
        public async Task<(IEnumerable<Choice> Data, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Choices.CountAsync();
            var data = await _context.Choices
                .OrderBy(c => c.ScenarioId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalCount);
        }
        public async Task<(IEnumerable<Choice> Data, int TotalCount)> GetPagedByScenarioIdAsync(int scenarioId, int pageNumber, int pageSize)
        {
            var totalCount = await _context.Choices.Where(c => c.ScenarioId == scenarioId).CountAsync();
            var data = await _context.Choices
                .Where(c => c.ScenarioId == scenarioId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (data, totalCount);
        }

        public async Task<IEnumerable<Choice>> GetByScenarioIdAsync(int scenarioId)
        {
            return await _context.Choices
                .Where(c => c.ScenarioId == scenarioId)
                .ToListAsync();
        }

        public async Task<Choice?> GetByIdAsync(int id)
        {
            return await _context.Choices
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Choice choice)
        {
            await _context.Choices.AddAsync(choice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Choice choice)
        {
            _context.Choices.Update(choice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Choice choice)
        {
            _context.Choices.Remove(choice);
            await _context.SaveChangesAsync();
        }
    }
}
