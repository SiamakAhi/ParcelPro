using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto.FinancialDtos
{
    public class CreditClientInfoDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "فروشنده نامشخص است.")]
        public Int64? SellerId { get; set; }


        [Display(Name = "نام شخص حقیقی یا حقوغی *")]
        [Required(ErrorMessage = "نوشتن نام الزامیست")]
        public string Name { get; set; }

        [Display(Name = "شناسه/کد ملی *")]
        public string? NationalId { get; set; }

        [Display(Name = "کد اقتصادی *")]
        public string? EconomicCode { get; set; }


        [Display(Name = "نشانی")]
        public string? Address { get; set; }

        [Display(Name = "کد پستی 10 رقمی *")]
        public string? PostalCode { get; set; }

        [Display(Name = " موبایل")]
        public string? MobilePhone { get; set; }

        [Display(Name = "نمابر")]
        public string? Fax { get; set; }


        public bool IsCreditCustomer { get; set; } = false;

        [Display(Name = "سقف اعتبار مشتری")]
        public long CreditCus_CreditAmount { get; set; } = 0;
        [Display(Name = "شماره همراه واحد مالی حهت ارسال پیام")]
        public string? CreditCus_Mobile { get; set; }

        [Display(Name = "آدرس ایمیل معتبر واحد مالی جهت ارسال اطلاعات")]
        public string? CreditCus_Email { get; set; }

    }
}
