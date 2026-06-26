using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IScenarioService
    {
        // Admin
        Task<IEnumerable<ScenarioAdminDto>> GetAllForAdminAsync();
        Task<PagedResult<ScenarioAdminDto>> GetPagedForAdminAsync(PaginationParams pagination);
        Task<PagedResult<ScenarioAdminDto>> GetPagedByStoryIdForAdminAsync(int storyId, PaginationParams pagination);
        Task<IEnumerable<ScenarioAdminDto>> GetByStoryIdForAdminAsync(int storyId);
        Task<ScenarioAdminDto?> GetByIdForAdminAsync(int id);
        Task<ScenarioAdminDto> CreateAsync(CreateScenarioDto dto);
        Task<ScenarioAdminDto> UpdateAsync(int id, UpdateScenarioDto dto);
        Task DeleteAsync(int id);

        // User
        Task<ScenarioDetailDTO?> GetScenarioByIdAsync(int scenarioId, int userId);
        Task<List<ScenarioDTO>> GetScenariosByStoryAsync(int storyId, int userId);
    }
}
