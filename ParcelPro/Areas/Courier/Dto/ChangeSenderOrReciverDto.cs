using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class ChangeSenderOrReciverDto
    {
        [Required(ErrorMessage = "خطایی در ارسال اطلاعات رخ داده است با مدیر سیستم تماس بگیرید")]
        public Guid BillId { get; set; }

        [Required(ErrorMessage = "شخص فرستنده یا گیرنده نا مشخص است")]
        public long PersonId { get; set; }

        public string? WaybillNumber { get; set; }

        [Required(ErrorMessage = "شماره تماس را بدرستی وارد کنید")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "آدرس را بنویسید")]
        public string Address { get; set; }

        public string UserName { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "خطایی در ارسال اطلاعات رخ داده است با مدیر سیستم تماس بگیرید")]
        public bool IsSender { get; set; }
    }
}
