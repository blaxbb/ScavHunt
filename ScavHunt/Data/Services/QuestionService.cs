using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class QuestionService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;

        public QuestionService(IDbContextFactory<ApplicationDbContext> factory)
        {
            dbFactory = factory;
        }

        public async Task<Question?> Get(long id)
        {
            using var db = dbFactory.CreateDbContext();
            return await db.Questions.FindAsync(id);
        }

        public async Task<Question?> GetFromShortCode(string code)
        {
            using var db = dbFactory.CreateDbContext();
            return await db.Questions.Where(q => q.ShortCode == code).FirstOrDefaultAsync();
        }

        public async Task<List<Question>> Active()
        {
            using var db = dbFactory.CreateDbContext();
            var res = await db.Questions.Include(q => q.ParentQuestion).ToListAsync();

            return res.Where(q => !q.IsCurrentlyLockedTime()).ToList();
        }

        async Task IsUnlocked(Question q, Player p)
        {
            
        }
    }
}
