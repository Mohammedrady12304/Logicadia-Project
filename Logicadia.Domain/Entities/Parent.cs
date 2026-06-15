using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logicadia.Domain.Entities
{
    public class Parent : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

       
        public int UserId { get; set; }
        public User User { get; set; } = null!;

    
        public ICollection<Child> Children { get; set; } = new List<Child>();
    }
}
