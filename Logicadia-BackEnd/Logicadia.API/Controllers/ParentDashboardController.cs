using Logicadia.Application.Features.DTOs.ParentDashboard;
using Logicadia.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicadia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ParentDashboardController : ControllerBase
    {
        private readonly IParentDashboardService _dashboardService;

        public ParentDashboardController(IParentDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        
        private int GetCurrentParentId()
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimValue))
                throw new UnauthorizedAccessException("Cannot extract parent data from the Token.");

            return int.Parse(claimValue); 
        }

        [HttpGet("children")]
        public async Task<IActionResult> GetChildren()
        {
            var parentId = GetCurrentParentId(); 
            var children = await _dashboardService.GetParentChildrenAsync(parentId);
            return Ok(children);
        }

        [HttpGet("child/{childId}/progress")]
        public async Task<IActionResult> GetChildProgress([FromRoute] int childId)
        {
            var parentId = GetCurrentParentId();
            var progress = await _dashboardService.GetChildProgressAsync(parentId, childId);

            if (progress == null)
                return NotFound(new { message = "Child not found or you don't have permission to access it." });

            return Ok(progress);
        }

        [HttpPost("child/{childId}/assign-path")]
        public async Task<IActionResult> AssignPath([FromRoute] int childId, [FromBody] AssignPathDto dto)
        {
            var parentId = GetCurrentParentId();
            var result = await _dashboardService.AssignPathToChildAsync(parentId, childId, dto);

            if (!result)
                return BadRequest(new { message = "Failed to update the learning path, please check the data and permissions." });

            return Ok(new { message = "The learning path has been updated successfully." });
        }
    }
}
