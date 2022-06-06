using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PlayerService
    {
        protected ApplicationDbContext db;
        protected JSInterop js;

        public PlayerService(ApplicationDbContext ctx, JSInterop jsInterop)
        {
            db = ctx;
            js = jsInterop;
        }

        public Player? GetFromBadge(string badge)
        {
            return db.Players
                .Include(p => p.PointTransactions)
                .ThenInclude(t => t.Question)
                .FirstOrDefault(p => p.BadgeNumber == badge);
        }

        public async Task<Player> CreateOrExisting(string badge)
        {
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
