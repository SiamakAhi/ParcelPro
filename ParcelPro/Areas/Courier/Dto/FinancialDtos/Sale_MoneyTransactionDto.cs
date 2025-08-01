using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto.FinancialDtos
{
    public class Sale_MoneyTransactionDto
    {
        public byte TransactionTypeId { get; set; }

        [Display(Name = "فروشنده")]
        public long SellerId { get; set; }

        [Display(Name = "شماره بارنامه")]
        public Guid? BillOfLadingId { get; set; }
        public Guid? BillFinancialtransactionId { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string? BillNumber { get; set; }

        [Display(Name = "طرف حساب")]
        public long AccountPartyId { get; set; }

        [Display(Name = "طرف حساب")]
        public string Party { get; set; }

        [Display(Name = "روش دریافت / پرداخت")]
        public int OperationId { get; set; }
        public string OperationName { get; set; }

        [Display(Name = "تاریخ تراکنش")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "زمان")]
        public TimeSpan TransactionTime { get; set; }

        [Display(Name = "شرح تراکنش")]
        public string? Description { get; set; }

        [Display(Name = " شعبه")]
        public Guid? BranchId { get; set; }

        [Display(Name = " شعبه")]
        public string IssuerBranch { get; set; }

        [Display(Name = "کاربر")]
        public string? UserId { get; set; }

        [Display(Name = "مبلغ بارنامه")]
        public long BillAmount { get; set; }

        [Display(Name = "مبلغ")]
        public long Amount { get; set; }

        [Display(Name = "بد")]
        public long DebitAmount { get; set; } = 0;

        [Display(Name = "بس")]
        public long CreditAmount { get; set; } = 0;

        [Display(Name = " حساب بانکی")]
        public int? BankAccountId { get; set; }
        public string BankAccount { get; set; }

        [Display(Name = "گارت خوان")]
        public int? PosId { get; set; }

        [Display(Name = "گارت خوان")]

        public string? POS { get; set; }

        [Display(Name = "شماره حواله")]
        public string? TransferNumber { get; set; }

        [Display(Name = " درگاه پرداخت")]
        public string? PaymentGatewayTransactionId { get; set; }
        public string? OnlineGetwayName { get; set; }

        [Display(Name = "شماره چک")]
        public string? CheckNumber { get; set; }

        [Display(Name = "تاریخ سررسید چک")]
        public DateTime? CheckDueDate { get; set; }

        [Display(Name = "نام صاحب چک")]
        public string? CheckOwnerName { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
