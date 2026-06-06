using LOGICADIA.Data;
using LOGICADIA.DTOs;
using LOGICADIA.Models;
using Microsoft.EntityFrameworkCore;

namespace LOGICADIA.Services
{
    public interface IChoiceService
    {
        Task<ChoiceSubmitResponse> SubmitChoiceAsync(int userId, int scenarioId, int choiceId);
        Task<List<ChoiceDTO>> GetChoicesByScenarioAsync(int scenarioId);
    }

    public class ChoiceService : IChoiceService
    {
        private readonly AppDbContext _context;
        private readonly IAchievementEngine _achievementEngine;
        private readonly ILevelUnlockService _levelUnlockService;

        public ChoiceService(AppDbContext context, IAchievementEngine achievementEngine, ILevelUnlockService levelUnlockService)
        {
            _context = context;
            _achievementEngine = achievementEngine;
            _levelUnlockService = levelUnlockService;
        }

        public async Task<ChoiceSubmitResponse> SubmitChoiceAsync(int userId, int scenarioId, int choiceId)
        {
            var choice = await _context.Choices.FindAsync(choiceId);
            if (choice == null || choice.ScenarioId != scenarioId)
                throw new InvalidOperationException("Invalid choice selection");

            var scenario = await _context.Scenarios
                .Include(s => s.Story)
                .FirstOrDefaultAsync(s => s.Id == scenarioId);

            if (scenario == null)
                throw new InvalidOperationException("Scenario not found");

            var existingProgress = await _context.UserProgress
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ScenarioId == scenarioId);

            if (existingProgress != null)
                throw new InvalidOperationException("Scenario already completed");

            var xpEarned = choice.IsCorrect ? choice.XpValue : choice.XpValue / 2;

            var userProgress = new UserProgress
            {
                UserId = userId,
                ScenarioId = scenarioId,
                ChosenChoiceId = choiceId,
                IsCorrect = choice.IsCorrect,
                XpEarned = xpEarned,
                CompletedAt = DateTime.UtcNow
            };

            _context.UserProgress.Add(userProgress);

            var levelUnlocked = await _levelUnlockService.CheckAndUnlockNextLevelAsync(userId, scenario.Story.LevelId);

            var achievementsUnlocked = await _achievementEngine.CheckAndUnlockAchievementsAsync(userId);

            await _context.SaveChangesAsync();

            return new ChoiceSubmitResponse
            {
                IsCorrect = choice.IsCorrect,
                XpEarned = xpEarned,
                Feedback = choice.Feedback,
                LevelUnlocked = levelUnlocked,
                AchievementsUnlocked = achievementsUnlocked
            };
        }

        public async Task<List<ChoiceDTO>> GetChoicesByScenarioAsync(int scenarioId)
        {
            return await _context.Choices
                .Where(c => c.ScenarioId == scenarioId)
                .OrderBy(c => c.Id)
                .Select(c => new ChoiceDTO
                {
                    Id = c.Id,
                    ChoiceText = c.ChoiceText,
                    IsCorrect = c.IsCorrect,
                    Feedback = c.Feedback,
                    XpValue = c.XpValue
                })
                .ToListAsync();
        }
    }
}
