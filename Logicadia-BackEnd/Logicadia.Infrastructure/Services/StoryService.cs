using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Stories;
using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Logicadia.Infrastructure.Repositories;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.Infrastructure.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _repo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public StoryService(IStoryRepository repo, IMapper mapper, ApplicationDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        // ============ Admin ============

        public async Task<IEnumerable<StoryAdminDto>> GetAllForAdminAsync()
        {
            var stories = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<StoryAdminDto>>(stories);
        }

        public async Task<StoryAdminDto?> GetByIdForAdminAsync(int id)
        {
            var story = await _repo.GetByIdAsync(id);
            if (story is null) return null;
            return _mapper.Map<StoryAdminDto>(story);
        }

        public async Task<IEnumerable<StoryAdminDto>> GetByLevelIdForAdminAsync(int levelId)
        {
            var stories = await _repo.GetByLevelIdAsync(levelId);
            return _mapper.Map<IEnumerable<StoryAdminDto>>(stories);
        }

        public async Task<PagedResult<StoryAdminDto>> GetPagedForAdminAsync(PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return new PagedResult<StoryAdminDto>
            {
                Data = _mapper.Map<IEnumerable<StoryAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<PagedResult<StoryAdminDto>> GetPagedByLevelIdForAdminAsync(int levelId, PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedByLevelIdAsync(levelId, pagination.PageNumber, pagination.PageSize);
            return new PagedResult<StoryAdminDto>
            {
                Data = _mapper.Map<IEnumerable<StoryAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<StoryAdminDto> CreateAsync(CreateStoryDto dto)
        {
            var story = _mapper.Map<Story>(dto);
            await _repo.AddAsync(story);
            return _mapper.Map<StoryAdminDto>(story);
        }

        public async Task<StoryAdminDto> UpdateAsync(int id, UpdateStoryDto dto)
        {
            var story = await _repo.GetByIdAsync(id);
            if (story is null) throw new NotFoundException(nameof(Story), id);
            _mapper.Map(dto, story);
            await _repo.UpdateAsync(story);
            return _mapper.Map<StoryAdminDto>(story);
        }

        public async Task DeleteAsync(int id)
        {
            var story = await _repo.GetByIdAsync(id);
            if (story is null) throw new NotFoundException(nameof(Story), id);
            await _repo.DeleteAsync(story);
        }

        // ============ User ============

        public async Task<List<StoryDTO>> GetStoriesByLevelAsync(int levelId, int userId)
        {
            var stories = await _context.Stories
                .Where(s => s.LevelId == levelId)
                .OrderBy(s => s.OrderIndex)
                .Include(s => s.Scenarios)
                .ToListAsync();

            var userCompletedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == levelId)
                .Select(p => p.ScenarioId)
                .ToListAsync();

            return stories.Select(s => new StoryDTO
            {
                Id = s.Id,
                LevelId = s.LevelId,
                Title = s.Title,
                NarrativeText = s.NarrativeText,
                OrderIndex = s.OrderIndex,
                IsCompleted = s.Scenarios.All(sc => userCompletedScenarios.Contains(sc.Id))
            }).ToList();
        }

        public async Task<StoryDetailDTO?> GetStoryByIdAsync(int storyId, int userId)
        {
            var story = await _context.Stories
                .Include(s => s.Scenarios.OrderBy(sc => sc.OrderIndex))
                .FirstOrDefaultAsync(s => s.Id == storyId);

            if (story == null) return null;

            var userCompletedScenarios = await _context.UserProgress
                .Where(p => p.UserId == userId && p.ScenarioId == storyId)
                .Select(p => p.ScenarioId)
                .Distinct()
                .ToListAsync();

            return new StoryDetailDTO
            {
                Id = story.Id,
                LevelId = story.LevelId,
                Title = story.Title,
                NarrativeText = story.NarrativeText,
                OrderIndex = story.OrderIndex,
                IsCompleted = story.Scenarios.All(sc => userCompletedScenarios.Contains(sc.Id)),
                Scenarios = story.Scenarios.Select(sc => new ScenarioDTO
                {
                    Id = sc.Id,
                    StoryId = sc.StoryId,
                    Title = sc.Title,
                    Description = sc.Description,
                    OrderIndex = sc.OrderIndex,
                    IsCompleted = userCompletedScenarios.Contains(sc.Id)
                }).ToList()
            };
        }
    }
}