using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PlayerService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;
        protected JSInterop js;

        public PlayerService(IDbContextFactory<ApplicationDbContext> factory, JSInterop jsInterop)
        {
            dbFactory = factory;
            js = jsInterop;
        }

        public virtual Player? GetFromBadge(string badge)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                var player = db.Players
                    .Include(p => p.Responses)
                    .ThenInclude(r => r.Question)
                    .Include(p => p.PointTransactions)
                    .ThenInclude(t => t.Question)
                    .FirstOrDefault(p => p.BadgeNumber == badge);

                return player;
            }
        }

        public async Task<Player> CreateOrExisting(string badge)
        {
            using var db = dbFactory.CreateDbContext();

            var existing = GetFromBadge(badge);
            if(existing != default)
            {
                return existing;
            }

            var player = new Player()
            {
                BadgeNumber = badge,
                Created = DateTime.Now,
            };

            db.Players.Add(player);
            await db.SaveChangesAsync();

            return player;
        }

        public async Task<Player?> GetCurrent()
        {
            var existingBadge = await js.GetStorage("badgeNumber");
            if (string.IsNullOrWhiteSpace(existingBadge))
            {
                return default;
            }

            var existingPlayer = GetFromBadge(existingBadge);
            if (existingPlayer == default)
            {
                return default;
            }

            return existingPlayer;
        }
    }
}
