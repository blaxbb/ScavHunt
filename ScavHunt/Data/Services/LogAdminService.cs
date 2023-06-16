using ScavHunt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ScavHunt.Data.Services
{
    public class LogAdminService : LogService
    {
        public LogAdminService(IDbContextFactory<ApplicationDbContext> factory) : base(factory)
        {

        }

        public async Task<List<LogRecord>> All()
        {
            using var db = dbFactory.CreateDbContext();
            return await db.Log.Include(l => l.User).Include(l => l.Question).ToListAsync();
        }
    }
}
