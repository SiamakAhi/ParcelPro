using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class TreCashier_Config : IEntityTypeConfiguration<TreCashier>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TreCashier> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.CashBox)
                .WithMany(n => n.Cashiers)
                .HasForeignKey(p => p.CachboxId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(p => p.Person)
                .WithMany(n => n.Cashiers)
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
