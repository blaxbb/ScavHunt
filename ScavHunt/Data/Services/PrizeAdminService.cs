using Microsoft.EntityFrameworkCore;
using ScavHunt.Data.Models;

namespace ScavHunt.Data.Services
{
    public class PrizeAdminService
    {
        protected IDbContextFactory<ApplicationDbContext> dbFactory;
        public PrizeAdminService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<List<Prize>> All()
        {
            var db = await dbFactory.CreateDbContextAsync();
            return await db.Prizes.ToListAsync();
        }

        public async Task<Prize> New(Prize prize)
        {
            var db = await dbFactory.CreateDbContextAsync();
            var updated = db.Prizes.Add(prize);
            await db.SaveChangesAsync();

            return updated.Entity;
        }

        public async Task<Prize> Update(Prize prize)
        {
            var db = await dbFactory.CreateDbContextAsync();
            var updated = db.Prizes.Update(prize);
            await db.SaveChangesAsync();
            return updated.Entity;
        }
    }
}
