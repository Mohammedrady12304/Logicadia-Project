using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Achievement;
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
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _repo;
        private readonly IMapper _mapper;

        public AchievementService(IAchievementRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ============ Admin ============

        public async Task<IEnumerable<AchievementAdminDto>> GetAllForAdminAsync()
        {
            var achievements = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<AchievementAdminDto>>(achievements);
        }
        public async Task<PagedResult<AchievementAdminDto>> GetPagedForAdminAsync(PaginationParams pagination)
        {
            var (data, totalCount) = await _repo.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            return new PagedResult<AchievementAdminDto>
            {
                Data = _mapper.Map<IEnumerable<AchievementAdminDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
        public async Task<AchievementAdminDto?> GetByIdForAdminAsync(int id)
        {
            var achievement = await _repo.GetByIdAsync(id);
            if (achievement is null) return null;
            return _mapper.Map<AchievementAdminDto>(achievement);
        }

        public async Task<AchievementAdminDto> CreateAsync(CreateAchievementDto dto)
        {
            var achievement = _mapper.Map<Achievement>(dto);
            await _repo.AddAsync(achievement);
            return _mapper.Map<AchievementAdminDto>(achievement);
        }

        public async Task<AchievementAdminDto> UpdateAsync(int id, UpdateAchievementDto dto)
        {
            var achievement = await _repo.GetByIdAsync(id);
            if (achievement is null) throw new NotFoundException(nameof(Achievement), id);
            _mapper.Map(dto, achievement);
            await _repo.UpdateAsync(achievement);
            return _mapper.Map<AchievementAdminDto>(achievement);
        }
        public async Task DeleteAsync(int id)
        {
            var achievement = await _repo.GetByIdAsync(id);
            if (achievement is null) throw new NotFoundException(nameof(Achievement), id);
            await _repo.DeleteAsync(achievement);
        }

        // ============ User ============
    }
}
