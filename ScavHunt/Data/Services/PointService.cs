using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;
using static ScavHunt.Data.Models.PointTransaction;

namespace ScavHunt.Data.Services
{
    public class PointService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;
        protected LogService log;

        public PointService(IDbContextFactory<ApplicationDbContext> factory, LogService logService)
        {
            dbFactory = factory;
            log = logService;
        }

        public async Task<PointTransaction?> AddPoints(Player player, Question question, int value)
        {
            var transaction = new PointTransaction()
            {
                Player = player,
                Source = PointSource.Question,
                Value = value,
                Question = question
            };

            return await AddPoints(transaction);
        }

        //public async Task<PointTransaction?> AddPoints(Player player, PointSource source, int value)
        //{
        //    var transaction = new PointTransaction()
        //    {
        //        Player = player,
        //        Source = source,
        //        Value = value
        //    };

        //    return await AddPoints(transaction);
        //}

        public async Task<PointTransaction?> AddPoints(PointTransaction transaction)
        {
            using var db = dbFactory.CreateDbContext();

            try
            {
                db.Attach(transaction);

                await db.PointTransactions.AddAsync(transaction);
                await db.SaveChangesAsync();

                await log.Create(new LogRecord()
                {
                    User = transaction.Player.User,
                    Message = $"Awarded {transaction.Value} points for {transaction.Source}",
                    Timestamp = DateTime.Now,
                    Type = LogRecord.RecordType.PointTransaction
                });

                return transaction;
            }
            catch(Exception e)
            {
                Console.WriteLine($"DB Error: {e.Message}");
                return default;
            }
        }
    }
}
