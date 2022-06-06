using ScavHunt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ScavHunt.Data.Services
{
    public class LogAdminService : LogService
    {
        public LogAdminService(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<LogRecord>> All()
        {
            return await db.Log.Include(l => l.Player).Include(l => l.Question).ToListAsync();
        }
    }
}
