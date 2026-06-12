using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.Achievement
{
    public class UpdateAchievementDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public string TriggerType { get; set; } = null!;
        public int TriggerValue { get; set; }
    }
}
