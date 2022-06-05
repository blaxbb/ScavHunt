using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PlayerService
    {
        protected ApplicationDbContext db;

        public PlayerService(ApplicationDbContext ctx)
        {
            db = ctx;
        }

        public Player? GetFromBadge(string badge)
        {
            var player = db.Players.FirstOrDefault(p => p.BadgeNumber == badge);
            return player;
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
    }
}
