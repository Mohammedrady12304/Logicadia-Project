using Logicadia.Infrastructure.Data;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.Infrastructure.Services.Services
{
    public interface IScoreService
    {
        Task<int> CalculateTotalXpAsync(int userId);
        Task<int> GetCurrentLevelAsync(int userId);
        Task<int> GetXpToNextLevelAsync(int userId);
        Task<double> CalculateAccuracyAsync(int userId);
    }

    public class ScoreService : IScoreService
    {
        private readonly ApplicationDbContext _context;
        private const int XP_PER_LEVEL = 1000;

        public ScoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CalculateTotalXpAsync(int userId)
        {
            return await _context.UserProgress
                .Where(p => p.UserId == userId)
                .SumAsync(p => p.XpEarned);
        }

        public async Task<int> GetCurrentLevelAsync(int userId)
        {
            var totalXp = await CalculateTotalXpAsync(userId);
            return (totalXp / XP_PER_LEVEL) + 1;
        }

        public async Task<int> GetXpToNextLevelAsync(int userId)
        {
            var totalXp = await CalculateTotalXpAsync(userId);
            var currentLevel = await GetCurrentLevelAsync(userId);
            var xpNeededForCurrentLevel = (currentLevel - 1) * XP_PER_LEVEL;
            return XP_PER_LEVEL - (totalXp - xpNeededForCurrentLevel);
        }

        public async Task<double> CalculateAccuracyAsync(int userId)
        {
            var totalAttempts = await _context.UserProgress
                .CountAsync(p => p.UserId == userId);

            if (totalAttempts == 0) return 0;

            var correctAnswers = await _context.UserProgress
                .Where(p => p.UserId == userId && p.IsCorrect)
                .CountAsync();

            return (double)correctAnswers / totalAttempts * 100;
        }
    }
}
