using Logicadia.Application.Features.DTOs.Achievement;
using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IAchievementService
    {
        // Admin
        Task<IEnumerable<AchievementAdminDto>> GetAllForAdminAsync();
        Task<AchievementAdminDto?> GetByIdForAdminAsync(int id);
        Task<PagedResult<AchievementAdminDto>> GetPagedForAdminAsync(PaginationParams pagination);

        Task<AchievementAdminDto> CreateAsync(CreateAchievementDto dto);
        Task<AchievementAdminDto> UpdateAsync(int id, UpdateAchievementDto dto);
        Task DeleteAsync(int id);

        // User
    }
}

