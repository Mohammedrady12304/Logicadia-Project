using Logicadia.Application.Features.DTOs.Stories;
using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IStoryService
    {
        // Admin
        Task<IEnumerable<StoryAdminDto>> GetAllForAdminAsync();
        Task<PagedResult<StoryAdminDto>> GetPagedForAdminAsync(PaginationParams pagination);
        Task<PagedResult<StoryAdminDto>> GetPagedByLevelIdForAdminAsync(int levelId, PaginationParams pagination); Task<IEnumerable<StoryAdminDto>> GetByLevelIdForAdminAsync(int levelId);
        Task<StoryAdminDto?> GetByIdForAdminAsync(int id);
        Task<StoryAdminDto> CreateAsync(CreateStoryDto dto);
        Task<StoryAdminDto> UpdateAsync(int id, UpdateStoryDto dto);
        Task DeleteAsync(int id);

        // User
    }
}
