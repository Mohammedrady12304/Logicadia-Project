using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Achievement;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logicadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAchievementsController : ControllerBase
    {
        private readonly IAchievementService _achievementService;

        public AdminAchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        // GET api/admin/achievements
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var achievements = await _achievementService.GetAllForAdminAsync();
            return Ok(achievements);
        }

        // GET api/admin/achievements/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var achievement = await _achievementService.GetByIdForAdminAsync(id);
            if (achievement is null) return NotFound();
            return Ok(achievement);
        }
        // POST api/admin/achievements
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAchievementDto dto)
        {
            var created = await _achievementService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/admin/achievements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAchievementDto dto)
        {
            try
            {
                var updated = await _achievementService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // DELETE api/admin/achievements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _achievementService.DeleteAsync(id);
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
            var result = await _achievementService.GetPagedForAdminAsync(pagination);
            return Ok(result);
        }
    }
}