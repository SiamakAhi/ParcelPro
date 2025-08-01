using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto.ContractDto
{
    public class AddClientUserDto
    {
        public int Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "قرارداد مربوطه را انتحاب کنید")]
        [Required(ErrorMessage = " انتخاب کاربر الزامی است")]
        public int ContractId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "نام کاربر را بنویسید")]
        public string Name { get; set; }

        [Display(Name = "نام خاودگی")]
        [Required(ErrorMessage = "نام خانوادگی را بنویسی")]
        public string Family { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "جنسیت را مشخص کنید")]
        public short Gender { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل معتبر وارد کنید")]
        public string? Email { get; set; }

        [Display(Name = "شماره همراه")]
        [Required(ErrorMessage = "شماره همراه معتبر وارد کنید")]
        public string? Mobile { get; set; }

        public string Role { get; set; } = "MainUser";
        public DateTime CreateAt { get; set; } = new DateTime();

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری را بنویسید")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "کلمه عبور را بنویسید")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public short DepartmentCode { get; set; } = 201;

        public int? CustomerId { get; set; }
        public string[]? IdentityRols { get; set; }

    }
}
