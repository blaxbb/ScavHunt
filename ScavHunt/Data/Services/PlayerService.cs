using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;
using ScavHunt.Pages.Admin;

namespace ScavHunt.Data.Services
{
    public class PlayerService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;
        private readonly AuthenticationStateProvider authProvider;

        public PlayerService(IDbContextFactory<ApplicationDbContext> factory, AuthenticationStateProvider auth)
        {
            dbFactory = factory;
            authProvider = auth;
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
            var authStatus = await authProvider.GetAuthenticationStateAsync();

            if (authStatus.User?.Identity?.Name != null)
            {
                return GetFromBadge(authStatus.User.Identity.Name);
            }

            return default;
        }
    }
}
