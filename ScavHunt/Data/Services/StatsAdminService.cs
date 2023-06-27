using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class StatsAdminService
    {
        private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        public StatsAdminService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<List<LogRecord>> GetQuestionLogs()
        {
            var db = contextFactory.CreateDbContext();

            return await db.Log.Include(l => l.Question).Include(l => l.User).Where(l => l.Question != null && (l.Type == Models.LogRecord.RecordType.IncorrectAnswer || l.Type == Models.LogRecord.RecordType.CompletedQuestion)).ToListAsync();
        }


        public record QuestionStats(Question question, int totalPlayers, Dictionary<int, int> responseCounts);

        public async Task<List<QuestionStats>> GetStats()
        {
            var logs = await GetQuestionLogs();

            var result = new List<QuestionStats>();

            var questions = logs.GroupBy(l => l.Question);
            foreach(var q in  questions)
            {
                var question = q.Key;
                if(question.Answers == null || question.Answers.Count == 0)
                {
                    result.Add(new QuestionStats(question, q.Count(), new ()));
                }

                var resp = q.ToList();
                var responses = resp.Where(r => r.ResponseIndex != null).GroupBy(r => r.ResponseIndex);
                var orderedResponses = responses.OrderBy(r => r.Key);

                var players = resp.Where(r => r.User != null).Select(r => r.User.Id).Distinct().Count();
                var responseCounts = orderedResponses.Where(r => r.Key != null).ToDictionary((r) => r.Key.Value, (r) => r.Count());

                result.Add(new QuestionStats(question, players, responseCounts));

            }

            return result;
        }
    }
}
