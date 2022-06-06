namespace ScavHunt.Data.Models
{
    public class PointTransaction
    {
        public long Id { get; set; }

        public Player Player { get; set; }
        public int Value { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public Question? Question { get; set; }

        public PointSource Source { get; set; }
        public enum PointSource
        {
            Question,
            Manual
        }

    }
}
