using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto.ContractDto
{
    public class SaleContractDto
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "شناسه فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "عنوان قرارداد")]
        public string Title { get; set; }

        [Display(Name = "شناسه طرف قرارداد")]
        public long PartyId { get; set; }

        [Display(Name = "طرف قرارداد")]
        public string? PartyName { get; set; }

        [Display(Name = "تاریخ شروع")]
        public DateTime StartDate { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "وضعیت فعال بودن حساب")]
        public bool AccounIstActive { get; set; } = true;

        [Display(Name = "کلید API")]
        public string? ApiKey { get; set; }
    }
}
