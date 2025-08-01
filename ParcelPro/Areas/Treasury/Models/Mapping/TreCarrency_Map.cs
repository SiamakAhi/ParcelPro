using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Treasury.Models.Entities;

namespace ParcelPro.Areas.Treasury.Models.Mapping
{
    public class TreCarrency_Map : IEntityTypeConfiguration<TreCurrency>
    {
        public void Configure(EntityTypeBuilder<TreCurrency> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasData(
                new TreCurrency { Id = 1, SellerId = 120, FullName = "ریال ایران", ShortName = "IRR", ExchangeRateToRial = 1 },
                new TreCurrency { Id = 2, SellerId = 120, FullName = "دلار آمریکا", ShortName = "USD", ExchangeRateToRial = 770000 },
                new TreCurrency { Id = 3, SellerId = 120, FullName = "یورو", ShortName = "EUR", ExchangeRateToRial = 810000 },
                new TreCurrency { Id = 4, SellerId = 120, FullName = "پوند انگلیس", ShortName = "GBP", ExchangeRateToRial = 920000 },
                new TreCurrency { Id = 5, SellerId = 120, FullName = "یوآن چین", ShortName = "CNY", ExchangeRateToRial = 6500 },
                new TreCurrency { Id = 6, SellerId = 120, FullName = "درهم امارات", ShortName = "AED", ExchangeRateToRial = 11400 },
                new TreCurrency { Id = 7, SellerId = 120, FullName = "لیره ترکیه", ShortName = "TRY", ExchangeRateToRial = 6000 },
                new TreCurrency { Id = 8, SellerId = 120, FullName = "دینار عراق", ShortName = "IQD", ExchangeRateToRial = 35 },
                new TreCurrency { Id = 9, SellerId = 120, FullName = "ریال قطر", ShortName = "QAR", ExchangeRateToRial = 11500 },
                new TreCurrency { Id = 10, SellerId = 120, FullName = "ریال سعودی", ShortName = "SAR", ExchangeRateToRial = 11200 },
                new TreCurrency { Id = 11, SellerId = 120, FullName = "دینار کویت", ShortName = "KWD", ExchangeRateToRial = 140000 },
                new TreCurrency { Id = 12, SellerId = 120, FullName = "ین ژاپن", ShortName = "JPY", ExchangeRateToRial = 380 }
                // ارزهای دیگر می‌توانند به این لیست اضافه شوند
            );
        }
    }
}
