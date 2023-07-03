using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScavHunt.Data.Models;
using System.Reflection.Emit;

namespace ScavHunt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ScavhuntUser>
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public DbSet<LogRecord> Log { get; set; }

        public DbSet<Prize> Prizes { get; set; }
        public DbSet<PrizeTransaction> PrizeTransactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "8a3258f3-4a3e-40c8-997e-e04421c1210e", ConcurrencyStamp = "f312aca4-e4a9-46fd-bba8-a26916eb5b63", Name = "admin", NormalizedName = "ADMIN".ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "4e8d9538-ac48-4fe3-98e9-ed7d8fed9e6b", ConcurrencyStamp = "a2e4c2bc-8954-4f38-a817-e5cb5e27b01a", Name = "prize", NormalizedName = "prize".ToUpper() });

            builder.Entity<Question>()
                .Property(q => q.Answers)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>()
                );

            builder.Entity<ScavhuntUser>()
                .HasIndex(u => u.Id).IncludeProperties(p => new { p.BadgeId });

            builder.Entity<Question>()
                .HasIndex(q => q.ShortCode)
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}