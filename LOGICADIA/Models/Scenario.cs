using System.Collections.Generic;

namespace LOGICADIA.Models
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
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
    }
}
