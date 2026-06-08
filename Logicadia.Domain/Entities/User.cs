using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
