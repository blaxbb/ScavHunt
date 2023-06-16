using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ScavHunt.Data.Models
{
    public class ScavhuntUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        [StringLength(255)]
        public string? BadgeId { get; set; }

        public const string BADGE_REGEX = "^http:\\/\\/rcsreg\\.com\\/(?:.*?\\/){8}$";
        public List<LogRecord> Responses { get; set; }

        public ScavhuntUser() { }
    }
}
