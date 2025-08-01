using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Models.Identity;

namespace ParcelPro.Models.Mapping
{
    public class AppIdentityUser_Map : IEntityTypeConfiguration<AppIdentityUser>
    {
        public void Configure(EntityTypeBuilder<AppIdentityUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(n => n.Customer)
                .WithMany(n => n.CustomerUsers)
                .HasForeignKey(x => x.CustomerId);


        }
    }
}
