using System;

namespace Logicadia.Domain.Entities
{
    public class UserProgress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScenarioId { get; set; }
        public int ChosenChoiceId { get; set; }
        public bool IsCorrect { get; set; }
        public int XpEarned { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
        public int ChildId { get; set; }
        public Child Child { get; set; }
        public int LevelId { get; set; }
        public Level Level { get; set; }
        public int? StoryId { get; set; }
        public Story Story { get; set; }

        
        
        // Navigation
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Scenario Scenario { get; set; } = null!;
        public virtual Choice ChosenChoice { get; set; } = null!;
    }
}
