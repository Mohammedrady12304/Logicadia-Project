using Logicadia.Infrastructure.Data;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.Infrastructure.Services.Services
{
    public interface IUserProgressService
    {
        Task<UserProgressDTO> GetUserProgressAsync(int userId);
        Task<int> GetScenariosCompletedAsync(int userId);
        Task<int> GetCorrectAnswersAsync(int userId);
    }

    public class UserProgressService : IUserProgressService
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoreService _scoreService;

        public UserProgressService(ApplicationDbContext context, IScoreService scoreService)
        {
            _context = context;
            _scoreService = scoreService;
        }

        public async Task<UserProgressDTO> GetUserProgressAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found");

            var totalXp = await _scoreService.CalculateTotalXpAsync(userId);
            var currentLevel = await _scoreService.GetCurrentLevelAsync(userId);
            var xpToNextLevel = await _scoreService.GetXpToNextLevelAsync(userId);
            var scenariosCompleted = await GetScenariosCompletedAsync(userId);
            var correctAnswers = await GetCorrectAnswersAsync(userId);
            var accuracy = await _scoreService.CalculateAccuracyAsync(userId);

            var lastActivity = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CompletedAt)
                .Select(p => p.CompletedAt)
                .FirstOrDefaultAsync();

            return new UserProgressDTO
            {
                TotalXp = totalXp,
                CurrentLevel = currentLevel,
                CurrentLevelProgress = (currentLevel - 1) * 1000,
                XpToNextLevel = xpToNextLevel,
                ScenariosCompleted = scenariosCompleted,
                CorrectAnswers = correctAnswers,
                AccuracyPercentage = accuracy,
                JoinDate = user.CreatedAt,
                LastActivityDate = lastActivity != default ? lastActivity : null
            };
        }

        public async Task<int> GetScenariosCompletedAsync(int userId)
        {
            return await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Select(p => p.ScenarioId)
                .Distinct()
                .CountAsync();
        }

        public async Task<int> GetCorrectAnswersAsync(int userId)
        {
            return await _context.UserProgress
                .Where(p => p.UserId == userId && p.IsCorrect)
                .CountAsync();
        }
    }
}
