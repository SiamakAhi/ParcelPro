using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_Courier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public long SellerId { get; set; }

        [Required]
        [Display(Name = "شعبه")]
        public Guid BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Cu_Branch Branch { get; set; }

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

        [Display(Name = "عکس")]
        public string? CourierPhoto { get; set; }

        public int? MoeinId { get; set; }
        public long? TafsilId { get; set; }

        [Display(Name = "وضیت فعال")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string CreatedBy { get; set; }
    }
}
