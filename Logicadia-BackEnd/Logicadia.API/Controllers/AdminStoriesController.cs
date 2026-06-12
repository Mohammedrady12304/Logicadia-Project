using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Stories;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logicadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminStoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public AdminStoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        // GET api/admin/stories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stories = await _storyService.GetAllForAdminAsync();
            return Ok(stories);
        }

        // GET api/admin/stories/by-level/5
        [HttpGet("by-level/{levelId}")]
        public async Task<IActionResult> GetByLevelId(int levelId)
        {
            var stories = await _storyService.GetByLevelIdForAdminAsync(levelId);
            return Ok(stories);
        }
        // GET api/admin/stories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var story = await _storyService.GetByIdForAdminAsync(id);
            if (story is null) return NotFound();
            return Ok(story);
        }

        // POST api/admin/stories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStoryDto dto)
        {
            var created = await _storyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        // PUT api/admin/stories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStoryDto dto)
        {
            try
            {
                var updated = await _storyService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // DELETE api/admin/stories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _storyService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams pagination)
        {
            var result = await _storyService.GetPagedForAdminAsync(pagination);
            return Ok(result);
        }

        [HttpGet("paged/by-level/{levelId}")]
        public async Task<IActionResult> GetPagedByLevelId(int levelId, [FromQuery] PaginationParams pagination)
        {
            var result = await _storyService.GetPagedByLevelIdForAdminAsync(levelId, pagination);
            return Ok(result);
        }
    }
}

