using Logicadia.Application.Features.DTOs.Choice;
using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IChoiceService
    {
        // Admin
        Task<IEnumerable<ChoiceAdminDto>> GetAllForAdminAsync();
        Task<PagedResult<ChoiceAdminDto>> GetPagedForAdminAsync(PaginationParams pagination);
        Task<PagedResult<ChoiceAdminDto>> GetPagedByScenarioIdForAdminAsync(int scenarioId, PaginationParams pagination);
        Task<IEnumerable<ChoiceAdminDto>> GetByScenarioIdForAdminAsync(int scenarioId);
        Task<ChoiceAdminDto?> GetByIdForAdminAsync(int id);
        Task<ChoiceAdminDto> CreateAsync(CreateChoiceDto dto);
        Task<ChoiceAdminDto> UpdateAsync(int id, UpdateChoiceDto dto);
        Task DeleteAsync(int id);

        // User
        Task<ChoiceSubmitResponse> SubmitChoiceAsync(int userId, int scenarioId, int choiceId);
        Task<List<ChoiceDTO>> GetChoicesByScenarioAsync(int scenarioId);
    }
}
