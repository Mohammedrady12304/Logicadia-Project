using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.ParentDashboard
{
    public class ChildSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string CurrentLevelName { get; set; }
        public int TotalAchievements { get; set; }
    }
}
