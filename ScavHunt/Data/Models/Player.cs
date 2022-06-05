using System.Text.RegularExpressions;

namespace ScavHunt.Data.Models
{
    public class Player
    {
        public long Id { get; set; }

        public string BadgeNumber { get; set; }
        public DateTime Created { get; set; }
        public int Points { get; set; }

        public static Regex BadgeFormat = new Regex("^\\d{10}$");
    }
}
