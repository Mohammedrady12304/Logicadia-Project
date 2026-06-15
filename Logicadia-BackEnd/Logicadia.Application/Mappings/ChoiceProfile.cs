using AutoMapper;
using Logicadia.Application.Features.DTOs.Choice;
using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Mappings
{
    public class ChoiceProfile : Profile
    {
        public ChoiceProfile()
        {
            CreateMap<Choice, ChoiceAdminDto>();
            CreateMap<CreateChoiceDto, Choice>();
            CreateMap<UpdateChoiceDto, Choice>();
        }
    }
}
