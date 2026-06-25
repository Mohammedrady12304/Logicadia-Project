using System;


namespace Logicadia.Domain.Entities
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        public int ChildId { get; set; }
        public Child Child { get; set; }

        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;


        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Achievement Achievement { get; set; } = null!;
    }
}
