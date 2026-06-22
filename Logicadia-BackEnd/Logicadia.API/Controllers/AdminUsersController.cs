using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Users;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logicadia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/admin/users/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var result = await _userService.GetAllForAdminAsync(pagination);
            return Ok(result);
        }
        // GET api/admin/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllForAdminAsync();
            return Ok(result);
        }
        // GET api/admin/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdForAdminAsync(id);
            if (user is null) return NotFound();
            return Ok(user);
        }

        // PUT api/admin/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var updated = await _userService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // DELETE api/admin/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // PUT api/admin/users/5/ban
        [HttpPut("{id}/ban")]
        public async Task<IActionResult> Ban(int id)
        {
            try
            {
                await _userService.BanUserAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // PUT api/admin/users/5/unban
        [HttpPut("{id}/unban")]
        public async Task<IActionResult> Unban(int id)
        {
            try
            {
                await _userService.UnbanUserAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}