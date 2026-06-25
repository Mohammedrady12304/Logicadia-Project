using Logicadia.Infrastructure.Data;
using LOGICADIA.DTOs;
using Logicadia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Logicadia.Application.Features.DTOs.Stories;

namespace Logicadia.Infrastructure.Services.Services
{
    public interface ILevelService
    {
        Task<List<LevelDTO>> GetAllLevelsAsync(int userId);
        Task<LevelDetailDTO?> GetLevelByIdAsync(int levelId, int userId);
        Task<bool> IsLevelUnlockedAsync(int levelId, int userId);
    }

    public class LevelService : ILevelService
    {
        private readonly ApplicationDbContext _context;

        public LevelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LevelDTO>> GetAllLevelsAsync(int userId)
        {
            var levels = await _context.Levels
                .OrderBy(l => l.OrderIndex)
                .ToListAsync();

            var userProgress = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Select(p => p.Scenario.Story.LevelId)
                .Distinct()
                .ToListAsync();

            return levels.Select((l, index) =>
            {
                bool isUnlocked = index == 0 || userProgress.Contains(l.Id);
                var firstLevelProgress = _context.UserProgress
                    .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == l.Id)
                    .OrderByDescending(p => p.CompletedAt)
                    .FirstOrDefault();

                return new LevelDTO
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    OrderIndex = l.OrderIndex,
                    XpReward = l.XpReward,
                    IsUnlocked = isUnlocked,
                    UnlockedAt = firstLevelProgress?.CompletedAt
                };
            }).ToList();
        }

        public async Task<LevelDetailDTO?> GetLevelByIdAsync(int levelId, int userId)
        {
            var level = await _context.Levels
                .Include(l => l.Stories.OrderBy(s => s.OrderIndex))
                .ThenInclude(s => s.Scenarios.OrderBy(sc => sc.OrderIndex))
                .FirstOrDefaultAsync(l => l.Id == levelId);

            if (level == null) return null;

            var isUnlocked = await IsLevelUnlockedAsync(levelId, userId);
            if (!isUnlocked) return null;

            var userCompletedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == levelId)
                .Select(p => p.ScenarioId)
                .ToListAsync();

            var levelDetail = new LevelDetailDTO
            {
                Id = level.Id,
                Title = level.Title,
                Description = level.Description,
                OrderIndex = level.OrderIndex,
                XpReward = level.XpReward,
                IsUnlocked = true,
                Stories = level.Stories.Select(s => new StoryDTO
                {
                    Id = s.Id,
                    LevelId = s.LevelId,
                    Title = s.Title,
                    NarrativeText = s.NarrativeText,
                    OrderIndex = s.OrderIndex,
                    IsCompleted = s.Scenarios.All(sc => userCompletedScenarios.Contains(sc.Id))
                }).ToList()
            };

            return levelDetail;
        }

        public async Task<bool> IsLevelUnlockedAsync(int levelId, int userId)
        {
            var level = await _context.Levels.FindAsync(levelId);
            if (level == null) return false;

            if (level.OrderIndex == 0) return true;

            var previousLevelId = await _context.Levels
                .Where(l => l.OrderIndex == level.OrderIndex - 1)
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            if (previousLevelId == 0) return false;

            return await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == previousLevelId)
                .AnyAsync();
        }
    }
}
