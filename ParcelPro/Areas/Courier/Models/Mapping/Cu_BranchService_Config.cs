using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_BranchService_Config : IEntityTypeConfiguration<Cu_BranchService>
    {
        public void Configure(EntityTypeBuilder<Cu_BranchService> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(n => n.Cu_Service).WithMany(n => n.BranchServices).HasForeignKey(n => n.ServiceId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Branch).WithMany(n => n.BranchServices).HasForeignKey(n => n.BranchId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
