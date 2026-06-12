using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.Stories
{
    public class CreateStoryDto
    {
        public int LevelId { get; set; }
        public string Title { get; set; } = null!;
        public string? NarrativeText { get; set; }
        public int OrderIndex { get; set; }
    }
}
