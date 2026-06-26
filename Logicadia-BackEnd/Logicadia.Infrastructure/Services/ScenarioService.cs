using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Application.Features.DTOs.Choice;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Logicadia.Infrastructure.Repositories;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.Infrastructure.Services
{
    public class ScenarioService : IScenarioService
    {
        private readonly IScenarioRepository _repo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ScenarioService(IScenarioRepository repo, IMapper mapper, ApplicationDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        // ============ Admin ============

        public async Task<IEnumerable<ScenarioAdminDto>> GetAllForAdminAsync()
        {
            var scenarios = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ScenarioAdminDto>>(scenarios);
        }

        public async Task<ScenarioAdminDto?> GetByIdForAdminAsync(int id)
        {
            var scenario = await _repo.GetByIdAsync(id);
            if (scenario is null) return null;
            return _mapper.Map<ScenarioAdminDto>(scenario);
        }

        public async Task<IEnumerable<ScenarioAdminDto>> GetByStoryIdForAdminAsync(int storyId)
        {
            var scenarios = await _repo.GetByStoryIdAsync(storyId);
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

        public async Task<ScenarioDetailDTO?> GetScenarioByIdAsync(int scenarioId, int userId)
        {
            var scenario = await _context.Scenarios
                .Include(s => s.Choices.OrderBy(c => c.Id))
                .FirstOrDefaultAsync(s => s.Id == scenarioId);

            if (scenario == null) return null;

            var userAttempt = await _context.UserProgress
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ScenarioId == scenarioId);

            return new ScenarioDetailDTO
            {
                Id = scenario.Id,
                StoryId = scenario.StoryId,
                Title = scenario.Title,
                Description = scenario.Description,
                OrderIndex = scenario.OrderIndex,
                IsCompleted = userAttempt != null,
                Choices = scenario.Choices.Select(c => new ChoiceDTO
                {
                    Id = c.Id,
                    ChoiceText = c.ChoiceText,
                    IsCorrect = c.IsCorrect,
                    Feedback = c.Feedback,
                    XpValue = c.XpValue
                }).ToList()
            };
        }

        public async Task<List<ScenarioDTO>> GetScenariosByStoryAsync(int storyId, int userId)
        {
            var scenarios = await _context.Scenarios
                .Where(s => s.StoryId == storyId)
                .OrderBy(s => s.OrderIndex)
                .ToListAsync();

            var userCompletedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.StoryId == storyId)
                .Select(p => p.ScenarioId)
                .ToListAsync();

            return scenarios.Select(s => new ScenarioDTO
            {
                Id = s.Id,
                StoryId = s.StoryId,
                Title = s.Title,
                Description = s.Description,
                OrderIndex = s.OrderIndex,
                IsCompleted = userCompletedScenarios.Contains(s.Id)
            }).ToList();
        }
    }
}