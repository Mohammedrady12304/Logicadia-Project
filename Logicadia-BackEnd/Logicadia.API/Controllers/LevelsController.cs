using LOGICADIA.DTOs;
using Logicadia.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Logicadia.Application.Interfaces;

namespace Logicadia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class LevelsController : ControllerBase
    {
        private readonly ILevelService _levelService;

        public LevelsController(ILevelService levelService)
        {
            _levelService = levelService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet]
        public async Task<ActionResult<List<LevelDTO>>> GetAllLevels()
        {
            var userId = GetUserId();
            var levels = await _levelService.GetAllLevelsAsync(userId);
            return Ok(levels);
        }

        [HttpGet("{levelId}")]
        public async Task<ActionResult<LevelDetailDTO>> GetLevelById(int levelId)
        {
            var userId = GetUserId();
            var level = await _levelService.GetLevelByIdAsync(levelId, userId);

            if (level == null)
                return NotFound("Level not found or not unlocked");

            return Ok(level);
        }
    }
}
