using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_Consignment_Config : IEntityTypeConfiguration<Cu_Consignment>
    {
        public void Configure(EntityTypeBuilder<Cu_Consignment> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.NatureType)
                .WithMany(n => n.Consignments)
                .HasForeignKey(n => n.NatureTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.BillOfLading)
             .WithMany(n => n.Consignments)
             .HasForeignKey(n => n.BillOfLadingId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Manifest)
             .WithMany(n => n.Parcels)
             .HasForeignKey(n => n.ManifestId)
             .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
