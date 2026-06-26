using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Users;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserAdminDto>> GetAllForAdminAsync(PaginationParams pagination)
        {
            var query = _context.Users.Include(u => u.Role).AsNoTracking();

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var usersDto = new List<UserAdminDto>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<UserAdminDto>(user);
                dto.Role = user.Role?.Name ?? "User";
                dto.IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;
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
            var users = await _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .OrderBy(u => u.CreatedAt)
                .ToListAsync();

            var usersDto = new List<UserAdminDto>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<UserAdminDto>(user);
                dto.Role = user.Role?.Name ?? "User";
                dto.IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;
                usersDto.Add(dto);
            }
            return usersDto;
        }

        public async Task<UserAdminDto?> GetByIdForAdminAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null) return null;

            var dto = _mapper.Map<UserAdminDto>(user);
            dto.Role = user.Role?.Name ?? "User";
            dto.IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;
            return dto;
        }

        public async Task<UserAdminDto> UpdateAsync(int id, UpdateUserDto updateDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            user.UserName = updateDto.UserName;
            user.Email = updateDto.Email;

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<UserAdminDto>(user);
            dto.Role = user.Role?.Name ?? "User";
            dto.IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task BanUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            await _context.SaveChangesAsync();
        }

        public async Task UnbanUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) throw new NotFoundException(nameof(ApplicationUser), id);

            user.LockoutEnd = null;
            await _context.SaveChangesAsync();
        }
    }
}