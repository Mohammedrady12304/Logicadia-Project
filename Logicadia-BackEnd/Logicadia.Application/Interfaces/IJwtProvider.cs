using Logicadia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Interfaces
{
    public interface IJwtProvider
    {

        string GenerateToken(ApplicationUser user, string roleName);
       // string GenerateToken(ApplicationUser user, object name);
    }
}
