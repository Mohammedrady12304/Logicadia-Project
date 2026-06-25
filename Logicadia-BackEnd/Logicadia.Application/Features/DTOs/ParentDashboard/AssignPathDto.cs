using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.ParentDashboard
{
    public class AssignPathDto
    {
        public int LevelId { get; set; }
        public int? StoryId { get; set; }
        public int? ScenarioId { get; set; }
    }
}
