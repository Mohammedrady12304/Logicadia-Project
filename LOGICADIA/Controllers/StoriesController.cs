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
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet("level/{levelId}")]
        public async Task<ActionResult<List<StoryDTO>>> GetStoriesByLevel(int levelId)
        {
            var userId = GetUserId();
            var stories = await _storyService.GetStoriesByLevelAsync(levelId, userId);
            return Ok(stories);
        }

        [HttpGet("{storyId}")]
        public async Task<ActionResult<StoryDetailDTO>> GetStoryById(int storyId)
        {
            var userId = GetUserId();
            var story = await _storyService.GetStoryByIdAsync(storyId, userId);

            if (story == null)
                return NotFound("Story not found");

            return Ok(story);
        }
    }
}
