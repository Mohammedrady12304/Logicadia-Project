using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Domain.Entities
{
    public class Scenario
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }

        // Navigation
        public virtual Story Story { get; set; } = null!;
        public virtual ICollection<Choice> Choices { get; set; } = new List<Choice>();
    }
}
