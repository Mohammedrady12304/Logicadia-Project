using Logicadia.Infrastructure.Data;
using LOGICADIA.DTOs;
using Logicadia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Logicadia.Application.Features.DTOs.Stories;
using Logicadia.Application.Features.DTOs.Scenario;

namespace Logicadia.Infrastructure.Services.Services
{
    public interface IStoryService
    {
        Task<List<StoryDTO>> GetStoriesByLevelAsync(int levelId, int userId);
        Task<StoryDetailDTO?> GetStoryByIdAsync(int storyId, int userId);
    }

    public class StoryService : IStoryService
    {
        private readonly ApplicationDbContext _context;

        public StoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StoryDTO>> GetStoriesByLevelAsync(int levelId, int userId)
        {
            var stories = await _context.Stories
                .Where(s => s.LevelId == levelId)
                .OrderBy(s => s.OrderIndex)
                .Include(s => s.Scenarios)
                .ToListAsync();

            var userCompletedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == levelId)
                .Select(p => p.ScenarioId)
                .ToListAsync();

            return stories.Select(s => new StoryDTO
            {
                Id = s.Id,
                LevelId = s.LevelId,
                Title = s.Title,
                NarrativeText = s.NarrativeText,
                OrderIndex = s.OrderIndex,
                IsCompleted = s.Scenarios.All(sc => userCompletedScenarios.Contains(sc.Id))
            }).ToList();
        }

        public async Task<StoryDetailDTO?> GetStoryByIdAsync(int storyId, int userId)
        {
            var story = await _context.Stories
                .Include(s => s.Scenarios.OrderBy(sc => sc.OrderIndex))
                .FirstOrDefaultAsync(s => s.Id == storyId);

            if (story == null) return null;

            var userCompletedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.ScenarioId == storyId)
                .Select(p => p.ScenarioId)
                .Distinct()
                .ToListAsync();

            return new StoryDetailDTO
            {
                Id = story.Id,
                LevelId = story.LevelId,
                Title = story.Title,
                NarrativeText = story.NarrativeText,
                OrderIndex = story.OrderIndex,
                IsCompleted = story.Scenarios.All(sc => userCompletedScenarios.Contains(sc.Id)),
                Scenarios = story.Scenarios.Select(sc => new ScenarioDTO
                {
                    Id = sc.Id,
                    StoryId = sc.StoryId,
                    Title = sc.Title,
                    Description = sc.Description,
                    OrderIndex = sc.OrderIndex,
                    IsCompleted = userCompletedScenarios.Contains(sc.Id)
                }).ToList()
            };
        }
    }
}
