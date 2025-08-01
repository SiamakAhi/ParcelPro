using ParcelPro.Areas.Courier.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_Packaging_Config : IEntityTypeConfiguration<Cu_Packaging>
    {
        public void Configure(EntityTypeBuilder<Cu_Packaging> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(n => n.ProductCategory)
                .WithMany(n => n.PackageList)
                .HasForeignKey(n => n.WarehouseProductCategoryId)
                .OnDelete(DeleteBehavior.NoAction);



            builder.HasData(
                 new Cu_Packaging { Id = 1, SellerId = 3, PackageCode = "80", Name = "پاکت", Price = 0, ForExport = false }


             );

        }
    }
}
