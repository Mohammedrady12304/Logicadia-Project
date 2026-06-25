using Logicadia.Application.Features.DTOs.Stories;

namespace LOGICADIA.DTOs
{
    public class LevelDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public int XpReward { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime? UnlockedAt { get; set; }
    }

    public class LevelDetailDTO : LevelDTO
    {
        public List<StoryDTO> Stories { get; set; } = new List<StoryDTO>();
    }
}
