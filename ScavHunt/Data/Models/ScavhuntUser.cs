using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ScavHunt.Data.Models
{
    public class ScavhuntUser : IdentityUser
    {
        [PersonalData]
        public string? DisplayName { get; set; }

        [StringLength(255)]
        [PersonalData]
        public string? BadgeId { get; set; }

        public List<LogRecord> Responses { get; set; }

        public ScavhuntUser() { }
    }
}
