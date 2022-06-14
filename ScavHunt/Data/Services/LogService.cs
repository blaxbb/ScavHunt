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

        public async Task Question(Player player, Question question, RecordType type, string message)
        {
            var record = QuestionLog(question, player, type, message);
            await Create(record);
        }

        public async Task Message(RecordType type, string message)
        {
            var record = MessageLog(type, message);
            await Create(record);
        }

        public async Task<LogRecord> Create(LogRecord record)
        {
            using var db = dbFactory.CreateDbContext();

            db.Attach(record);

            db.Log.Add(record);
            await db.SaveChangesAsync();

            return record;
        }
    }
}