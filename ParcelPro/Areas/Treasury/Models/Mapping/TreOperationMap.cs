using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class TreOperationMap : IEntityTypeConfiguration<TreOperation>
    {
        public void Configure(EntityTypeBuilder<TreOperation> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OperationType)
                .IsRequired();

            builder.Property(o => o.OperationName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.IsPOSTransaction)
                .HasDefaultValue(false); // Default value: Not a POS transaction

            // Seeding data with HasData
            builder.HasData(
                // Receipt operations
                new TreOperation { Id = 1, OperationType = 1, OperationName = "دریافت وجه نقد", IsPOSTransaction = false, IsPay = false, UserAlowSelect = true },
                new TreOperation { Id = 2, OperationType = 2, OperationName = "واریز با کارتخوان (POS)", IsPOSTransaction = true, IsPay = false, UserAlowSelect = true },
                new TreOperation { Id = 3, OperationType = 3, OperationName = "حواله بانکی", IsPOSTransaction = false, IsPay = false, UserAlowSelect = true },
                new TreOperation { Id = 4, OperationType = 4, OperationName = "دریافت چک", IsPOSTransaction = false, IsPay = false, UserAlowSelect = true },
                new TreOperation { Id = 5, OperationType = 5, OperationName = "تهاتر", IsPOSTransaction = false, IsPay = false, UserAlowSelect = false },
                new TreOperation { Id = 6, OperationType = 6, OperationName = "واریز از طریق درگاه الکترونیک", IsPOSTransaction = false, IsPay = false, UserAlowSelect = false },

                // Payment operations
                new TreOperation { Id = 7, OperationType = 20, OperationName = "پرداخت وجه نقد", IsPOSTransaction = false, IsPay = true, UserAlowSelect = true },
                new TreOperation { Id = 8, OperationType = 21, OperationName = "واریز به حساب", IsPOSTransaction = false, IsPay = true, UserAlowSelect = true },
                new TreOperation { Id = 9, OperationType = 22, OperationName = "حواله", IsPOSTransaction = false, IsPay = true, UserAlowSelect = true },
                new TreOperation { Id = 10, OperationType = 23, OperationName = "پرداخت چک", IsPOSTransaction = false, IsPay = true, UserAlowSelect = true },
                 new TreOperation { Id = 11, OperationType = 24, OperationName = "پرداخت از طریق درگاه الکترونیک", IsPOSTransaction = false, IsPay = true, UserAlowSelect = false },
                  new TreOperation { Id = 12, OperationType = 25, OperationName = "پرداخت با کارتخوان (POS)", IsPOSTransaction = true, IsPay = false, UserAlowSelect = true }
            );
        }
    }
}
