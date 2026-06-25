using Logicadia.Application.Features.DTOs.Choice;
using Logicadia.Application.Interfaces;
using Logicadia.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicadia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChoicesController : ControllerBase
    {
        private readonly IChoiceService _choiceService;

        public ChoicesController(IChoiceService choiceService)
        {
            _choiceService = choiceService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        [HttpPost("submit")]
        public async Task<ActionResult<ChoiceSubmitResponse>> SubmitChoice(ChoiceSubmitRequest request)
        {
            var userId = GetUserId();

            try
            {
                var response = await _choiceService.SubmitChoiceAsync(userId, request.ScenarioId, request.ChoiceId);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("scenario/{scenarioId}")]
        public async Task<ActionResult<List<ChoiceDTO>>> GetChoicesByScenario(int scenarioId)
        {
            var choices = await _choiceService.GetChoicesByScenarioAsync(scenarioId);
            return Ok(choices);
        }
    }
}
