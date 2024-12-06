using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.Configurations;
using SeminarHub.Data.DataModels;

namespace SeminarHub.Data
{
    public class SeminarHubDbContext : IdentityDbContext
    {
        public SeminarHubDbContext(DbContextOptions<SeminarHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Seminar> Seminars { get; set; } = null!;

        public DbSet<SeminarParticipant> SeminarsParticipants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new SeminarConfiguration());
            builder.ApplyConfiguration(new SeminarParticipantConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
