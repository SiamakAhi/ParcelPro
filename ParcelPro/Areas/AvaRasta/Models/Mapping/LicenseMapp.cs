using ParcelPro.Areas.AvaRasta.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ParcelPro.Areas.AvaRasta.Models.Mapping
{
    public class LicenseMapp : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.Customer)
                  .WithMany(n => n.Licenses)
                  .HasForeignKey(n => n.CustomerId);

            builder.HasOne(n => n.Module)
                  .WithMany(n => n.Licenses)
                  .HasForeignKey(n => n.ModuleId);
        }
    }
}
