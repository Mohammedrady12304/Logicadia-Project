using Microsoft.AspNetCore.Identity;

namespace LOGICADIA.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string? Permissions { get; set; }
    }
}
