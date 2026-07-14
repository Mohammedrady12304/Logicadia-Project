using Logicadia.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logicadia.Domain.Entities
{
    public class Child : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public string Interests { get; set; } 

        
        public string FavoriteColor { get; set; } 

        
        public string FavoriteAnimal { get; set; } 

       
        public string LearningTopic { get; set; } 

        
        public string ReadingLevel { get; set; } 

        
        public string PreferredLanguage { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public int ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
    }
}
