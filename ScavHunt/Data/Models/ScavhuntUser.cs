using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ScavHunt.Data.Models
{
    public class ScavhuntUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        [StringLength(255)]
        public string? BadgeId { get; set; }

        public ScavhuntUser() { }
    }
}
