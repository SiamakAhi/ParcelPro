using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class CourierDto
    {
        [Display(Name = "نام پیک")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [StringLength(100, ErrorMessage = "حداکثر طول {0} نباید بیش از {1} کاراکتر باشد.")]
        public string FullName { get; set; }

        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Phone(ErrorMessage = "فرمت {0} نامعتبر است.")]
        public string Mobile { get; set; }

        [Display(Name = "کد پرسنلی")]
        [StringLength(50, ErrorMessage = "حداکثر طول {0} نباید بیش از {1} کاراکتر باشد.")]
        public string? PersonnelCode { get; set; }

        [Display(Name = "موتورسوار است؟")]
        public bool IsMotorcycle { get; set; }

        [Display(Name = "فعال است؟")]
        public bool IsActive { get; set; } = true;
    }

}
