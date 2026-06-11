using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Logicadia.Domain.Entities;
using Logicadia.Application.Features.DTOs.Levels;
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
