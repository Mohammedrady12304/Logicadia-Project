using Logicadia.Infrastructure.Data;
using LOGICADIA.DTOs;
using Logicadia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Application.Features.DTOs.Choice;

namespace Logicadia.Infrastructure.Services.Services
{
    public interface IScenarioService
    {
        Task<ScenarioDetailDTO?> GetScenarioByIdAsync(int scenarioId, int userId);
        Task<List<ScenarioDTO>> GetScenariosByStoryAsync(int storyId, int userId);
    }

    public class ScenarioService : IScenarioService
    {
        private readonly ApplicationDbContext _context;

        public ScenarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ScenarioDetailDTO?> GetScenarioByIdAsync(int scenarioId, int userId)
        {
            var scenario = await _context.Scenarios
                .Include(s => s.Choices.OrderBy(c => c.Id))
                .FirstOrDefaultAsync(s => s.Id == scenarioId);

            if (scenario == null) return null;

            var userAttempt = await _context.UserProgress
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ScenarioId == scenarioId);

            var isCompleted = userAttempt != null;

            return new ScenarioDetailDTO
            {
                Id = scenario.Id,
                StoryId = scenario.StoryId,
                Title = scenario.Title,
                Description = scenario.Description,
                OrderIndex = scenario.OrderIndex,
                IsCompleted = isCompleted,
                Choices = scenario.Choices.Select(c => new ChoiceDTO
                {
                    Id = c.Id,
                    ChoiceText = c.ChoiceText,
                    IsCorrect = c.IsCorrect,
                    Feedback = c.Feedback,
                    XpValue = c.XpValue
                }).ToList()
            };
        }

        public async Task<List<ScenarioDTO>> GetScenariosByStoryAsync(int storyId, int userId)
        {
            var scenarios = await _context.Scenarios
                .Where(s => s.StoryId == storyId)
                .OrderBy(s => s.OrderIndex)
                .ToListAsync();

            var userCompletedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.StoryId == storyId)
                .Select(p => p.ScenarioId)
                .ToListAsync();

            return scenarios.Select(s => new ScenarioDTO
            {
                Id = s.Id,
                StoryId = s.StoryId,
                Title = s.Title,
                Description = s.Description,
                OrderIndex = s.OrderIndex,
                IsCompleted = userCompletedScenarios.Contains(s.Id)
            }).ToList();
        }
    }
}
