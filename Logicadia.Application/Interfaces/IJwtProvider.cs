using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logicadia.Domain.Entities;

namespace Logicadia.Application.Interfaces
{
    public interface IJwtProvider
    {
      
        string GenerateToken(User user, string roleName);
    }
}