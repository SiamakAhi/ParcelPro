using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_Courier_Config : IEntityTypeConfiguration<Cu_Courier>
    {
        public void Configure(EntityTypeBuilder<Cu_Courier> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(n => n.Branch)
                .WithMany(n => n.BranchCouriers)
                .HasForeignKey(n => n.BranchId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
