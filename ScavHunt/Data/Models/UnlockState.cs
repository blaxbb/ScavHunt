namespace ScavHunt.Data.Models
{
    public class UnlockState
    {
        public long Id { get; set; }

        public Player Player { get; set; }
        public Question Question { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}