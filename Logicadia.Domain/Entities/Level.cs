using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Domain.Entities
{
    public class Level
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public int XpReward { get; set; }

        // Navigation
        public virtual ICollection<Story> Stories { get; set; } = new List<Story>();
    }
}
