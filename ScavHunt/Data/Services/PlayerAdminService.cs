using ScavHunt.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ScavHunt.Data.Services
{
    public class PlayerAdminService : PlayerService
    {
        public PlayerAdminService(IDbContextFactory<ApplicationDbContext> factory, AuthenticationStateProvider auth, UserManager<ScavhuntUser> users) : base(factory, auth, users)
        {

        }

        public List<Player> AllPlayers()
        {
            using var db = dbFactory.CreateDbContext();

            return db.Players.Include(p => p.User).Include(p => p.PointTransactions).ToList();
        }

        public override Player? GetFromUsername(string username)
        {
            using var db = dbFactory.CreateDbContext();

            return db.Players
                .Include(p => p.Responses)
                .ThenInclude(l => l.Question)
                .Include(p => p.PointTransactions)
                .ThenInclude(t => t.Question)
                .Include(p => p.User)
                .FirstOrDefault(p => p.User.UserName == username);
        }

        public async Task Delete(Player player)
        {
            using var db = await dbFactory.CreateDbContextAsync();

            var existing = await db.Players.Include(p => p.User).Where(p => p.User.Id == player.User.Id).FirstOrDefaultAsync();
            if(existing != null)
            {
                db.Players.Remove(existing);
                await db.SaveChangesAsync();
            }
        }
    }
}
