using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class AlertAdminService : AlertService
    {
        public AlertAdminService(IDbContextFactory<ApplicationDbContext> dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Alert>> All()
        {
            var db = await dbFactory.CreateDbContextAsync();
            return await db.Alerts.ToListAsync();
        }

        public async Task New(Alert alert)
        {
            var db = await dbFactory.CreateDbContextAsync();

            db.Alerts.Add(alert);
            await db.SaveChangesAsync();
        }

        public async Task Update(Alert alert)
        {
            var db = await dbFactory.CreateDbContextAsync();

            var existing = db.Alerts.FirstOrDefault(a => a.Id == alert.Id);
            if(existing == null)
            {
                Console.WriteLine($"ERROR: Failed to update existing alert id = {alert.Id}");
                return;
            }

            existing.Title = alert.Title;
            existing.Message = alert.Message;
            existing.Type = alert.Type;
            existing.StartTime = alert.StartTime;
            existing.EndTime = alert.EndTime;

            db.Alerts.Update(existing);
            await db.SaveChangesAsync();
        }
    }
}
