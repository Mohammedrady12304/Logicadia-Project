using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.Scenario
{
    public class ScenarioAdminDto
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public int ChoicesCount { get; set; }
    }
}
