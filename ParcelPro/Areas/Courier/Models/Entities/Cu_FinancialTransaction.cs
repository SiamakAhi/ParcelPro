using ParcelPro.Areas.Treasury.Models.Entities;
using ParcelPro.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_FinancialTransaction
    {
        [Key]
        public Guid Id { get; set; }

        public short OperationId { get; set; }

        [Display(Name = "فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "شناسه بارنامه")]
        public Guid? BillOfLadingId { get; set; }

        [Display(Name = "طرف حساب")]
        public long AccountPartyId { get; set; }

        // 1- نقد
        // 2- پسکرایه
        // 3- اعتباری
        [Display(Name = "نوع تسویه حساب")]
        public int SettlementTypeId { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Display(Name = "زمان")]
        public TimeSpan TransactionTime { get; set; } = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        [Display(Name = "شرح تراکنش")]
        public string? Description { get; set; }

        [Display(Name = "شناسه شعبه")]
        public Guid? BranchId { get; set; }

        [Display(Name = "کاربر")]
        public string UserId { get; set; }

        [Display(Name = "مبلغ")]
        public long Amount { get; set; }

        [Display(Name = "بدهکار")]
        public long Bed { get; set; } = 0;

        [Display(Name = "بدهکار")]
        public long Bes { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;

        [Display(Name = "تسویه شده")]
        public bool IsSettled { get; set; } = false;

        public virtual Cu_BillOfLading? BillOfLading { get; set; }
        public virtual Party Party { get; set; }
        public virtual Cu_Branch? Branch { get; set; }
        public virtual Cu_FinancialTransactionOperation TransactionOperation { get; set; }
        public virtual AppIdentityUser User { get; set; }
        public virtual ICollection<TreTransaction>? MoneyTransactions { get; set; }
    }
}
