using LOGICADIA.Data;
using LOGICADIA.Models;
using Microsoft.EntityFrameworkCore;

namespace LOGICADIA.Services
{
    public interface ILevelUnlockService
    {
        Task<bool> CheckAndUnlockNextLevelAsync(int userId, int currentLevelId);
        Task<bool> IsNextLevelReadyToUnlockAsync(int userId, int levelId);
    }

    public class LevelUnlockService : ILevelUnlockService
    {
        private readonly AppDbContext _context;
        private const double UNLOCK_THRESHOLD_PERCENTAGE = 0.8;

        public LevelUnlockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckAndUnlockNextLevelAsync(int userId, int currentLevelId)
        {
            var isNextReady = await IsNextLevelReadyToUnlockAsync(userId, currentLevelId);
            return isNextReady;
        }

        public async Task<bool> IsNextLevelReadyToUnlockAsync(int userId, int levelId)
        {
            var currentLevel = await _context.Levels.FindAsync(levelId);
            if (currentLevel == null) return false;

            var nextLevel = await _context.Levels
                .FirstOrDefaultAsync(l => l.OrderIndex == currentLevel.OrderIndex + 1);

            if (nextLevel == null) return false;

            var totalScenarios = await _context.Scenarios
                .Where(s => s.Story.LevelId == levelId)
                .CountAsync();

            var completedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == levelId)
                .Distinct()
                .CountAsync();

            var completionPercentage = (double)completedScenarios / totalScenarios;

            return completionPercentage >= UNLOCK_THRESHOLD_PERCENTAGE;
        }
    }
}
