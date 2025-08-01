using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Models
{
    public class SmsLog
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "شناسه فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "شماره موبایل گیرنده")]
        [Required(ErrorMessage = "لطفا شماره موبایل گیرنده را وارد کنید")]
        public string RecipientNumber { get; set; }

        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا متن پیام را وارد کنید")]
        public string MessageContent { get; set; }

        [Display(Name = "شناسه قالب پیام")]
        public int? TemplateId { get; set; }

        [Display(Name = "وضعیت ارسال")]
        public Int16 Status { get; set; }

        [Display(Name = "شناسه پیام در سرویس")]
        public string? ProviderMessageId { get; set; }

        [Display(Name = "خطای ارسال")]
        public string? ErrorMessage { get; set; }

        [Display(Name = "سرویس دهنده")]
        public string? ProviderName { get; set; }

        [Display(Name = "تاریخ ارسال")]
        public DateTime SentAt { get; set; } = DateTime.Now;

        [Display(Name = "تاریخ دریافت")]
        public DateTime? DeliveredAt { get; set; }
    }
}
