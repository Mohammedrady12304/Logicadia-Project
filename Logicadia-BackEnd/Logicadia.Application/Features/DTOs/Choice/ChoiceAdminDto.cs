using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.Choice
{
    public class ChoiceAdminDto
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public string? Feedback { get; set; }
        public int XpValue { get; set; }
    }
}
