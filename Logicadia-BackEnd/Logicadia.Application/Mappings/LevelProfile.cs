using AutoMapper;
using Logicadia.Application.Features.DTOs.Levels;
using Logicadia.Domain.Entities;

namespace Logicadia.Application.Mappings
{
    public class LevelProfile : Profile
    {
        public LevelProfile()
        {
            CreateMap<Level, LevelDto>();

            CreateMap<Level, LevelAdminDto>()
                .ForMember(
                    dest => dest.StoriesCount,
                    opt => opt.MapFrom(src => src.Stories.Count)
                );

            CreateMap<CreateLevelDto, Level>();

            CreateMap<UpdateLevelDto, Level>();
        }
    }
}