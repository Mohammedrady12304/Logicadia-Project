namespace LOGICADIA.DTOs
{
    public class UserProgressDTO
    {
        public int TotalXp { get; set; }
        public int CurrentLevel { get; set; }
        public int CurrentLevelProgress { get; set; }
        public int XpToNextLevel { get; set; }
        public int ScenariosCompleted { get; set; }
        public int CorrectAnswers { get; set; }
        public double AccuracyPercentage { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
    }
}
