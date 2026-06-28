using AutoMapper;
using Logicadia.Application.Exceptions;
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
    public class ChoiceService : IChoiceService
    {
        private readonly IChoiceRepository _repo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IAchievementEngine _achievementEngine;
        private readonly ILevelUnlockService _levelUnlockService;

        public ChoiceService(
            IChoiceRepository repo,
            IMapper mapper,
            ApplicationDbContext context,
            IAchievementEngine achievementEngine,
            ILevelUnlockService levelUnlockService)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
            _achievementEngine = achievementEngine;
            _levelUnlockService = levelUnlockService;
        }

        // ============ Admin ============

        public async Task<IEnumerable<ChoiceAdminDto>> GetAllForAdminAsync()
        {
            var choices = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ChoiceAdminDto>>(choices);
        }

        public async Task<ChoiceAdminDto?> GetByIdForAdminAsync(int id)
        {
            var choice = await _repo.GetByIdAsync(id);
            if (choice is null) return null;
            return _mapper.Map<ChoiceAdminDto>(choice);
        }

        public async Task<IEnumerable<ChoiceAdminDto>> GetByScenarioIdForAdminAsync(int scenarioId)
        {
            var choices = await _repo.GetByScenarioIdAsync(scenarioId);
            return _mapper.Map<IEnumerable<ChoiceAdminDto>>(choices);
        }

        public async Task<PagedResult<ChoiceAdminDto>> GetPagedForAdminAsync(PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return new PagedResult<ChoiceAdminDto>
            {
                Data = _mapper.Map<IEnumerable<ChoiceAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<PagedResult<ChoiceAdminDto>> GetPagedByScenarioIdForAdminAsync(int scenarioId, PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedByScenarioIdAsync(scenarioId, pagination.PageNumber, pagination.PageSize);
            return new PagedResult<ChoiceAdminDto>
            {
                Data = _mapper.Map<IEnumerable<ChoiceAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<ChoiceAdminDto> CreateAsync(CreateChoiceDto dto)
        {
            var choice = _mapper.Map<Choice>(dto);
            await _repo.AddAsync(choice);
            return _mapper.Map<ChoiceAdminDto>(choice);
        }

        public async Task<ChoiceAdminDto> UpdateAsync(int id, UpdateChoiceDto dto)
        {
            var choice = await _repo.GetByIdAsync(id);
            if (choice is null) throw new NotFoundException(nameof(Choice), id);
            _mapper.Map(dto, choice);
            await _repo.UpdateAsync(choice);
            return _mapper.Map<ChoiceAdminDto>(choice);
        }

        public async Task DeleteAsync(int id)
        {
            var choice = await _repo.GetByIdAsync(id);
            if (choice is null) throw new NotFoundException(nameof(Choice), id);
            await _repo.DeleteAsync(choice);
        }

        // ============ User ============

        public async Task<ChoiceSubmitResponse> SubmitChoiceAsync(int userId, int scenarioId, int choiceId)
        {
            var choice = await _context.Choices.FindAsync(choiceId);
            if (choice == null || choice.ScenarioId != scenarioId)
                throw new InvalidOperationException("Invalid choice selection");

            var scenario = await _context.Scenarios
                .Include(s => s.Story)
                .FirstOrDefaultAsync(s => s.Id == scenarioId);
            if (scenario == null)
                throw new InvalidOperationException("Scenario not found");

            var child = await _context.Children
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (child == null)
                throw new InvalidOperationException("Child not found");

            var xpEarned = choice.IsCorrect ? choice.XpValue : choice.XpValue / 2;

            var existingProgress = await _context.UserProgress
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ScenarioId == scenarioId);

            if (existingProgress != null)
            {
                existingProgress.ChosenChoiceId = choiceId;
                existingProgress.IsCorrect = choice.IsCorrect;
                existingProgress.XpEarned = xpEarned;
                existingProgress.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                var userProgress = new UserProgress
                {
                    UserId = userId,
                    ChildId = child.Id,
                    ScenarioId = scenarioId,
                    ChosenChoiceId = choiceId,
                    IsCorrect = choice.IsCorrect,
                    XpEarned = xpEarned,
                    LevelId = scenario.Story.LevelId,
                    StoryId = scenario.StoryId,
                    CompletedAt = DateTime.UtcNow
                };
                _context.UserProgress.Add(userProgress);
            }

            var levelUnlocked = await _levelUnlockService
                .CheckAndUnlockNextLevelAsync(userId, scenario.Story.LevelId);
            var achievementsUnlocked = await _achievementEngine
                .CheckAndUnlockAchievementsAsync(userId);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    ex.InnerException?.InnerException?.Message
                    ?? ex.InnerException?.Message
                );
            }

            return new ChoiceSubmitResponse
            {
                IsCorrect = choice.IsCorrect,
                XpEarned = xpEarned,
                Feedback = choice.Feedback,
                LevelUnlocked = levelUnlocked,
                AchievementsUnlocked = achievementsUnlocked
            };
        }

       
        public async Task<List<ChoiceDTO>> GetChoicesByScenarioAsync(int scenarioId)
        {
            return await _context.Choices
                .Where(c => c.ScenarioId == scenarioId)
                .OrderBy(c => c.Id)
                .Select(c => new ChoiceDTO
                {
                    Id = c.Id,
                    ChoiceText = c.ChoiceText,
                    IsCorrect = c.IsCorrect,
                    Feedback = c.Feedback,
                    XpValue = c.XpValue
                })
                .ToListAsync();
        }
    }
}