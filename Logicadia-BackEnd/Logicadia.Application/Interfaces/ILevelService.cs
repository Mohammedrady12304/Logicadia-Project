using Logicadia.Application.Features.DTOs.Levels;
using Logicadia.Domain.Common;
using LOGICADIA.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface ILevelService
    {
        // Admin
        Task<IEnumerable<LevelAdminDto>> GetAllForAdminAsync();
        Task<PagedResult<LevelAdminDto>> GetPagedForAdminAsync(PaginationParams pagination);
        Task<LevelAdminDto?> GetByIdForAdminAsync(int id);
        Task<LevelAdminDto> CreateAsync(CreateLevelDto dto);
        Task<LevelAdminDto> UpdateAsync(int id, UpdateLevelDto dto);
        Task DeleteAsync(int id);
        // User
        Task<List<LevelDTO>> GetAllLevelsAsync(int userId);
        Task<LevelDetailDTO?> GetLevelByIdAsync(int levelId, int userId);
        Task<bool> IsLevelUnlockedAsync(int levelId, int userId);
    }
}
