using AutoMapper;
using Logicadia.Application.Features.DTOs.Stories;
using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Mappings
{
    public class StoryProfile : Profile
    {
        public StoryProfile()
        {
            CreateMap<Story, StoryAdminDto>()
                .ForMember(dest => dest.ScenariosCount,
                           opt => opt.MapFrom(src => src.Scenarios.Count));

            CreateMap<CreateStoryDto, Story>();
            CreateMap<UpdateStoryDto, Story>();
        }
    }
}
