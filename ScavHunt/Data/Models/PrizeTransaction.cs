using System.ComponentModel.DataAnnotations;

namespace ScavHunt.Data.Models
{
    public class PrizeTransaction
    {
        public long Id { get; set; }
        [StringLength(250)]
        public string? Badge { get; set; } = null;
        public ScavhuntUser? User { get; set; } = null;
        public Prize Prize { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ScavhuntUser CreatedBy { get; set; }

    }
}
