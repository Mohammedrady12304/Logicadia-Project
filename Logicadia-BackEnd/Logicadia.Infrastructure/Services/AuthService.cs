using Logicadia.Application.Features.DTOs.Auth;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(ApplicationDbContext context, IJwtProvider jwtProvider)
        {
            _context = context;
            _jwtProvider = jwtProvider;
        }

        // 1. Login
        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResultDto { IsSuccess = false, Message = "Invalid email or password." };
            
        }
        string token = _jwtProvider.GenerateToken(user, user.Role.Name);

            return new AuthResultDto
            {
            IsSuccess = true,
                Message = "Logged in successfully.",
                Token = token,
                Role = user.Role.Name
        };
        }

        // 2. Register Parent
        public async Task<AuthResultDto> RegisterParentAsync(RegisterParentDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Username);
            if (exists)
            {
                return new AuthResultDto { IsSuccess = false, Message = "This email is already registered." };
            }

            var parentRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Parent");
            if (parentRole == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Parent role is not defined in the system." };
            }
            var user = new ApplicationUser
            {
                Name = dto.FullName,
                Email = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = parentRole.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var parent = new Parent
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();
            return new AuthResultDto { IsSuccess = true, Message = "Parent account registered successfully." };
        }

        // 3. Register Child
        public async Task<AuthResultDto> RegisterChildAsync(RegisterChildDto dto, string parentUserId)
        {
            if (string.IsNullOrEmpty(parentUserId))
            {
                return new AuthResultDto { IsSuccess = false, Message = "Unauthorized to add a child." };
            }

            var parent = await _context.Parents.FirstOrDefaultAsync(p => p.UserId == int.Parse(parentUserId));
            if (parent == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Parent account not found." };
            }

            var childExists = await _context.Users.AnyAsync(u => u.Email == dto.Username);
            if (childExists)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Child username is already taken." };
            }
            var childRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Child");
            if (childRole == null)
            {
                return new AuthResultDto { IsSuccess = false, Message = "Child role is not defined in the system." };
            }

            var childUser = new ApplicationUser
            {
                Name = dto.Name,
                Email = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = childRole.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(childUser);
            await _context.SaveChangesAsync();

            var child = new Child
            {
                Name = dto.Name,
                Age = dto.Age,
                UserId = childUser.Id,
                ParentId = parent.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Children.Add(child);
            await _context.SaveChangesAsync();

            return new AuthResultDto { IsSuccess = true, Message = "Child added and linked successfully." };
        }
    }
}