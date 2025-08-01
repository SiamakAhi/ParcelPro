using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class CargoManifestHeaderDto
    {

        [Display(Name = "تاریخ ایجاد")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ رهسپاری")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
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

        [Display(Name = "کد خودروی حمل")]
        public int? VehicleId { get; set; }

        [Display(Name = "کد راننده")]
        public int? DriverId { get; set; }



    }
}
