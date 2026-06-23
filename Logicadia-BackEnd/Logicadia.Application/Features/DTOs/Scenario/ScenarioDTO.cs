using Logicadia.Application.Features.DTOs.Choice;

namespace Logicadia.Application.Features.DTOs.Scenario
{
    public class ScenarioDTO
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ScenarioDetailDTO : ScenarioDTO
    {
        public List<ChoiceDTO> Choices { get; set; } = new List<ChoiceDTO>();
    }
}
