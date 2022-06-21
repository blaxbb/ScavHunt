using ScavHunt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ScavHunt.Data.Services
{
    public class PlayerAdminService : PlayerService
    {
        public PlayerAdminService(IDbContextFactory<ApplicationDbContext> factory, JSInterop js) : base(factory, js)
        {

        }

        public List<Player> AllPlayers()
        {
            using var db = dbFactory.CreateDbContext();

            return db.Players.Include(p => p.PointTransactions).ToList();
        }

        public override Player? GetFromBadge(string badge)
        {
            using var db = dbFactory.CreateDbContext();

            return db.Players
                .Include(p => p.Responses)
                .ThenInclude(l => l.Question)
                .Include(p => p.PointTransactions)
                .ThenInclude(t => t.Question)
                .FirstOrDefault(p => p.BadgeNumber == badge);
        }

        public async Task Delete(Player player)
        {
            using var db = await dbFactory.CreateDbContextAsync();

            var existing = await db.Players.Where(p => p.BadgeNumber == player.BadgeNumber).FirstOrDefaultAsync();
            if(existing != null)
            {
                db.Players.Remove(existing);
                await db.SaveChangesAsync();
            }
        }
    }
}
