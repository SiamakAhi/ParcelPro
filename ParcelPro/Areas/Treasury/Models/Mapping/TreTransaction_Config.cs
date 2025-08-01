using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class TreTransaction_Config : IEntityTypeConfiguration<TreTransaction>
    {
        public void Configure(EntityTypeBuilder<TreTransaction> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(d => d.BillOfLading)
               .WithMany(d => d.TreTransactions)
               .HasForeignKey(f => f.BillOfLadingId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Party)
               .WithMany(d => d.TreTransactions)
               .HasForeignKey(f => f.AccountPartyId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Operation)
               .WithMany(d => d.TreTransactions)
               .HasForeignKey(f => f.OperationId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.User)
               .WithMany(d => d.TreTransactions)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.BillFinancialTransaction)
              .WithMany(d => d.MoneyTransactions)
              .HasForeignKey(f => f.BillFinancialtransactionId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.BankAccount)
             .WithMany(d => d.Transactions)
             .HasForeignKey(f => f.BankAccountId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Pos)
            .WithMany(d => d.Transactions)
            .HasForeignKey(f => f.PosId)
            .OnDelete(DeleteBehavior.NoAction);

        }

    }
}
