using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class ParcelDeliveryDto
    {
        [Required]
        public Guid BillOfLadingId { get; set; }

        [Required]
        public long SellerId { get; set; }

        [Display(Name = "تحویل دهنده")]
        public string SenderUserName { get; set; }
        public string? WayBillNumber { get; set; }

        [Required(ErrorMessage = "نام تحویل‌گیرنده الزامی است")]
        [Display(Name = "تحویل‌گیرنده")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "شماره همراه الزامی است")]
        [Phone]
        [Display(Name = "شماره همراه")]
        public string ReceiverMobile { get; set; }

        // [Required(ErrorMessage = "کد ملی الزامی است")]
        [Display(Name = "کد ملی")]
        public string? ReceiverNationalCode { get; set; }

        [Display(Name = "زمان تحویل")]
        public DateTime DeliveryDateTime { get; set; } = DateTime.Now;

        [Display(Name = "امضای دیجیتال")]
        public string? SignatureImage { get; set; } // Base64 string from client

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
        public string? UserId { get; set; }
    }
}
