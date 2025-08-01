using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class TreCheckBook_Config : IEntityTypeConfiguration<TreCheckbook>
    {
        public void Configure(EntityTypeBuilder<TreCheckbook> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.BankAccount)
                .WithMany(n => n.Checkbooks)
                .HasForeignKey(p => p.BankAccountId);
        }
    }
}
