using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_CargoManifest_Config : IEntityTypeConfiguration<Cu_CargoManifest>
    {
        public void Configure(EntityTypeBuilder<Cu_CargoManifest> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasOne(n => n.DestinationHub)
            .WithMany(n => n.ManifestsIn)
            .HasForeignKey(n => n.DestinationHubId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.OriginHub)
           .WithMany(n => n.ManifestsOut)
           .HasForeignKey(n => n.OriginHubId)
           .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Driver)
               .WithMany(n => n.Manifests)
               .HasForeignKey(n => n.DriverId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Vehicle)
             .WithMany(n => n.Manifests)
             .HasForeignKey(n => n.VehicleId)
             .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
