using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;
using static ScavHunt.Data.Models.LogRecord;

namespace ScavHunt.Data.Services
{
    public class LogService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;

        public LogService(IDbContextFactory<ApplicationDbContext> factory)
        {
            dbFactory = factory;
        }

        public async Task Question(Player player, Question question, RecordType type, string message, int? responseIndex = null)
        {
            var record = QuestionLog(question, player.User, type, message, responseIndex);
            await Create(record);
        }

        public async Task Message(RecordType type, string message)
        {
            var record = MessageLog(type, message);
            await Create(record);
        }

        public async Task Prize(PrizeTransaction prize)
        {
            var record = PrizeLog(prize);
            await Create(record);
        }

        public async Task<LogRecord> Create(LogRecord record)
        {
            using var db = dbFactory.CreateDbContext();
            db.ChangeTracker.Clear();
            if (record.User != null)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == record.User.Id);
                record.User = user;
            }
            if(record.Question != null)
            {
                var question = db.Questions.FirstOrDefault(q => q.Id == record.Question.Id);
                record.Question = question;
            }

            var created = db.Log.Add(record);
            await db.SaveChangesAsync();

            return created.Entity;
        }
    }
}