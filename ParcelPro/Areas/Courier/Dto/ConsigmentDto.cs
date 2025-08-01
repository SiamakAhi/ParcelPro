using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class ConsigmentDto
    {
        public Guid Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "کد")]
        public string Code { get; set; }

        [Display(Name = "وزن")]
        [Required(ErrorMessage = "فیلد وزن الزامی است")]
        public float Weight { get; set; }

        [Display(Name = "ارتفاع")]
        public float Height { get; set; }

        [Display(Name = "عرض")]
        public float Width { get; set; }

        [Display(Name = "طول")]
        public float Length { get; set; }

        [Display(Name = "حجم")]
        public float Volume { get; set; }

        [Display(Name = "محتوا")]
        public string? ContentDescription { get; set; }

        [Display(Name = "ارزش مرسوله")]
        [Required(ErrorMessage = "فیلد ارزش مرسوله الزامی است")]
        public long Value { get; set; } = 0;
        public string strValue { get; set; } = "0";

        [Display(Name = "اطلاعات خدماتی")]
        public string? ServiceInformation { get; set; }

        [Display(Name = "تحویل گرفته شده است؟")]
        public bool IsDelivered { get; set; } = false;

        [Display(Name = "نام تحویل گیرنده")]
        public string? RecipientName { get; set; }

        [Display(Name = "تاریخ تحویل به گیرنده")]
        public DateTime? DeliveryDate { get; set; }

        [Display(Name = "امضای الکترونیکی تحویل‌گیرنده")]
        public byte[]? ReceiverSignature { get; set; }

        [Display(Name = "مبلغ رهسپاری")]
        public long CargoFare { get; set; } = 0;
        public string strCargoFare { get; set; } = "0";

        [Display(Name = "هزینه بسته بندی")]
        public long PackagingCost { get; set; } = 0;
        public string strPackagingCost { get; set; } = "0";

        [Display(Name = "هزینه بیمه")]
        public long InsuranceCost { get; set; } = 0;
        public string strInsuranceCost { get; set; } = "0";

        [Display(Name = "سایر هزینه ها")]
        public long OtherCost { get; set; } = 0;
        public string strOtherCost { get; set; } = "0";

        [Display(Name = "تخفیف")]
        public long Discount { get; set; } = 0;
        public string strDiscount { get; set; } = "0";

        [Display(Name = "نرخ ارزش افزوده")]
        public float VatRate { get; set; } = 0;

        [Display(Name = "مبلغ ارزش افزوده")]
        public long VatPrice { get; set; } = 0;
        public string strVatPrice { get; set; } = "0";

        [Display(Name = "جمع هزینه های ")]
        public long TotalCostPrice { get; set; }
        public string strTotalCostPrice { get; set; } = "0";

        [Display(Name = "مبغ نهایی مرسوله")]
        public long TotalPrice { get; set; } = 0;
        public string strTotalPrice { get; set; } = "0";

        [Display(Name = "شناسه بارنامه")]
        [Required(ErrorMessage = "بارنامه مربوطه شناسایی نشد")]
        public Guid BillOfLadingId { get; set; }

        [Display(Name = " ماهیت مرسوله")]
        [Required(ErrorMessage = "تعیین ماهیت مرسوله الزامی است")]
        public short NatureTypeId { get; set; }
        public string? NatureName { get; set; }

        [Display(Name = "نوع بسته بندی")]
        public long? PackageTypeId { get; set; }
        public string? Package { get; set; } = string.Empty;

        public string CreatedBy { get; set; }

        public List<AddParcelCostDto>? Costs { get; set; } = new List<AddParcelCostDto>();

        public DateTime? CreateAt { get; set; }
        public TimeSpan? CreateAtTime { get; set; }

        public string? UserId { get; set; }
        public string? BillOfLadingNumber { get; set; }

    }
}
