using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logicadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminScenariosController : ControllerBase
    {
        private readonly IScenarioService _scenarioService;

        public AdminScenariosController(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        // GET api/admin/scenarios
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var scenarios = await _scenarioService.GetAllForAdminAsync();
            return Ok(scenarios);
        }
        // GET api/admin/scenarios/by-story/5
        [HttpGet("by-story/{storyId}")]
        public async Task<IActionResult> GetByStoryId(int storyId)
        {
            var scenarios = await _scenarioService.GetByStoryIdForAdminAsync(storyId);
            return Ok(scenarios);
        }

        // GET api/admin/scenarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var scenario = await _scenarioService.GetByIdForAdminAsync(id);
            if (scenario is null) return NotFound();
            return Ok(scenario);
        }

        // POST api/admin/scenarios
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScenarioDto dto)
        {
            var created = await _scenarioService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        // PUT api/admin/scenarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateScenarioDto dto)
        {
            try
            {
                var updated = await _scenarioService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // DELETE api/admin/scenarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _scenarioService.DeleteAsync(id);
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
            var result = await _scenarioService.GetPagedForAdminAsync(pagination);
            return Ok(result);
        }

        [HttpGet("paged/by-story/{storyId}")]
        public async Task<IActionResult> GetPagedByStoryId(int storyId, [FromQuery] PaginationParams pagination)
        {
            var result = await _scenarioService.GetPagedByStoryIdForAdminAsync(storyId, pagination);
            return Ok(result);
        }

    }
}