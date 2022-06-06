using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class QuestionService
    {
        protected ApplicationDbContext db;

        public QuestionService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<Question?> Get(long id)
        {
            return await db.Questions.FindAsync(id);
        }

        public async Task<Question?> GetFromShortCode(string code)
        {
            return await db.Questions.Where(q => q.ShortCode == code).FirstOrDefaultAsync();
        }
    }
}
