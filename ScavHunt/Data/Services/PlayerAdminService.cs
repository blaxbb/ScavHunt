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

        public override Player? GetFromBadge(string badge)
        {
            return db.Players
                .Include(p => p.Responses)
                .ThenInclude(l => l.Question)
                .Include(p => p.PointTransactions)
                .ThenInclude(t => t.Question)
                .FirstOrDefault(p => p.BadgeNumber == badge);
        }
    }
}
