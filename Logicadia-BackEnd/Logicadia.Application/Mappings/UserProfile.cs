using AutoMapper;
using Logicadia.Application.Features.DTOs.Users;
using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserAdminDto>()
                .ForMember(dest => dest.IsLocked,
                           opt => opt.MapFrom(src => src.LockoutEnd.HasValue &&
                                                     src.LockoutEnd > DateTimeOffset.UtcNow));
        }
    }
}
