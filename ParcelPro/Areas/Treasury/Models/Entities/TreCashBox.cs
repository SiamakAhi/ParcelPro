using ParcelPro.Areas.Accounting.Models.Entities;
using ParcelPro.Areas.Courier.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Models.Entities
{
    public class TreCashBox
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public long SellerId { get; set; }

        [Display(Name = "شناسه شعبه")]
        public Guid? BranchId { get; set; }

        [Required(ErrorMessage = "نام صندوق الزامی است")]
        [Display(Name = "نام صندوق")]
        public string RegisterName { get; set; }

        [Display(Name = "محل فیزیکی صندوق")]
        public string? PhysicalLocation { get; set; }

        [Display(Name = "شناسه حساب معین")]
        public int? AccountId { get; set; }

        [Display(Name = "شناسه تفصیلی صندوق")]
        public long? DetailedAccountId { get; set; }

        [Required(ErrorMessage = "تاریخ افتتاح صندوق الزامی است")]
        [Display(Name = "تاریخ افتتاح صندوق")]
        public DateTime OpeningDate { get; set; }



        [Display(Name = "ارز")]
        public int? CurrencyId { get; set; }
        public virtual TreCurrency Currency { get; set; }
        [Display(Name = "نرخ")]
        public long CurrencyRate { get; set; } = 1;

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string CreatorUserName { get; set; }

        public virtual Cu_Branch? Branch { get; set; }
        public virtual Acc_Coding_Tafsil? Tafsil { get; set; }
        public virtual ICollection<TreCashier>? Cashiers { get; set; }


    }
}
