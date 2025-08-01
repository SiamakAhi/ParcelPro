using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_BranchMap : IEntityTypeConfiguration<Cu_Branch>
    {
        public void Configure(EntityTypeBuilder<Cu_Branch> builder)
        {
            builder.HasKey(k => k.Id);


            builder.HasOne(n => n.BranchCity)
               .WithMany(n => n.CityBranches)
               .HasForeignKey(f => f.CityId).OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(n => n.TafsilAccount)
            //  .WithMany(n => n.Branches)
            //  .HasForeignKey(f => f.TafsilId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.BranchHub)
             .WithMany(n => n.Branches)
             .HasForeignKey(f => f.HubId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.BranchPerson)
             .WithMany(n => n.Branches)
             .HasForeignKey(f => f.PartyId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
