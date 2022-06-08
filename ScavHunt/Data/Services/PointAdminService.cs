using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components.Authorization;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PointAdminService : PointService
    {
        AuthenticationStateProvider auth;

        public PointAdminService(ApplicationDbContext context, LogService logService, AuthenticationStateProvider authProvider) : base(context, logService)
        {
            auth = authProvider;
        }

        public async Task<bool> Reset(Player player, Question question)
        {
            var state = await auth.GetAuthenticationStateAsync();
            var adminName = state?.User?.Identity?.Name;

            if (string.IsNullOrWhiteSpace(adminName))
            {
                return false;
            }

            var points = db.PointTransactions.Where(p => p.Source == PointTransaction.PointSource.Question && p.Player == player && p.Question == question);
            var totalPoints = points.Sum(p => p.Value);
            db.PointTransactions.RemoveRange(points);

            var logs = db.Log.Where(l => l.Type == LogRecord.RecordType.StartedQuestion && l.Player == player && l.Question == question);
            db.Log.RemoveRange(logs);


            await db.SaveChangesAsync();

            await log.Create(new LogRecord()
            {
                Message = $"Admin {adminName} RESET PROGRESS on question {question.ShortCode} for player {player.BadgeNumber} removing {totalPoints} points.",
                Player = player,
                Question = question,
                Type = LogRecord.RecordType.Player,
                Timestamp = DateTime.Now
            });

            return true;
        }
    }
}
