using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;

namespace Logicadia.Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        // public string Username { get; set; } = null!;
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public ApplicationUser Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }


        // Navigation
        public virtual ICollection<UserProgress> Progress { get; set; } = new List<UserProgress>();
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
        public virtual Subscription? Subscription { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
