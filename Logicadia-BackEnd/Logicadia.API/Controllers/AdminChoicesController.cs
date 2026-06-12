using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Choice;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logicadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminChoicesController : ControllerBase
    {
        private readonly IChoiceService _choiceService;

        public AdminChoicesController(IChoiceService choiceService)
        {
            _choiceService = choiceService;
        }

        // GET api/admin/choices
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var choices = await _choiceService.GetAllForAdminAsync();
            return Ok(choices);
        }
        // GET api/admin/choices/by-scenario/5
        [HttpGet("by-scenario/{scenarioId}")]
        public async Task<IActionResult> GetByScenarioId(int scenarioId)
        {
            var choices = await _choiceService.GetByScenarioIdForAdminAsync(scenarioId);
            return Ok(choices);
        }

        // GET api/admin/choices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var choice = await _choiceService.GetByIdForAdminAsync(id);
            if (choice is null) return NotFound();
            return Ok(choice);
        }

        // POST api/admin/choices
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChoiceDto dto)
        {
            var created = await _choiceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        // PUT api/admin/choices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChoiceDto dto)
        {
            try
            {
                var updated = await _choiceService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // DELETE api/admin/choices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _choiceService.DeleteAsync(id);
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
            var result = await _choiceService.GetPagedForAdminAsync(pagination);
            return Ok(result);
        }

        [HttpGet("paged/by-scenario/{scenarioId}")]
        public async Task<IActionResult> GetPagedByScenarioId(int scenarioId, [FromQuery] PaginationParams pagination)
        {
            var result = await _choiceService.GetPagedByScenarioIdForAdminAsync(scenarioId, pagination);
            return Ok(result);
        }
    }
}
