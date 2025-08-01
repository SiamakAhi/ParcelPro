using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_ParcelStatusConfig : IEntityTypeConfiguration<Cu_ParcelStatus>
    {
        public void Configure(EntityTypeBuilder<Cu_ParcelStatus> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasData(
                    new Cu_ParcelStatus { Id = 1, StatusCode = "1", Status = "در حال صدور", Message = "مرسوله در حال صدور است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 2, StatusCode = "2", Status = "در انتظار پرداخت", Message = "مرسوله در انتظار پرداخت است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 3, StatusCode = "3", Status = "در انتظار جمع آوری", Message = "مرسوله در انتظار جمع آوری است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 4, StatusCode = "4", Status = "در حال جمع آوری", Message = "مرسوله در حال جمع آوری است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 5, StatusCode = "5", Status = "تأیید ورود به هاب مبدأ", Message = "مرسوله به هاب مبدأ وارد شده است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 6, StatusCode = "6", Status = "در حال ارسال به هاب شهر مقصد", Message = "مرسوله در حال ارسال به هاب شهر مقصد است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 7, StatusCode = "7", Status = "تأیید ورود به هاب شهر مقصد", Message = "مرسوله به هاب شهر مقصد وارد شده است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 8, StatusCode = "8", Status = "آماده توزیع (در انتظار تحویل به سفیر)", Message = "مرسوله آماده توزیع است و منتظر تحویل به سفیر است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 9, StatusCode = "9", Status = "تحویل سفیر جهت تحویل به گیرنده", Message = "مرسوله به سفیر تحویل شده است و در حال ارسال به گیرنده است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 10, StatusCode = "10", Status = "در انتظار پرداخت توسط گیرنده", Message = "مرسوله در انتظار پرداخت توسط گیرنده است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 11, StatusCode = "11", Status = "مرسوله تحویل گیرنده شد", Message = "مرسوله با موفقیت به گیرنده تحویل داده شد.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 12, StatusCode = "12", Status = "برگشت به هاب مقصد", Message = "مرسوله به هاب مقصد برگشت داده شده است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 13, StatusCode = "13", Status = "مفقود شده", Message = "مرسوله مفقود شده است.", ForReciver = false, ForSender = false },
                    new Cu_ParcelStatus { Id = 14, StatusCode = "14", Status = "فاسد شده", Message = "مرسوله فاسد شده است.", ForReciver = false, ForSender = false }
            );
        }
    }
}
