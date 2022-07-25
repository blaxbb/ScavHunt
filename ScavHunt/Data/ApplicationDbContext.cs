using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScavHunt.Data.Models;

namespace ScavHunt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public DbSet<LogRecord> Log { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Question>()
                .Property(q => q.Answers)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>()
                );

            builder.Entity<Question>()
                .HasIndex(q => q.ShortCode)
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}