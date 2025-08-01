using ParcelPro.Areas.Warehouse.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ParcelPro.Areas.Warehouse.Models.Mapping
{
    public class Wh_WarehouseMapp : IEntityTypeConfiguration<Wh_Warehouse>
    {
        public void Configure(EntityTypeBuilder<Wh_Warehouse> builder)
        {
            builder.HasKey(k => k.WarehouseId);

            builder.HasOne(n => n.WarehouseBranch)
                .WithMany(n => n.Warehouses)
                .HasForeignKey(n => n.BranchId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
