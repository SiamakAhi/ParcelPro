using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Dto
{
    public class TreBankPosUcDto
    {

        public int Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شعبه")]
        public Guid? BranchId { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "بانک")]
        public int BankAccountId { get; set; }

        [Display(Name = "شماره ترمینال")]
        public string TerminalNumber { get; set; }

        [Display(Name = "حساب تفصیل")]
        public long? TafsilId { get; set; }

        [Display(Name = "ارز")]
        public int? CurrencyId { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }

        [Display(Name = "حساب بانک")]
        public string? BankAccountName { get; set; }
        [Display(Name = "ارز")]
        public string? CurrencyName { get; set; }
    }

}
