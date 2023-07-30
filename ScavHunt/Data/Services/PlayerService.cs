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

        public virtual async Task<Player?> GetFromUsernameFull(string username)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                return await db.Players
                    .Include(p => p.PointTransactions)
                    .ThenInclude(t => t.Question)
                    .Include(p => p.User)
                    .ThenInclude(p => p.Responses)
                    .ThenInclude(r => r.Question)
                    .FirstOrDefaultAsync(p => p.User.UserName == username);
            }
        }

        public virtual async Task<Player?> GetFromUsernameQuestionDetails(string username, long questionId)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                return await db.Players
                    .Include(p => p.PointTransactions.Where(transaction => transaction.Question != null && transaction.Question.Id == questionId))
                    .ThenInclude(t => t.Question)
                    .Include(p => p.User)
                    .ThenInclude(p => p.Responses)
                    .FirstOrDefaultAsync(p => p.User.UserName == username);
            }
        }

        public virtual async Task<Player?> GetFromUsernameWithPointTransactions(string username)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                return await db.Players
                    .Include(p => p.User)
                    .Include(p => p.PointTransactions)
                    .ThenInclude(t => t.Question)
                    .FirstOrDefaultAsync(p => p.User.UserName == username);
            }
        }

        public virtual async Task<Player?> GetFromUsername(string username)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                return await db.Players
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.User.UserName == username);
            }
        }

        public virtual async Task<Player?> GetFromBadge(string badge)
        {
            using (var db = dbFactory.CreateDbContext())
            {
                return await db.Players
                    .Include(p => p.PointTransactions)
                    .ThenInclude(t => t.Question)
                    .Include(p => p.User)
                    .ThenInclude(p => p.Responses)
                    .ThenInclude(r => r.Question)
                    .FirstOrDefaultAsync(p => p.User.BadgeId == badge);
            }
        }

        public async Task<Player?> GetCurrentFull()
        {
            var authStatus = await authProvider.GetAuthenticationStateAsync();

            if (authStatus.User?.Identity?.Name != null)
            {
                return await GetFromUsernameFull(authStatus.User.Identity.Name);
            }

            return default;
        }

        public async Task<Player?> GetCurrentWithPointTransactions()
        {
            var authStatus = await authProvider.GetAuthenticationStateAsync();

            if (authStatus.User?.Identity?.Name != null)
            {
                return await GetFromUsernameWithPointTransactions(authStatus.User.Identity.Name);
            }

            return default;
        }

        public async Task<Player?> GetCurrentQuestionDetails(long questionId)
        {
            var authStatus = await authProvider.GetAuthenticationStateAsync();

            if (authStatus.User?.Identity?.Name != null)
            {
                return await GetFromUsernameQuestionDetails(authStatus.User.Identity.Name, questionId);
            }

            return default;
        }

        public async Task<Player?> GetCurrent()
        {
            var authStatus = await authProvider.GetAuthenticationStateAsync();

            if (authStatus.User?.Identity?.Name != null)
            {
                return await GetFromUsername(authStatus.User.Identity.Name);
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
