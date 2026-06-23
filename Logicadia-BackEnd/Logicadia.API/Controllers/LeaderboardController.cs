using LOGICADIA.DTOs;
using Logicadia.Infrastructure.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicadia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet("top")]
        public async Task<ActionResult<List<LeaderboardEntryDTO>>> GetTopLeaderboard([FromQuery] int topCount = 100)
        {
            if (topCount < 1 || topCount > 1000)
                return BadRequest("Top count must be between 1 and 1000");

            var leaderboard = await _leaderboardService.GetTopLeaderboardAsync(topCount);
            return Ok(leaderboard);
        }

        [HttpGet("position")]
        public async Task<ActionResult<LeaderboardEntryDTO>> GetUserPosition()
        {
            var userId = GetUserId();
            var position = await _leaderboardService.GetUserLeaderboardPositionAsync(userId);

            if (position == null)
                return NotFound("User not found in leaderboard");

            return Ok(position);
        }

        [HttpGet("around")]
        public async Task<ActionResult<List<LeaderboardEntryDTO>>> GetLeaderboardAround([FromQuery] int range = 10)
        {
            if (range < 1 || range > 100)
                return BadRequest("Range must be between 1 and 100");

            var userId = GetUserId();
            var leaderboard = await _leaderboardService.GetLeaderboardAroundUserAsync(userId, range);
            return Ok(leaderboard);
        }
    }
}
