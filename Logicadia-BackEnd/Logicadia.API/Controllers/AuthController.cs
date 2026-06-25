using Logicadia.Application.Features.DTOs.Auth;
using Logicadia.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logicadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register-parent")]

        public async Task<IActionResult> RegisterParent([FromBody] RegisterParentDto dto)
        {
            var result = await _authService.RegisterParentAsync(dto);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpPost("register-child")]
        [Authorize(Roles = "Parent,Admin")]
        public async Task<IActionResult> RegisterChild([FromBody] RegisterChildDto dto)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _authService.RegisterChildAsync(dto, int.Parse(userIdClaim));
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok(new { Token = result.Token, Role = result.Role });
        }
    }
}
