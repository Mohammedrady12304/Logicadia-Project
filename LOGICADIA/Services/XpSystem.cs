using LOGICADIA.Data;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LOGICADIA.Services
{
    public interface IXpSystem
    {
        Task<int> GetUserLevelAsync(int userId);
        Task<int> GetUserTotalXpAsync(int userId);
        Task<int> GetXpProgressToNextLevelAsync(int userId);
    }

    public class XpSystem : IXpSystem
    {
        private readonly AppDbContext _context;
        private readonly IScoreService _scoreService;
        private const int XP_PER_LEVEL = 1000;

        public XpSystem(AppDbContext context, IScoreService scoreService)
        {
            _context = context;
            _scoreService = scoreService;
        }

        public async Task<int> GetUserLevelAsync(int userId)
        {
            return await _scoreService.GetCurrentLevelAsync(userId);
        }

        public async Task<int> GetUserTotalXpAsync(int userId)
        {
            return await _scoreService.CalculateTotalXpAsync(userId);
        }

        public async Task<int> GetXpProgressToNextLevelAsync(int userId)
        {
            var totalXp = await GetUserTotalXpAsync(userId);
            var currentLevel = await GetUserLevelAsync(userId);
            var xpAtCurrentLevel = (currentLevel - 1) * XP_PER_LEVEL;
            return totalXp - xpAtCurrentLevel;
        }
    }
}
