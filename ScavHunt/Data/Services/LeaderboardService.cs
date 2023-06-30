﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Player>> Leaderboard(int count = 0)
        {
            using var db = await dbFactory.CreateDbContextAsync();
            var result = db.Players.Include(p => p.PointTransactions).Include(p => p.User).Where(p => p.User.EmailConfirmed).OrderByDescending(p => p.PointTransactions.Sum(p => p.Value)).ThenBy(p => p.PointTransactions.Sum(p => p.Duration));
            if (count > 0)
            {
                return await result.Take(count).ToListAsync();
            }
            else
            {
                return await result.ToListAsync();
            }
        }

        public async Task<int> GetPosition(Player player)
        {
            var leaderboard = await Leaderboard();
            return leaderboard.FindIndex(p => p.User.Id == player.User.Id) + 1;
        }

        public string PointString(List<Player> LeaderboardData, Player player)
        {
            var points = player.PointTransactions.Sum(p => p.Value);
            var others = LeaderboardData?.Count(other => other.PointTransactions.Sum(p => p.Value) == points);
            if (others > 1)
            {
                return $"{player.PointTransactions.Sum(p => p.Value)} ({player.PointTransactions.Sum(p => p.Duration).ToString("F2")} seconds)";
            }
            else
            {
                return player.PointTransactions.Sum(p => p.Value).ToString();
            }
        }
    }
}
