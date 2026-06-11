using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Domain.Entities
{
    public class Story
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public string Title { get; set; } = null!;
        public string? NarrativeText { get; set; }
        public int OrderIndex { get; set; }

        // Navigation
        public virtual Level Level { get; set; } = null!;
        public virtual ICollection<Scenario> Scenarios { get; set; } = new List<Scenario>();
    }
}
