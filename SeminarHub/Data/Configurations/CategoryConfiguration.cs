using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeminarHub.Data.DataModels;

namespace SeminarHub.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category()
            {
                Id = 1,
                Name = "Technology & Innovation"
            },
            new Category()
            {
                Id = 2,
                Name = "Business & Entrepreneurship"
            },
            new Category()
            {
                Id = 3,
                Name = "Science & Research"
            },
            new Category()
            {
                Id = 4,
                Name = "Arts & Culture"
            },
            new Category()
            {
                Id = 5,
                Name = "Economics & Politics"
            });
        }
    }
}
