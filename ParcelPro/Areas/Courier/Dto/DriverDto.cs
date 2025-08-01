using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class DriverDto
    {
        public int Id { get; set; }

        [Required]
        public long SellerId { get; set; }

        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "نام کامل الزامی است.")]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "شماره تماس الزامی است.")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Display(Name = "کد ملی")]
        [MaxLength(10)]
        public string? NationalCode { get; set; }

        [Display(Name = "شماره گواهینامه")]
        [Required(ErrorMessage = "شماره گواهینامه الزامی است.")]
        [MaxLength(20)]
        public string LicenseNumber { get; set; }

        [Display(Name = "تاریخ اعتبار گواهینامه")]
        [Required(ErrorMessage = "تاریخ اعتبار گواهینامه الزامی است.")]
        public DateTime LicenseExpiryDate { get; set; }

        [Display(Name = "عکس راننده")]
        public string? DriverPhoto { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string CreatedBy { get; set; }

        public int? MoeinId { get; set; } = null;
        public long? TafsilId { get; set; } = null;
    }

}
