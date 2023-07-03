using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PointAdminService : PointService
    {
        AuthenticationStateProvider auth;
        private readonly UserManager<ScavhuntUser> userManager;

        public PointAdminService(IDbContextFactory<ApplicationDbContext> factory, LogService logService, AuthenticationStateProvider authProvider, UserManager<ScavhuntUser> userManager) : base(factory, logService)
        {
            auth = authProvider;
            this.userManager = userManager;
        }

        public async Task<bool> Reset(Player player, Question question, string message)
        {
            using var db = dbFactory.CreateDbContext();

            var state = await auth.GetAuthenticationStateAsync();
            if (state == null)
            {
                return false;
            }

            var adminUser = await userManager.GetUserAsync(state?.User);
            var adminName = adminUser?.DisplayName ?? null;


            if (string.IsNullOrWhiteSpace(adminName))
            {
                return false;
            }

            var points = db.PointTransactions.Where(p => p.Source == PointTransaction.PointSource.Question && p.Player == player && p.Question == question);
            var totalPoints = points.Sum(p => p.Value);
            db.PointTransactions.RemoveRange(points);

            var logs = db.Log.Where(l => (l.Type == LogRecord.RecordType.StartedQuestion || l.Type == LogRecord.RecordType.IncorrectAnswer) && l.User == player.User && l.Question == question);
            db.Log.RemoveRange(logs);


            await db.SaveChangesAsync();

            await log.Create(new LogRecord()
            {
                Message = $"Admin {adminName} RESET PROGRESS on question {question.ShortCode} for player {player.User.UserName} removing {totalPoints} points.  {message}",
                User = player.User,
                Question = question,
                Type = LogRecord.RecordType.Player,
                Timestamp = DateTime.Now
            });

            return true;
        }
    }
}
