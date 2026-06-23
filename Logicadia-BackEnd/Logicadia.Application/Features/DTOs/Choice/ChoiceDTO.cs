using LOGICADIA.DTOs;

namespace Logicadia.Application.Features.DTOs.Choice
{
    public class ChoiceDTO
    {
        public int Id { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public string? Feedback { get; set; }
        public int XpValue { get; set; }
    }

    public class ChoiceSubmitRequest
    {
        public int ScenarioId { get; set; }
        public int ChoiceId { get; set; }
    }

    public class ChoiceSubmitResponse
    {
        public bool IsCorrect { get; set; }
        public int XpEarned { get; set; }
        public string? Feedback { get; set; }
        public bool LevelUnlocked { get; set; }
        public List<AchievementUnlockedDTO> AchievementsUnlocked { get; set; } = new List<AchievementUnlockedDTO>();
    }
}
