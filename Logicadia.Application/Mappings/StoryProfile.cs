using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logicadia.Domain.Entities;
using Logicadia.Application.Features.DTOs.Stories;
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
