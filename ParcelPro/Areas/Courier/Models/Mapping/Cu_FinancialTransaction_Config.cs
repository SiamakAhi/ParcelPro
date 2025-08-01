using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_FinancialTransaction_Config : IEntityTypeConfiguration<Cu_FinancialTransaction>
    {
        public void Configure(EntityTypeBuilder<Cu_FinancialTransaction> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(d => d.BillOfLading)
                .WithMany(d => d.FinancialTransactions)
                .HasForeignKey(f => f.BillOfLadingId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Party)
               .WithMany(d => d.FinancialTransactions)
               .HasForeignKey(f => f.AccountPartyId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Branch)
               .WithMany(d => d.FinancialTransactions)
               .HasForeignKey(f => f.BranchId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.TransactionOperation)
               .WithMany(d => d.Transactions)
               .HasForeignKey(f => f.OperationId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.User)
               .WithMany(d => d.CuTransactions)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
