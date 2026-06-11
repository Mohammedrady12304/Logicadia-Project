using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Domain.Entities
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        // Navigation
      //  public virtual ApplicationUser User { get; set; } = null!;
        public virtual Achievement Achievement { get; set; } = null!;
    }
}
