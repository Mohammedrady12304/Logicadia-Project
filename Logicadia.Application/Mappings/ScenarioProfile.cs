using AutoMapper;
using Logicadia.Application.Features.DTOs.Scenario;
using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Mappings
{
    public class ScenarioProfile : Profile
    {
        public ScenarioProfile()
        {
            CreateMap<Scenario, ScenarioAdminDto>()
                .ForMember(dest => dest.ChoicesCount,
                           opt => opt.MapFrom(src => src.Choices.Count));

            CreateMap<CreateScenarioDto, Scenario>();
            CreateMap<UpdateScenarioDto, Scenario>();
        }
    }
}