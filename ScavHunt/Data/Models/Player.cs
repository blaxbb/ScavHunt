using System.Text.RegularExpressions;

namespace ScavHunt.Data.Models
{
    public class Player
    {
        public long Id { get; set; }

        public ScavhuntUser User { get; set; }

        public DateTime Created { get; set; }

        public static Regex BadgeFormat = new Regex("^\\d{10}$");

        public List<PointTransaction> PointTransactions { get; set; }
    }
}
