using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Levels;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Logicadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminLevelsController : ControllerBase
    {
        private readonly ILevelService _levelService;

        public AdminLevelsController(ILevelService levelService)
        {
            _levelService = levelService;
        }

        // GET api/admin/levels
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var levels = await _levelService.GetAllForAdminAsync();
            return Ok(levels);
        }

        // GET api/admin/levels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var level = await _levelService.GetByIdForAdminAsync(id);
            if (level is null) return NotFound();
            return Ok(level);
        }
        // POST api/admin/levels
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLevelDto dto)
        {
            var created = await _levelService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/admin/levels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLevelDto dto)
        {
            try
            {
                var updated = await _levelService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // DELETE api/admin/levels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _levelService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // GET api/admin/levels/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] PaginationParams pagination)
        {
            var result = await _levelService.GetPagedForAdminAsync(pagination);
            return Ok(result);
        }
    }
}