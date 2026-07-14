using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Application.Features.DTOs.ParentDashboard
{
    public class AssignPathDto
    {
        [Required]
        public int Age { get; set; }

        [Required]
        [MaxLength(250)]
        public string Interests { get; set; }

        [Required]
        [MaxLength(50)]
        public string FavoriteColor { get; set; }

        [Required]
        [MaxLength(50)]
        public string FavoriteAnimal { get; set; }

        [Required]
        [MaxLength(100)]
        public string LearningTopic { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReadingLevel { get; set; }

        [Required]
        [MaxLength(50)]
        public string PreferredLanguage { get; set; }
    }
}
