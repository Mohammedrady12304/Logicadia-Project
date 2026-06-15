using AutoMapper;
using Logicadia.Application.Exceptions;
using Logicadia.Application.Features.DTOs.Choice;
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
    public class ChoiceService : IChoiceService
    {
        private readonly IChoiceRepository _repo;
        private readonly IMapper _mapper;

        public ChoiceService(IChoiceRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ============ Admin ============

        public async Task<IEnumerable<ChoiceAdminDto>> GetAllForAdminAsync()
        {
            var choices = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ChoiceAdminDto>>(choices);
        }
        public async Task<IEnumerable<ChoiceAdminDto>> GetByScenarioIdForAdminAsync(int scenarioId)
        {
            var choices = await _repo.GetByScenarioIdAsync(scenarioId);
            return _mapper.Map<IEnumerable<ChoiceAdminDto>>(choices);
        }

        public async Task<ChoiceAdminDto?> GetByIdForAdminAsync(int id)
        {
            var choice = await _repo.GetByIdAsync(id);
            if (choice is null) return null;
            return _mapper.Map<ChoiceAdminDto>(choice);
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
        public async Task DeleteAsync(int id)
        {
            var choice = await _repo.GetByIdAsync(id);
            if (choice is null) throw new NotFoundException(nameof(Choice), id);
            await _repo.DeleteAsync(choice);
        }

        // ============ User ============
    }
}
