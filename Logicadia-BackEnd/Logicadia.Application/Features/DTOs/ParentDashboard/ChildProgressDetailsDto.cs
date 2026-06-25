using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.ParentDashboard
{
    public class ChildProgressDetailsDto
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public int CurrentLevelId { get; set; }
        public string CurrentLevelName { get; set; }
        public int? CurrentStoryId { get; set; }
        public string CurrentStoryTitle { get; set; }
        public int? CurrentScenarioId { get; set; }
        public string CurrentScenarioName { get; set; }
        public List<string> UnlockedAchievements { get; set; } = new();
    }
}
