using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;
using ScavHunt.Pages.Admin;

namespace ScavHunt.Data.Services
{
    public class PlayerService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;
        protected readonly AuthenticationStateProvider authProvider;
        protected readonly UserManager<ScavhuntUser> userManager;

        public PlayerService(IDbContextFactory<ApplicationDbContext> factory, AuthenticationStateProvider auth, UserManager<ScavhuntUser> users)
        {
            dbFactory = factory;
            authProvider = auth;
            userManager = users;
        }

        public virtual Player? GetFromUsername(string badge)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                var player = db.Players
                    .Include(p => p.Responses)
                    .ThenInclude(r => r.Question)
                    .Include(p => p.PointTransactions)
                    .ThenInclude(t => t.Question)
                    .Include(p => p.User)
                    .FirstOrDefault(p => p.User.UserName == badge);

                return player;
            }
        }

        public async Task<Player> CreateOrExisting(string badge)
        {
            using var db = dbFactory.CreateDbContext();

            var existing = GetFromUsername(badge);
            if (existing != default)
            {
                return existing;
            }

            var player = new Player()
            {
                Created = DateTime.Now,
            };

            //db.Players.Add(player);
            //await db.SaveChangesAsync();

            return player;
        }

        public async Task<Player?> GetCurrent()
        {
            var authStatus = await authProvider.GetAuthenticationStateAsync();

            if (authStatus.User?.Identity?.Name != null)
            {
                return GetFromUsername(authStatus.User.Identity.Name);
            }

            return default;
        }

        public async Task SetBadgeId(string badgeId)
        {
            var authStatus = await authProvider.GetAuthenticationStateAsync();

            if (authStatus?.User?.Identity?.Name != null && authStatus.User.Identity.IsAuthenticated)
            {
                var user = await userManager.FindByNameAsync(authStatus.User.Identity.Name);
                user.BadgeId = badgeId;
                await userManager.UpdateAsync(user);
            }
        }
    }
}
