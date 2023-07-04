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
            var db = await contextFactory.CreateDbContextAsync();

            return await db.Log.Include(l => l.Question).Include(l => l.User).Where(l => l.Question != null && (l.Type == Models.LogRecord.RecordType.IncorrectAnswer || l.Type == Models.LogRecord.RecordType.CompletedQuestion)).ToListAsync();
        }

        public async Task<Dictionary<long, QuestionAverage>> GetQuestionAverages()
        {
            var db = await contextFactory.CreateDbContextAsync();
            var questions = await db.PointTransactions.Include(t => t.Question).Where(trans => trans.Question != null).ToListAsync();
            var questionTransactions = questions.GroupBy(t => t.Question);
            return questionTransactions.ToDictionary(q => q.Key!.Id, q => new QuestionAverage(q.Average(t => t.Duration), q.Average(t => t.Value)));
        }

        public record QuestionAverage(double duration, double points);
        public record QuestionStats(Question question, int totalPlayers, Dictionary<int, int> responseCounts, QuestionAverage averages);

        public async Task<List<QuestionStats>> GetStats()
        {
            var logs = await GetQuestionLogs();
            var averages = await GetQuestionAverages();

            var result = new List<QuestionStats>();

            var questions = logs.GroupBy(l => l.Question);
            foreach (var q in questions)
            {
                var question = q.Key;
                if (question.Answers == null || question.Answers.Count == 0)
                {
                    result.Add(new QuestionStats(question, q.Count(), new(), new(0, 0)));
                }

                var resp = q.ToList();
                var responses = resp.Where(r => r.ResponseIndex != null).GroupBy(r => r.ResponseIndex);
                var orderedResponses = responses.OrderBy(r => r.Key);

                var players = resp.Where(r => r.User != null).Select(r => r.User.Id).Distinct().Count();
                var responseCounts = orderedResponses.Where(r => r.Key != null).ToDictionary((r) => r.Key.Value, (r) => r.Count());

                var average = new QuestionAverage(0, 0);
                if (averages.ContainsKey(question.Id))
                {
                    average = averages[question.Id];
                }

                result.Add(new QuestionStats(question, players, responseCounts, average));
            }

            return result;
        }
    }
}
