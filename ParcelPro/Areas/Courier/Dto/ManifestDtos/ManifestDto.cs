using ParcelPro.Areas.Courier.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto.ManifestDtos
{
    public class ManifestDto
    {
        public long Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ رهسپاری")]
        public DateTime? TransportDate { get; set; }

        [Required]
        [Display(Name = "نوع رهسپاری")]
        public short TransportType { get; set; }
        // 1- زمینی کیهان پست
        // 2- هوایی
        // 3- ترمینال اتوبوس
        // 4- ریلی


        [Display(Name = "هاب مبدا")]
        public Guid OriginHubId { get; set; }
        public virtual Cu_Hub OriginHub { get; set; }

        [Display(Name = "هاب مقصد")]
        public Guid DestinationHubId { get; set; }
        public virtual Cu_Hub DestinationHub { get; set; }

        [Display(Name = "جمع تعداد بسته‌ها")]
        public int TotalPackages { get; set; } = 0;
        public decimal TotalWeight { get; set; } = 0;

        [Display(Name = "تعداد بارنامه‌ها")]
        public int NumberOfWaybills { get; set; } = 0;

        [Display(Name = " خودروی حمل")]
        public int? VehicleId { get; set; }
        public virtual Cu_Vehicle? Vehicle { get; set; }

        [Display(Name = " راننده حمل")]
        public int? DriverId { get; set; }
        public virtual Cu_Driver? Driver { get; set; }

        [Display(Name = "شماره بارنامه")]
        [StringLength(100)]
        public string? BillOfLadingNumber { get; set; }

        [Display(Name = "تاریخ بارنامه")]
        [DataType(DataType.Date)]
        public DateTime? BillOfLadingDate { get; set; }

        [Display(Name = "شرکت خدمات بار")]
        [StringLength(200)]
        public string? FreightCompany { get; set; }

        [Display(Name = "مبلغ بارنامه")]
        public long FreightCost { get; set; } = 0;

        [Display(Name = "شماره تماس هماهنگی")]
        [StringLength(20)]
        [Phone]
        public string? ContactNumber { get; set; }

        //
        [Required]
        [Display(Name = "وضعیت")]
        public short Status { get; set; } = 1;
        // 1- در حال ثبت
        // 2- در حال رهسپاری
        // 3- بسته شده


        [Display(Name = "تاریخ حرکت")]
        [DataType(DataType.Date)]
        public DateTime? MovementDate { get; set; }

        [Display(Name = "زمان حرکت")]
        [DataType(DataType.Time)]
        public TimeSpan? MovementTime { get; set; }

        [Display(Name = "توضیحات صادر کننده")]
        public string? IssuerDescription { get; set; }


        [Display(Name = "نام تحویل‌دهنده")]
        public string? TahvilDahandeh { get; set; }

        [Display(Name = "نام تحویل‌گیرنده")]
        public string? ReceiverName { get; set; }

        [Display(Name = "تاریخ و زمان رسیدن به هاب مقصد")]
        public DateTime? ArrivalTime { get; set; }
    }
}
