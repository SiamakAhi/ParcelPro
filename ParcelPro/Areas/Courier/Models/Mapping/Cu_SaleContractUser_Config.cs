using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_SaleContractUser_Config : IEntityTypeConfiguration<Cu_SaleContractUser>
    {
        public void Configure(EntityTypeBuilder<Cu_SaleContractUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(n => n.Contract)
                .WithMany(n => n.ClientUsers)
                .HasForeignKey(n => n.ContractId);

            builder.HasOne(n => n.UserData)
                .WithOne(n => n.ClientUserData)
                .HasForeignKey<Cu_SaleContractUser>(n => n.userId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
