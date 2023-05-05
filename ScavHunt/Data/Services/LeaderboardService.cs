using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class LeaderboardService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;

        public LeaderboardService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<List<Player>> Leaderboard()
        {
            using var db = await dbFactory.CreateDbContextAsync();

            return await db.Players.Include(p => p.PointTransactions).Include(p => p.User).OrderByDescending(p => p.PointTransactions.Sum(p => p.Value)).ToListAsync();
        }

        public async Task<List<Player>> ShortLeaderboard()
        {
            using var db = await dbFactory.CreateDbContextAsync();

            return await db.Players.Include(p => p.PointTransactions).Include(p => p.User).OrderByDescending(p => p.PointTransactions.Sum(p => p.Value)).Take(5).ToListAsync();
        }

        public async Task<int> GetPosition(Player player)
        {
            var leaderboard = await Leaderboard();
            return leaderboard.FindIndex(p => p.User.Id == player.User.Id) + 1;
        }
    }
}
