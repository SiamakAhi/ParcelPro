using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_DistributionService_Config : IEntityTypeConfiguration<Cu_DistributionService>
    {
        public void Configure(EntityTypeBuilder<Cu_DistributionService> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(n => n.CuInvoice)
                .WithMany(n => n.PrtnerServices)
                .HasForeignKey(n => n.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
