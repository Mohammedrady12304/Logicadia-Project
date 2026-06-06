using LOGICADIA.Data;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LOGICADIA.Services
{
    public interface ILeaderboardService
    {
        Task<List<LeaderboardEntryDTO>> GetTopLeaderboardAsync(int topCount = 100);
        Task<LeaderboardEntryDTO?> GetUserLeaderboardPositionAsync(int userId);
        Task<List<LeaderboardEntryDTO>> GetLeaderboardAroundUserAsync(int userId, int range = 10);
    }

    public class LeaderboardService : ILeaderboardService
    {
        private readonly AppDbContext _context;

        public LeaderboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaderboardEntryDTO>> GetTopLeaderboardAsync(int topCount = 100)
        {
            var leaderboard = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    TotalXp = u.Progress.Sum(p => p.XpEarned),
                    AchievementsCount = u.UserAchievements.Count
                })
                .OrderByDescending(x => x.TotalXp)
                .Take(topCount)
                .ToListAsync();

            var result = new List<LeaderboardEntryDTO>();
            for (int i = 0; i < leaderboard.Count; i++)
            {
                var user = leaderboard[i];
                var level = (user.TotalXp / 1000) + 1;

                result.Add(new LeaderboardEntryDTO
                {
                    Rank = i + 1,
                    UserId = user.Id,
                    Username = user.UserName!,
                    TotalXp = user.TotalXp,
                    Level = level,
                    AchievementsCount = user.AchievementsCount
                });
            }

            return result;
        }

        public async Task<LeaderboardEntryDTO?> GetUserLeaderboardPositionAsync(int userId)
        {
            var user = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    TotalXp = u.Progress.Sum(p => p.XpEarned),
                    AchievementsCount = u.UserAchievements.Count
                })
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;

            var rank = await _context.Users
                .Where(u => u.Progress.Sum(p => p.XpEarned) > user.TotalXp)
                .CountAsync() + 1;

            var level = (user.TotalXp / 1000) + 1;

            return new LeaderboardEntryDTO
            {
                Rank = rank,
                UserId = user.Id,
                Username = user.UserName!,
                TotalXp = user.TotalXp,
                Level = level,
                AchievementsCount = user.AchievementsCount
            };
        }

        public async Task<List<LeaderboardEntryDTO>> GetLeaderboardAroundUserAsync(int userId, int range = 10)
        {
            var userPosition = await GetUserLeaderboardPositionAsync(userId);
            if (userPosition == null) return new List<LeaderboardEntryDTO>();

            var startRank = Math.Max(1, userPosition.Rank - range);
            var endRank = userPosition.Rank + range;

            var leaderboard = await GetTopLeaderboardAsync(endRank + range);
            return leaderboard
                .Where(x => x.Rank >= startRank && x.Rank <= endRank)
                .ToList();
        }
    }
}
