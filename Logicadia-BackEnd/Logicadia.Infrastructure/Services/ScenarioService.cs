using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Infrastructure.Services
{
    public class ScenarioService : IScenarioService
    {
        private readonly IScenarioRepository _repo;
        private readonly IMapper _mapper;

        public ScenarioService(IScenarioRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ============ Admin ============

        public async Task<IEnumerable<ScenarioAdminDto>> GetAllForAdminAsync()
        {
            var scenarios = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ScenarioAdminDto>>(scenarios);
        }
        public async Task<PagedResult<ScenarioAdminDto>> GetPagedForAdminAsync(PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return new PagedResult<ScenarioAdminDto>
            {
                Data = _mapper.Map<IEnumerable<ScenarioAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<PagedResult<ScenarioAdminDto>> GetPagedByStoryIdForAdminAsync(int storyId, PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedByStoryIdAsync(storyId, pagination.PageNumber, pagination.PageSize);
            return new PagedResult<ScenarioAdminDto>
            {
                Data = _mapper.Map<IEnumerable<ScenarioAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
        public async Task<IEnumerable<ScenarioAdminDto>> GetByStoryIdForAdminAsync(int storyId)
        {
            var scenarios = await _repo.GetByStoryIdAsync(storyId);
            return _mapper.Map<IEnumerable<ScenarioAdminDto>>(scenarios);
        }

        public async Task<ScenarioAdminDto?> GetByIdForAdminAsync(int id)
        {
            var scenario = await _repo.GetByIdAsync(id);
            if (scenario is null) return null;
            return _mapper.Map<ScenarioAdminDto>(scenario);
        }

        public async Task<ScenarioAdminDto> CreateAsync(CreateScenarioDto dto)
        {
            var scenario = _mapper.Map<Scenario>(dto);
            await _repo.AddAsync(scenario);
            return _mapper.Map<ScenarioAdminDto>(scenario);
        }
        public async Task<ScenarioAdminDto> UpdateAsync(int id, UpdateScenarioDto dto)
        {
            var scenario = await _repo.GetByIdAsync(id);
            if (scenario is null) throw new NotFoundException(nameof(Scenario), id);
            _mapper.Map(dto, scenario);
            await _repo.UpdateAsync(scenario);
            return _mapper.Map<ScenarioAdminDto>(scenario);
        }

        public async Task DeleteAsync(int id)
        {
            var scenario = await _repo.GetByIdAsync(id);
            if (scenario is null) throw new NotFoundException(nameof(Scenario), id);
            await _repo.DeleteAsync(scenario);
        }

        // ============ User ============
    }
}
