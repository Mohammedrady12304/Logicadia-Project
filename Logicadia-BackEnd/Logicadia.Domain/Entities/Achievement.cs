using System.Collections.Generic;

namespace Logicadia.Domain.Entities
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public string TriggerType { get; set; } = null!;
        public int TriggerValue { get; set; }

        // Navigation
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    }
}
