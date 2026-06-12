using AutoMapper;
using Logicadia.Application.Features.DTOs.Achievement;
using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Mappings
{
    public class AchievementProfile : Profile
    {
        public AchievementProfile()
        {
            CreateMap<Achievement, AchievementAdminDto>()
                .ForMember(dest => dest.UserAchievementsCount,
                           opt => opt.MapFrom(src => src.UserAchievements.Count));

            CreateMap<CreateAchievementDto, Achievement>();
            CreateMap<UpdateAchievementDto, Achievement>();
        }
    }
}
