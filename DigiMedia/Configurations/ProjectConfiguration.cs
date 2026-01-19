using DigiMedia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigiMedia.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
            builder.Property(x => x.ImagePath).IsRequired();

            builder.HasOne(x => x.Category).WithMany(x => x.Projects).HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
