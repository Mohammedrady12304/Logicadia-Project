using Logicadia.Application.Features.DTOs.Scenario;

namespace Logicadia.Application.Features.DTOs.Stories
{
    public class StoryDTO
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public string Title { get; set; } = null!;
        public string? NarrativeText { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class StoryDetailDTO : StoryDTO
    {
        public List<ScenarioDTO> Scenarios { get; set; } = new List<ScenarioDTO>();
    }
}
