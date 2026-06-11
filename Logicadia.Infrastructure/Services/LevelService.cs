using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Levels;
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
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _repo;
        private readonly IMapper _mapper;

        public LevelService(ILevelRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LevelAdminDto>> GetAllForAdminAsync()
        {
            var levels = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<LevelAdminDto>>(levels);
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
        public async Task<LevelAdminDto?> GetByIdForAdminAsync(int id)
        {
            var level = await _repo.GetByIdAsync(id);
            if (level is null) return null;
            return _mapper.Map<LevelAdminDto>(level);
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

    }
}
