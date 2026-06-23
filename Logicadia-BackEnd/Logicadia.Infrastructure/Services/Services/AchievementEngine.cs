using Logicadia.Infrastructure.Data;
using LOGICADIA.DTOs;
using Logicadia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.Infrastructure.Services.Services
{
    public interface IAchievementEngine
    {
        Task<List<AchievementUnlockedDTO>> CheckAndUnlockAchievementsAsync(int userId);
        Task<List<UserAchievementDTO>> GetUserAchievementsAsync(int userId);
    }

    public class AchievementEngine : IAchievementEngine
    {
        private readonly ApplicationDbContext _context;

        public AchievementEngine(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AchievementUnlockedDTO>> CheckAndUnlockAchievementsAsync(int userId)
        {
            var unlockedAchievements = new List<AchievementUnlockedDTO>();
            var achievements = await _context.Achievements.ToListAsync();

            var userProgress = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var userAchievements = await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.AchievementId)
                .ToListAsync();

            foreach (var achievement in achievements)
            {
                if (userAchievements.Contains(achievement.Id))
                    continue;

                if (ShouldUnlockAchievement(achievement, userProgress))
                {
                    var userAchievement = new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.Id,
                        UnlockedAt = DateTime.UtcNow
                    };

                    _context.UserAchievements.Add(userAchievement);

                    unlockedAchievements.Add(new AchievementUnlockedDTO
                    {
                        Id = achievement.Id,
                        Title = achievement.Title,
                        Description = achievement.Description,
                        IconUrl = achievement.IconUrl,
                        TriggerType = achievement.TriggerType,
                        TriggerValue = achievement.TriggerValue,
                        UnlockedAt = DateTime.UtcNow
                    });
                }
            }

            if (unlockedAchievements.Count > 0)
                await _context.SaveChangesAsync();

            return unlockedAchievements;
        }

        public async Task<List<UserAchievementDTO>> GetUserAchievementsAsync(int userId)
        {
            return await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Include(ua => ua.Achievement)
                .Select(ua => new UserAchievementDTO
                {
                    Id = ua.Achievement.Id,
                    Title = ua.Achievement.Title,
                    Description = ua.Achievement.Description,
                    IconUrl = ua.Achievement.IconUrl,
                    TriggerType = ua.Achievement.TriggerType,
                    TriggerValue = ua.Achievement.TriggerValue,
                    UnlockedAt = ua.UnlockedAt,
                    Rarity = CalculateRarity(ua.Achievement.TriggerValue)
                })
                .OrderByDescending(a => a.UnlockedAt)
                .ToListAsync();
        }

        private bool ShouldUnlockAchievement(Achievement achievement, List<UserProgress> userProgress)
        {
            return achievement.TriggerType switch
            {
                "CORRECT_ANSWERS" => userProgress.Count(p => p.IsCorrect) >= achievement.TriggerValue,
                "SCENARIOS_COMPLETED" => userProgress.Select(p => p.ScenarioId).Distinct().Count() >= achievement.TriggerValue,
                "PERFECT_ACCURACY" => userProgress.Count > 0 && 
                    (double)userProgress.Count(p => p.IsCorrect) / userProgress.Count >= (achievement.TriggerValue / 100.0),
                "TOTAL_XP" => userProgress.Sum(p => p.XpEarned) >= achievement.TriggerValue,
                _ => false
            };
        }

        private int CalculateRarity(int triggerValue)
        {
            return triggerValue switch
            {
                < 50 => 1,    // Common
                < 200 => 2,   // Rare
                < 500 => 3,   // Epic
                _ => 4        // Legendary
            };
        }
    }
}
