using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeminarHub.Data.DataModels;

namespace SeminarHub.Data.Configurations
{
    public class SeminarConfiguration : IEntityTypeConfiguration<Seminar>
    {
        public void Configure(EntityTypeBuilder<Seminar> builder)
        {
            builder.HasOne(s => s.Category)
                .WithMany(c => c.Seminars)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
