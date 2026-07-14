using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Levels;
using Logicadia.Application.Features.DTOs.Stories;
using Logicadia.Application.Interfaces;
using Logicadia.Domain.Common;
using Logicadia.Domain.Entities;
using Logicadia.Infrastructure.Data;
using Logicadia.Infrastructure.Repositories;
using LOGICADIA.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Logicadia.Infrastructure.Services
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _repo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public LevelService(ILevelRepository repo, IMapper mapper, ApplicationDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        // ============ Admin ============

        public async Task<IEnumerable<LevelAdminDto>> GetAllForAdminAsync()
        {
            var levels = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<LevelAdminDto>>(levels);
        }

        public async Task<LevelAdminDto?> GetByIdForAdminAsync(int id)
        {
            var level = await _repo.GetByIdAsync(id);
            if (level is null) return null;
            return _mapper.Map<LevelAdminDto>(level);
        }

        public async Task<PagedResult<LevelAdminDto>> GetPagedForAdminAsync(PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return new PagedResult<LevelAdminDto>
            {
                Data = _mapper.Map<IEnumerable<LevelAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<LevelAdminDto> CreateAsync(CreateLevelDto dto)
        {
            var level = _mapper.Map<Level>(dto);
            await _repo.AddAsync(level);
            return _mapper.Map<LevelAdminDto>(level);
        }

        public async Task<LevelAdminDto> UpdateAsync(int id, UpdateLevelDto dto)
        {
            var level = await _repo.GetByIdAsync(id);
            if (level is null) throw new NotFoundException(nameof(Level), id);
            _mapper.Map(dto, level);
            await _repo.UpdateAsync(level);
            return _mapper.Map<LevelAdminDto>(level);
        }

        public async Task DeleteAsync(int id)
        {
            var level = await _repo.GetByIdAsync(id);
            if (level is null) throw new NotFoundException(nameof(Level), id);
            await _repo.DeleteAsync(level);
        }

        // ============ User ============

        public async Task<List<LevelDTO>> GetAllLevelsAsync(int userId)
        {
            var levels = await _context.Levels
                .OrderBy(l => l.OrderIndex)
                .ToListAsync();

            // الليفلات اللي عند اليوزر progress فيها (يعني حل حاجة جواها)
            var levelIdsWithProgress = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Select(p => p.Scenario.Story.LevelId)
                .Distinct()
                .ToListAsync();

            var result = new List<LevelDTO>(levels.Count);

            for (int index = 0; index < levels.Count; index++)
            {
                var level = levels[index];

                bool isUnlocked;
                if (index == 0)
                {
                    // أول ليفل دايمًا مفتوح
                    isUnlocked = true;
                }
                else
                {
                    // الليفل ده يتفتح لو عند اليوزر progress في الليفل *السابق* (مش نفس الليفل)
                    var previousLevel = levels[index - 1];
                    isUnlocked = levelIdsWithProgress.Contains(previousLevel.Id);
                }

                var latestProgress = await _context.UserProgress
                    .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == level.Id)
                    .OrderByDescending(p => p.CompletedAt)
                    .FirstOrDefaultAsync();

                result.Add(new LevelDTO
                {
                    Id = level.Id,
                    Title = level.Title,
                    Description = level.Description,
                    OrderIndex = level.OrderIndex,
                    XpReward = level.XpReward,
                    IsUnlocked = isUnlocked,
                    UnlockedAt = latestProgress?.CompletedAt
                });
            }

            return result;
        }

        public async Task<LevelDetailDTO?> GetLevelByIdAsync(int levelId, int userId)
        {
            var level = await _context.Levels
                .Include(l => l.Stories.OrderBy(s => s.OrderIndex))
                .ThenInclude(s => s.Scenarios.OrderBy(sc => sc.OrderIndex))
                .FirstOrDefaultAsync(l => l.Id == levelId);

            if (level == null)
                return null;

            var firstLevelId = await _context.Levels
                .OrderBy(l => l.OrderIndex)
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            bool isUnlocked;

            if (level.Id == firstLevelId)
            {
                isUnlocked = true;
            }
            else
            {
                var previousLevelId = await _context.Levels
                    .Where(l => l.OrderIndex < level.OrderIndex)
                    .OrderByDescending(l => l.OrderIndex)
                    .Select(l => l.Id)
                    .FirstOrDefaultAsync();

                isUnlocked = await _context.UserProgress
                    .AnyAsync(p =>
                        p.UserId == userId &&
                        p.Scenario.Story.LevelId == previousLevelId
                    );
            }

            if (!isUnlocked)
                return null;

            var completedScenarios = await _context.UserProgress
                .Where(p =>
                    p.UserId == userId &&
                    p.Scenario.Story.LevelId == levelId
                )
                .Select(p => p.ScenarioId)
                .ToListAsync();

            return new LevelDetailDTO
            {
                Id = level.Id,
                Title = level.Title,
                Description = level.Description,
                OrderIndex = level.OrderIndex,
                XpReward = level.XpReward,
                IsUnlocked = true,

                Stories = level.Stories.Select(s => new StoryDTO
                {
                    Id = s.Id,
                    LevelId = s.LevelId,
                    Title = s.Title,
                    NarrativeText = s.NarrativeText,
                    OrderIndex = s.OrderIndex,
                    IsCompleted = s.Scenarios.All(sc => completedScenarios.Contains(sc.Id))
                }).ToList()
            };
        }

        public async Task<bool> IsLevelUnlockedAsync(int levelId, int userId)
        {
            var level = await _context.Levels.FindAsync(levelId);
            if (level == null) return false;

            // ملحوظة: عندك OrderIndex بيبدأ من 1 مش من 0 (زي ما ظاهر في الـ JSON)
            // فالمقارنة دي كانت غلط: level.OrderIndex == 0
            var firstLevel = await _context.Levels
                .OrderBy(l => l.OrderIndex)
                .FirstOrDefaultAsync();

            if (firstLevel != null && level.Id == firstLevel.Id) return true;

            var previousLevelId = await _context.Levels
                .Where(l => l.OrderIndex < level.OrderIndex)
                .OrderByDescending(l => l.OrderIndex)
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            if (previousLevelId == 0) return false;

            return await _context.UserProgress
                .Where(p => p.UserId == userId && p.Scenario.Story.LevelId == previousLevelId)
                .AnyAsync();
        }
    }
}