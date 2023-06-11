namespace ScavHunt.Data.Models
{
    public class PrizeTransaction
    {
        public long Id { get; set; }
        public ScavhuntUser User { get; set; } = null!;
        public Prize Prize { get; set; } = null!;

    }
}
