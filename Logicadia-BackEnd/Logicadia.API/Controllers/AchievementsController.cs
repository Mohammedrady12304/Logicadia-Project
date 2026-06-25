using LOGICADIA.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Logicadia.Infrastructure.Services;

namespace Logicadia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementEngine _achievementEngine;

        public AchievementsController(IAchievementEngine achievementEngine)
        {
            _achievementEngine = achievementEngine;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet]
        public async Task<ActionResult<List<UserAchievementDTO>>> GetUserAchievements()
        {
            var userId = GetUserId();
            var achievements = await _achievementEngine.GetUserAchievementsAsync(userId);
            return Ok(achievements);
        }
    }
}
