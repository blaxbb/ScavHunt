namespace ScavHunt.Data.Models
{
    public class Prize
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public PrizeType Type { get; set; }
        public enum PrizeType
        {
            Small,
            Daily,
            Grand
        }
        public ICollection<PrizeTransaction> Transactions { get; } = new List<PrizeTransaction>();
    }
}
