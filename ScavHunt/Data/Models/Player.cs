using System.Text.RegularExpressions;

namespace ScavHunt.Data.Models
{
    public class Player
    {
        public long Id { get; set; }

        public string BadgeNumber { get; set; }
        public DateTime Created { get; set; }

        public static Regex BadgeFormat = new Regex("^\\d{10}$");

        public List<LogRecord> Responses { get; set; }
        public List<PointTransaction> PointTransactions { get; set; }
    }
}
