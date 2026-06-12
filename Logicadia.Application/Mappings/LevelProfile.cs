using AutoMapper;
using Logicadia.Application.Features.DTOs.Levels;
using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Mappings
{
    public class LevelProfile : Profile
    {
        public LevelProfile()
        {
            CreateMap<Level, LevelDto>();
            CreateMap<CreateLevelDto, Level>();
            CreateMap<UpdateLevelDto, Level>();
        }
    }
}
