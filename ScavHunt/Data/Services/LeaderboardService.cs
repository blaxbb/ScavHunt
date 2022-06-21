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

        public async Task<List<Player>> FullLeaderboard()
        {
            using var db = await dbFactory.CreateDbContextAsync();

            return await db.Players.Include(p => p.PointTransactions).OrderByDescending(p => p.PointTransactions.Sum(p => p.Value)).ToListAsync();
        }
    }
}
