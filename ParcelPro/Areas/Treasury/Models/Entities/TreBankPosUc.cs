using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Models.Entities
{
    public class TreBankPosUc
    {
        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شناسه شعبه")]
        public Guid? BranchId { get; set; }

        [Required(ErrorMessage = "نام الزامی است")]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Required(ErrorMessage = "حساب بانکی را مشخص کنید")]
        [Display(Name = "حساب بانک")]
        public int BankAccountId { get; set; }

        [Required(ErrorMessage = "شماره ترمینال وارد نشده")]
        [Display(Name = "شماره ترمینال")]
        public string TerminalNumber { get; set; }

        [Display(Name = "تفصیل")]
        public long? TafsilId { get; set; }

        [Display(Name = "ارز")]
        public int? CurrencyId { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;

        public virtual kh_BankAccount BankAccount { get; set; }
        public virtual TreCurrency? Currency { get; set; }
        public virtual ICollection<TreTransaction>? Transactions { get; set; }

    }
}
