using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeminarHub.Data.DataModels;

namespace SeminarHub.Data.Configurations
{
    public class SeminarParticipantConfiguration : IEntityTypeConfiguration<SeminarParticipant>
    {
        public void Configure(EntityTypeBuilder<SeminarParticipant> builder)
        {
            builder.HasKey(sp => new
            {
                sp.SeminarId,
                sp.ParticipantId
            });

            builder.HasOne(sp => sp.Seminar)
                .WithMany(s => s.SeminarsParticipants)
                .HasForeignKey(sp => sp.SeminarId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
