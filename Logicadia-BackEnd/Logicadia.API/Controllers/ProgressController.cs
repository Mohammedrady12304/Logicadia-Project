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
    public class ProgressController : ControllerBase
    {
        private readonly IUserProgressService _userProgressService;
        private readonly IXpSystem _xpSystem;

        public ProgressController(IUserProgressService userProgressService, IXpSystem xpSystem)
        {
            _userProgressService = userProgressService;
            _xpSystem = xpSystem;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet]
        public async Task<ActionResult<UserProgressDTO>> GetUserProgress()
        {
            var userId = GetUserId();
            var progress = await _userProgressService.GetUserProgressAsync(userId);
            return Ok(progress);
        }

        [HttpGet("level")]
        public async Task<ActionResult<object>> GetUserLevel()
        {
            var userId = GetUserId();
            var level = await _xpSystem.GetUserLevelAsync(userId);
            var totalXp = await _xpSystem.GetUserTotalXpAsync(userId);
            var xpToNextLevel = await _xpSystem.GetXpProgressToNextLevelAsync(userId);

            return Ok(new
            {
                CurrentLevel = level,
                TotalXp = totalXp,
                XpProgressToNextLevel = xpToNextLevel
            });
        }
    }
}
