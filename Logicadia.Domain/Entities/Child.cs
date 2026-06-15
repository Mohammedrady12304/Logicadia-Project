using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Logicadia.Domain.Entities
{
    public class Child : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

       
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
    }
}
