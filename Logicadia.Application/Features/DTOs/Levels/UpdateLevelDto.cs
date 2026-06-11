using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.Levels
{
    public class UpdateLevelDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public int XpReward { get; set; }
    }
}
