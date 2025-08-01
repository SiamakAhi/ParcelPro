using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_SaleContract_Config : IEntityTypeConfiguration<Cu_SaleContract>
    {
        public void Configure(EntityTypeBuilder<Cu_SaleContract> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(n => n.ContractParty)
                .WithMany(n => n.Clients)
                .HasForeignKey(n => n.PartyId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
