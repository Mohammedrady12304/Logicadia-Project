using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.Users
{
    // UpdateUserDto.cs
    public class UpdateUserDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
