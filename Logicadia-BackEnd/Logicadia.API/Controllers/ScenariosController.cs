using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Infrastructure.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicadia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScenariosController : ControllerBase
    {
        private readonly IScenarioService _scenarioService;

        public ScenariosController(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpGet("story/{storyId}")]
        public async Task<ActionResult<List<ScenarioDTO>>> GetScenariosByStory(int storyId)
        {
            var userId = GetUserId();
            var scenarios = await _scenarioService.GetScenariosByStoryAsync(storyId, userId);
            return Ok(scenarios);
        }

        [HttpGet("{scenarioId}")]
        public async Task<ActionResult<ScenarioDetailDTO>> GetScenarioById(int scenarioId)
        {
            var userId = GetUserId();
            var scenario = await _scenarioService.GetScenarioByIdAsync(scenarioId, userId);

            if (scenario == null)
                return NotFound("Scenario not found");

            return Ok(scenario);
        }
    }
}
