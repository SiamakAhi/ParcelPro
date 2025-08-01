using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Dto
{
    public class BillCashPayDto
    {

        public Guid Id { get; set; } // input hidden

        // 1- واریز
        // 2 - برداشت
        public byte TransactionTypeId { get; set; } = 1; // input hidden

        [Display(Name = "فروشنده")]
        public long SellerId { get; set; } // input hidden

        [Display(Name = "شماره بارنامه")]
        public Guid? BillOfLadingId { get; set; } // input hidden
        public Guid? BillFinancialtransactionId { get; set; } // input hidden
        public string? BillNumber { get; set; } // input hidden

        [Display(Name = "طرف حساب")]
        public long AccountPartyId { get; set; } // Select .select2

        [Display(Name = "روش دریافت / پرداخت")]
        public int OperationId { get; set; } // select

        [Display(Name = "تاریخ")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        [Display(Name = "تاریخ")]
        public string? strTransactionDate { get; set; } // input .flatpickr-input
        public string? SenderName { get; set; }
        public string? PartyName { get; set; }
        public string? PartyMobile { get; set; }

        public int SettelmentType { get; set; }

        [Display(Name = "زمان")]
        public TimeSpan TransactionTime { get; set; } = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        [Display(Name = "شرح تراکنش")]
        public string? Description { get; set; } // input

        [Display(Name = "شناسه شعبه")]
        public Guid? BranchId { get; set; } // input hidden

        [Display(Name = "کاربر")]
        public string? UserId { get; set; } // input hidden

        [Display(Name = "مبلغ بدهی")]
        public long Amount { get; set; } // input (Readonly) or Lable

        [Display(Name = "قابل پرداخت")]
        public long DebitAmount { get; set; } = 0; // input (Readonly) or Lable

        [Display(Name = "پرداخت شده")]
        public long CreditAmount { get; set; } = 0;// input (Readonly) or Lable
        public long PayAmount { get; set; } = 0;// input (Readonly) or Lable

        [Display(Name = "مبلغ دریافتی")]
        public string strPayAmount { get; set; }// input (Readonly) or Lable

        [Display(Name = " حساب بانکی")]
        public int? BankAccountId { get; set; } //select

        [Display(Name = "گارت خوان")]
        public int? PosId { get; set; } // select

        [Display(Name = "شماره حواله")]
        public string? TransferNumber { get; set; } // input

        [Display(Name = "تاریخ ایجاد بارنامه")]
        public DateTime BillCreateDate { get; set; } // input (Readonly) or Lable

        public List<PaymentListViewModel>? PaymentList { get; set; } //Table

    }
}
