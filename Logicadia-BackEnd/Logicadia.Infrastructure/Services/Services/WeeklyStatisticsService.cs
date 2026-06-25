using Logicadia.Infrastructure.Data;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.Infrastructure.Services.Services
{
    public interface IWeeklyStatisticsService
    {
        Task<WeeklyStatisticsDTO> GetCurrentWeekStatisticsAsync(int userId);
        Task<WeeklyStatisticsDTO> GetWeekStatisticsAsync(int userId, int weekNumber, int year);
        Task<List<WeeklyStatisticsDTO>> GetAllWeeksStatisticsAsync(int userId);
    }

    public class WeeklyStatisticsService : IWeeklyStatisticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoreService _scoreService;

        public WeeklyStatisticsService(ApplicationDbContext context, IScoreService scoreService)
        {
            _context = context;
            _scoreService = scoreService;
        }

        public async Task<WeeklyStatisticsDTO> GetCurrentWeekStatisticsAsync(int userId)
        {
            var now = DateTime.UtcNow;
            var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
            var weekNumber = calendar.GetWeekOfYear(now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return await GetWeekStatisticsAsync(userId, weekNumber, now.Year);
        }

        public async Task<WeeklyStatisticsDTO> GetWeekStatisticsAsync(int userId, int weekNumber, int year)
        {
            var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
            var firstMonday = jan1.AddDays(daysOffset);
            var weekStart = firstMonday.AddDays((weekNumber - 1) * 7);
            var weekEnd = weekStart.AddDays(7).AddSeconds(-1);

            var weekProgress = await _context.UserProgress
                .Where(p => p.UserId == userId && p.CompletedAt >= weekStart && p.CompletedAt <= weekEnd)
                .ToListAsync();

            var scenariosCompleted = weekProgress.Select(p => p.ScenarioId).Distinct().Count();
            var correctAnswers = weekProgress.Count(p => p.IsCorrect);
            var totalAnswers = weekProgress.Count;
            var xpEarned = weekProgress.Sum(p => p.XpEarned);
            var achievementsUnlocked = await _context.UserAchievements
                .Where(ua => ua.UserId == userId && ua.UnlockedAt >= weekStart && ua.UnlockedAt <= weekEnd)
                .CountAsync();

            var accuracy = totalAnswers > 0 ? (double)correctAnswers / totalAnswers * 100 : 0;

            return new WeeklyStatisticsDTO
            {
                WeekNumber = weekNumber,
                Year = year,
                XpEarned = xpEarned,
                ScenariosCompleted = scenariosCompleted,
                AchievementsUnlocked = achievementsUnlocked,
                AccuracyPercentage = accuracy,
                StartDate = weekStart,
                EndDate = weekEnd
            };
        }

        public async Task<List<WeeklyStatisticsDTO>> GetAllWeeksStatisticsAsync(int userId)
        {
            var allProgress = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.CompletedAt)
                .ToListAsync();

            if (allProgress.Count == 0) return new List<WeeklyStatisticsDTO>();

            var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
            var weekStats = new Dictionary<(int, int), WeeklyStatisticsDTO>();

            foreach (var progress in allProgress)
            {
                var weekNumber = calendar.GetWeekOfYear(progress.CompletedAt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                var key = (weekNumber, progress.CompletedAt.Year);

                if (!weekStats.ContainsKey(key))
                {
                    weekStats[key] = await GetWeekStatisticsAsync(userId, weekNumber, progress.CompletedAt.Year);
                }
            }

            return weekStats.Values.OrderByDescending(w => new DateTime(w.Year, 1, 1).AddDays((w.WeekNumber - 1) * 7)).ToList();
        }
    }
}
