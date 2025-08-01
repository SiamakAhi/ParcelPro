using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class CargoManifestDto
    {
        [Display(Name = "تاریخ ایجاد")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ رهسپاری")]
        public DateTime? TransportDate { get; set; }

        [Display(Name = "نوع رهسپاری")]
        [Range(1, int.MaxValue, ErrorMessage = "لطفاً یک {0} معتبر انتخاب کنید.")]
        public int TransportType { get; set; }

        [Display(Name = "هاب مبدا")]
        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        public Guid OriginHubId { get; set; }

        [Display(Name = "هاب مقصد")]
        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        public Guid DestinationHubId { get; set; }

        [Display(Name = "تعداد بسته‌ها")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} نمی‌تواند منفی باشد.")]
        public int TotalPackages { get; set; }

        [Display(Name = "وزن کل")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} نمی‌تواند منفی باشد.")]
        public decimal TotalWeight { get; set; }

        [Display(Name = "تعداد بارنامه‌ها")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} نمی‌تواند منفی باشد.")]
        public int NumberOfWaybills { get; set; }

        [Display(Name = "کد خودروی حمل")]
        public int? VehicleId { get; set; }

        [Display(Name = "کد راننده")]
        public int? DriverId { get; set; }

        [Display(Name = "شرکت هواپیمایی")]
        [StringLength(200, ErrorMessage = "حداکثر طول {0} نباید بیش از {1} کاراکتر باشد.")]
        public string? AirlineCompany { get; set; }

        [Display(Name = "شماره بارنامه هوایی")]
        [StringLength(100, ErrorMessage = "حداکثر طول {0} نباید بیش از {1} کاراکتر باشد.")]
        public string? AirWaybillNumber { get; set; }

        [Display(Name = "مبلغ بارنامه هوایی")]
        [Range(0, long.MaxValue, ErrorMessage = "{0} نمی‌تواند منفی باشد.")]
        public long? AirWaybillPrice { get; set; }

        [Display(Name = "تحویل‌دهنده")]
        [StringLength(150, ErrorMessage = "حداکثر طول {0} نباید بیش از {1} کاراکتر باشد.")]
        public string? TahvilDahandeh { get; set; }

        [Display(Name = "تحویل‌گیرنده")]
        [StringLength(150, ErrorMessage = "حداکثر طول {0} نباید بیش از {1} کاراکتر باشد.")]
        public string? ReceiverName { get; set; }

        [Display(Name = "تاریخ رسیدن به هاب مقصد")]
        public DateTime? ArrivalTime { get; set; }
    }

}
