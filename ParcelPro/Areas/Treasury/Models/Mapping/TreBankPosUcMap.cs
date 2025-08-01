using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class TreBankPosUcMap : IEntityTypeConfiguration<TreBankPosUc>
    {
        public void Configure(EntityTypeBuilder<TreBankPosUc> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(n => n.BankAccount)
                .WithMany(n => n.Poses)
                .HasForeignKey(f => f.BankAccountId);

            builder.HasOne(n => n.Currency)
                .WithMany(n => n.Poses)
                .HasForeignKey(f => f.CurrencyId);

        }
    }
}
