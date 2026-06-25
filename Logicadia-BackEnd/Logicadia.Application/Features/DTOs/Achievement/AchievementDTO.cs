namespace LOGICADIA.DTOs
{
    public class AchievementDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public string TriggerType { get; set; } = null!;
        public int TriggerValue { get; set; }
    }

    public class AchievementUnlockedDTO : AchievementDTO
    {
        public DateTime UnlockedAt { get; set; }
    }

    public class UserAchievementDTO : AchievementDTO
    {
        public DateTime UnlockedAt { get; set; }
        public int Rarity { get; set; } // 1=Common, 2=Rare, 3=Epic, 4=Legendary
    }
}
