using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_FinancialTransactionOperationConfig : IEntityTypeConfiguration<Cu_FinancialTransactionOperation>
    {
        public void Configure(EntityTypeBuilder<Cu_FinancialTransactionOperation> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasData(
                new Cu_FinancialTransactionOperation { Id = 1, Code = 1, Name = "فروش بار" },
                new Cu_FinancialTransactionOperation { Id = 2, Code = 2, Name = "فروش خدمات توزیع بار" },
                new Cu_FinancialTransactionOperation { Id = 3, Code = 3, Name = "پورسانت فروش" },
                new Cu_FinancialTransactionOperation { Id = 4, Code = 4, Name = "دریافت خدمات توزیع و جمع آوری بار" }
                );
        }
    }
}
