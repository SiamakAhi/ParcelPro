using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class TreCashBox_Config : IEntityTypeConfiguration<TreCashBox>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TreCashBox> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Currency)
                .WithMany(n => n.CashBoxes)
                .HasForeignKey(p => p.CurrencyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Branch)
               .WithMany(n => n.CashBoxes)
               .HasForeignKey(p => p.BranchId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Tafsil)
               .WithMany(n => n.CashBoxes)
               .HasForeignKey(p => p.DetailedAccountId)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
