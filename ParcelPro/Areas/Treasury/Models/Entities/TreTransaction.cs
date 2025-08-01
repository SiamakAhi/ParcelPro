using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Models.Entities
{
    public class TreTransaction
    {
        [Key]
        public Guid Id { get; set; }

        // 1- واریز
        // 2 - برداشت
        public byte TransactionTypeId { get; set; }

        [Display(Name = "فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "شماره بارنامه")]
        public Guid? BillOfLadingId { get; set; }
        public Guid? BillFinancialtransactionId { get; set; }
        public string? BillNumber { get; set; }

        [Display(Name = "طرف حساب")]
        public long AccountPartyId { get; set; }

        [Display(Name = "روش دریافت / پرداخت")]
        public int OperationId { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Display(Name = "زمان")]
        public TimeSpan TransactionTime { get; set; } = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        [Display(Name = "شرح تراکنش")]
        public string? Description { get; set; }
        [Display(Name = "شرح تراکنش")]
        public string? UserRemart { get; set; }

        [Display(Name = "شناسه شعبه")]
        public Guid? BranchId { get; set; }

        [Display(Name = "کاربر")]
        public string? UserId { get; set; }

        [Display(Name = "مبلغ")]
        public long Amount { get; set; }

        [Display(Name = "بد")]
        public long DebitAmount { get; set; } = 0;

        [Display(Name = "بس")]
        public long CreditAmount { get; set; } = 0;

        [Display(Name = " حساب بانکی")]
        public int? BankAccountId { get; set; }

        [Display(Name = "گارت خوان")]
        public int? PosId { get; set; }

        [Display(Name = "شماره حواله")]
        public string? TransferNumber { get; set; }

        [Display(Name = "شناسه تراکنش درگاه پرداخت")]
        public string? PaymentGatewayTransactionId { get; set; }
        public string? OnlineGetwayName { get; set; }

        [Display(Name = "شماره چک")]
        public string? CheckNumber { get; set; }

        [Display(Name = "تاریخ سررسید چک")]
        public DateTime? CheckDueDate { get; set; }

        [Display(Name = "نام صاحب چک")]
        public string? CheckOwnerName { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Guid? DocId { get; set; }

        public virtual Cu_BillOfLading? BillOfLading { get; set; }
        public virtual Party Party { get; set; }
        public virtual TreOperation? Operation { get; set; }
        public virtual TreBankPosUc? Pos { get; set; }
        public virtual kh_BankAccount? BankAccount { get; set; }
        public virtual AppIdentityUser? User { get; set; }
        public virtual Cu_FinancialTransaction? BillFinancialTransaction { get; set; }

    }
}
