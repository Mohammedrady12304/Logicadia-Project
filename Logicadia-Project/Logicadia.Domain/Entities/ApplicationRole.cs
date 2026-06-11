using Microsoft.AspNetCore.Identity;

namespace Logicadia.Domain.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string? Permissions { get; set; }
    }
}
