using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PlayerAdminService : PlayerService
    {
        public PlayerAdminService(ApplicationDbContext db) : base(db)
        {

        }

        public List<Player> AllPlayers()
        {
            return db.Players.ToList();
        }
    }
}
