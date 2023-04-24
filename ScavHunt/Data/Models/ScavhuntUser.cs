using Microsoft.AspNetCore.Identity;

namespace ScavHunt.Data.Models
{
    public class ScavhuntUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        public ScavhuntUser() { }
    }
}
