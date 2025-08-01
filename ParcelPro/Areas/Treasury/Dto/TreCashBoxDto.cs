using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Dto
{
    public class TreCashBoxDto
    {
        public Guid Id { get; set; }

        [Required]
        public long SellerId { get; set; }

        [Display(Name = "شناسه شعبه")]
        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }

        [Required(ErrorMessage = "نام صندوق الزامی است")]
        [Display(Name = "نام صندوق")]
        public string RegisterName { get; set; }

        [Display(Name = "محل فیزیکی صندوق")]
        public string? PhysicalLocation { get; set; }

        [Display(Name = "شناسه حساب معین")]
        public int? AccountId { get; set; }

        [Display(Name = "شناسه تفصیلی صندوق")]
        public long? DetailedAccountId { get; set; }
        public string? TafsilName { get; set; }
        public string? TafsilCode { get; set; }

        [Required(ErrorMessage = "تاریخ افتتاح صندوق الزامی است")]
        [Display(Name = "تاریخ افتتاح صندوق")]
        public DateTime OpeningDate { get; set; } = DateTime.Now;

        [Display(Name = "تاریخ افتتاح صندوق")]
        public string? strOpeningDate { get; set; }



        [Display(Name = "ارز")]
        public int? CurrencyId { get; set; }
        [Display(Name = "ارز")]
        public string? CurrencyName { get; set; }

        [Display(Name = "نرخ")]
        public long CurrencyRate { get; set; } = 1;

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string CreatorUserName { get; set; }
    }
}
