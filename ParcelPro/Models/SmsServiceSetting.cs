using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Models
{
    public class SmsServiceSetting
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "شناسه فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "نام ارائه‌دهنده سرویس")]
        public string ProviderName { get; set; }

        [Display(Name = "آدرس وب‌سایت")]
        public string? WebsiteAddress { get; set; }

        [Display(Name = "اطلاعات تماس پشتیبانی")]
        public string? SupportContact { get; set; }

        [Display(Name = "آدرس API")]
        public string? ApiUrl { get; set; }

        [Display(Name = "متود درخواست")]
        public string? Method { get; set; }

        [Display(Name = "نام کاربری")]
        public string? UserName { get; set; }

        [Display(Name = "رمز عبور")]
        public string? Password { get; set; }
    }
}
