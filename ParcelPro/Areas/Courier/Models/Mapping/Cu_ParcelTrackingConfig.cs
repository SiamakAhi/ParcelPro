using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_ParcelTrackingConfig : IEntityTypeConfiguration<Cu_ParcelTracking>
    {
        public void Configure(EntityTypeBuilder<Cu_ParcelTracking> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.User)
                .WithMany(n => n.Parcels)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Status)
               .WithMany(n => n.Parcels)
               .HasForeignKey(f => f.StatusId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
