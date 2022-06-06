using ScavHunt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ScavHunt.Data.Services
{
    public class PlayerAdminService : PlayerService
    {
        public PlayerAdminService(ApplicationDbContext db, JSInterop js) : base(db, js)
        {

        }

        public List<Player> AllPlayers()
        {
            return db.Players.Include(p => p.PointTransactions).ToList();
        }
    }
}
