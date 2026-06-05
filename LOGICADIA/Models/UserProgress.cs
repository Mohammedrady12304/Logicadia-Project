using System;

namespace LOGICADIA.Models
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

        // Navigation
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Scenario Scenario { get; set; } = null!;
        public virtual Choice ChosenChoice { get; set; } = null!;
    }
}
