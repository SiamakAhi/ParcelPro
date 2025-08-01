using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class Kh_BankAccount_Map : IEntityTypeConfiguration<kh_BankAccount>
    {
        public void Configure(EntityTypeBuilder<kh_BankAccount> builder)
        {
            builder.HasKey("Id");

            builder.HasOne(n => n.Bank)
                .WithMany(n => n.BankAccounts)
                .HasForeignKey(f => f.BankId);

            builder.HasOne(n => n.Moein)
              .WithMany(n => n.BankAccounts)
              .HasForeignKey(f => f.MoeinId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
