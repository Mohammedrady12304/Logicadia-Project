namespace LOGICADIA.DTOs
{
    public class LeaderboardEntryDTO
    {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public int TotalXp { get; set; }
        public int Level { get; set; }
        public int AchievementsCount { get; set; }
    }

    public class WeeklyStatisticsDTO
    {
        public int WeekNumber { get; set; }
        public int Year { get; set; }
        public int XpEarned { get; set; }
        public int ScenariosCompleted { get; set; }
        public int AchievementsUnlocked { get; set; }
        public double AccuracyPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
