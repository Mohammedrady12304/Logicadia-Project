using LOGICADIA.DTOs;
using LOGICADIA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LOGICADIA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IWeeklyStatisticsService _weeklyStatisticsService;

        public StatisticsController(IWeeklyStatisticsService weeklyStatisticsService)
        {
            _weeklyStatisticsService = weeklyStatisticsService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet("current-week")]
        public async Task<ActionResult<WeeklyStatisticsDTO>> GetCurrentWeekStatistics()
        {
            var userId = GetUserId();
            var stats = await _weeklyStatisticsService.GetCurrentWeekStatisticsAsync(userId);
            return Ok(stats);
        }

        [HttpGet("week")]
        public async Task<ActionResult<WeeklyStatisticsDTO>> GetWeekStatistics([FromQuery] int weekNumber, [FromQuery] int year)
        {
            if (weekNumber < 1 || weekNumber > 53)
                return BadRequest("Week number must be between 1 and 53");

            if (year < 2020 || year > DateTime.UtcNow.Year + 1)
                return BadRequest("Invalid year");

            var userId = GetUserId();
            var stats = await _weeklyStatisticsService.GetWeekStatisticsAsync(userId, weekNumber, year);
            return Ok(stats);
        }

        [HttpGet("all-weeks")]
        public async Task<ActionResult<List<WeeklyStatisticsDTO>>> GetAllWeeksStatistics()
        {
            var userId = GetUserId();
            var stats = await _weeklyStatisticsService.GetAllWeeksStatisticsAsync(userId);
            return Ok(stats);
        }
    }
}
