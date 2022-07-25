using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class AlertService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;

        public AlertService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<List<Alert>> ActiveAlerts()
        {
            var db = await dbFactory.CreateDbContextAsync();
            var all = await db.Alerts.ToListAsync();
            return all.Where(a => a.IsActive()).ToList();
        }
    }
}
