using System.Collections.Generic;

namespace Logicadia.Domain.Entities
{
    public class Choice
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public string? Feedback { get; set; }
        public int XpValue { get; set; }

        // Navigation
        public virtual Scenario Scenario { get; set; } = null!;
        public virtual ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
    }

}