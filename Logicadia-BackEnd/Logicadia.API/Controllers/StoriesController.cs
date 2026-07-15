using Logicadia.Application.Features.DTOs.Stories;
using Logicadia.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicadia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public StoriesController(IStoryService storyService, IWebHostEnvironment env, IConfiguration configuration)
        {
            _storyService = storyService;
            _env = env;
            _configuration = configuration;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        // ✅ موجودة زي ما هي بالظبط
        [HttpGet("level/{levelId}")]
        public async Task<ActionResult<List<StoryDTO>>> GetStoriesByLevel(int levelId)
        {
            var userId = GetUserId();
            var stories = await _storyService.GetStoriesByLevelAsync(levelId, userId);
            return Ok(stories);
        }

        // ✅ موجودة زي ما هي بالظبط
        [HttpGet("{storyId}")]
        public async Task<ActionResult<StoryDetailDTO>> GetStoryById(int storyId)
        {
            var userId = GetUserId();
            var story = await _storyService.GetStoryByIdAsync(storyId, userId);
            if (story == null)
                return NotFound("Story not found");
            return Ok(story);
        }

        // 🆕 n8n هيبعت الفيديو هنا بعد التوليد، لستوري موجودة بالفعل
        [HttpPost("{storyId}/upload-video")]
        public async Task<IActionResult> UploadVideo(int storyId, IFormFile file)

        {
            //  if (apiKey != _configuration["N8nApiKey"])
            //   return Unauthorized();

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var fileName = $"{Guid.NewGuid()}.mp4";
            var folderPath = Path.Combine(_env.WebRootPath, "videos");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var videoUrl = $"{Request.Scheme}://{Request.Host}/videos/{fileName}";

            var updatedStory = await _storyService.AttachVideoAsync(storyId, videoUrl);
            if (updatedStory == null)
                return NotFound("Story not found");

            return Ok(new { updatedStory.Id, videoUrl });
        }
    }
}