using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Users;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Logicadia.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserAdminDto>> GetAllForAdminAsync(PaginationParams pagination)
        {
            var totalCount = _userManager.Users.Count();
            var users = await _userManager.Users
                .OrderBy(u => u.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var usersDto = new List<UserAdminDto>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<UserAdminDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                dto.Role = roles.FirstOrDefault() ?? "User";
                usersDto.Add(dto);
            }

            return new PagedResult<UserAdminDto>
            {
                Data = usersDto,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
        public async Task<IEnumerable<UserAdminDto>> GetAllForAdminAsync()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.CreatedAt)
                .ToListAsync();

            var usersDto = new List<UserAdminDto>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<UserAdminDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                dto.Role = roles.FirstOrDefault() ?? "User";
                usersDto.Add(dto);
            }
            return usersDto;
        }
        public async Task<UserAdminDto?> GetByIdForAdminAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return null;

            var dto = _mapper.Map<UserAdminDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault() ?? "User";
            return dto;
        }

        public async Task<UserAdminDto> UpdateAsync(int id, UpdateUserDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            user.UserName = updateDto.UserName;
            user.Email = updateDto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var dto = _mapper.Map<UserAdminDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault() ?? "User";
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task BanUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        }

        public async Task UnbanUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            await _userManager.SetLockoutEndDateAsync(user, null);
        }
    }
}
