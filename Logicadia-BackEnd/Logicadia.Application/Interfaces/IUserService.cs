using Logicadia.Application.Features.DTOs.Users;
using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IUserService
    {
        // Admin
        Task<PagedResult<UserAdminDto>> GetAllForAdminAsync(PaginationParams pagination);
        Task<IEnumerable<UserAdminDto>> GetAllForAdminAsync();
        Task<UserAdminDto?> GetByIdForAdminAsync(int id);
        Task<UserAdminDto> UpdateAsync(int id, UpdateUserDto dto);
        Task DeleteAsync(int id);
        Task BanUserAsync(int id);
        Task UnbanUserAsync(int id);
    }
}
