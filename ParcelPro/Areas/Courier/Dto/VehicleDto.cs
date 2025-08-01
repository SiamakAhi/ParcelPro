using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public long? SellerId { get; set; }

        [Display(Name = "شماره پلاک")]
        [Required(ErrorMessage = "شماره پلاک الزامی است.")]
        [MaxLength(15)]
        public string PlateNumber { get; set; }

        [Display(Name = "نوع وسیله نقلیه")]
        [Required(ErrorMessage = "نوع وسیله نقلیه الزامی است.")]
        [MaxLength(50)]
        public string VehicleType { get; set; }

        [Display(Name = "ظرفیت حمل (کیلوگرم)")]
        [Required(ErrorMessage = "ظرفیت حمل الزامی است.")]
        public int LoadCapacityKg { get; set; } = 0;

        [Display(Name = "مدل")]
        [MaxLength(50)]
        public string? Model { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(500)]
        public string? Description { get; set; }

        public int? MoeinId { get; set; }
        public long? TafsilId { get; set; }


        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
    }

}
