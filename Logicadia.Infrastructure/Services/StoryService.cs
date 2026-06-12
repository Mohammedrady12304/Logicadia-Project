using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Stories;
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
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _repo;
        private readonly IMapper _mapper;

        public StoryService(IStoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ============ Admin ============

        public async Task<IEnumerable<StoryAdminDto>> GetAllForAdminAsync()
        {
            var stories = await _repo.GetAllAsync();
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
        public async Task<IEnumerable<StoryAdminDto>> GetByLevelIdForAdminAsync(int levelId)
        {
            var stories = await _repo.GetByLevelIdAsync(levelId);
            return _mapper.Map<IEnumerable<StoryAdminDto>>(stories);
        }

        public async Task<StoryAdminDto?> GetByIdForAdminAsync(int id)
        {
            var story = await _repo.GetByIdAsync(id);
            if (story is null) return null;
            return _mapper.Map<StoryAdminDto>(story);
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

    }
}
