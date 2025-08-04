using ParcelPro.Areas.Treasury.Models.Entities;
using System.ComponentModel.DataAnnotations;



namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_BillOfLading
    {
        [Key]
        public Guid Id { get; set; }
        public short WayBillType { get; set; } = 1;
        public long SellerId { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string WaybillNumber { get; set; }
        public string? ReferenceCode { get; set; }

        [Display(Name = "تاریخ صدور")]
        [Required(ErrorMessage = "تاریخ بارنامه نامشخص است")]
        public DateTime IssuanceDate { get; set; } = DateTime.Now;
        public TimeSpan IssuanceTime { get; set; } = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        [Display(Name = " مسیر")]
        [Required(ErrorMessage = "نعیین مسیر الزامی است")]
        public int RouteId { get; set; }
        public virtual Cu_Route Route { get; set; }


        [Display(Name = " سرویس")]
        [Required(ErrorMessage = "نوع سرویس را مشخص کنید")]
        public int ServiceId { get; set; }
        public virtual Cu_Service Service { get; set; }

        [Display(Name = "شعبه صادرکننده")]
        public Guid OriginBranchId { get; set; }
        public Cu_Branch IssuingBranch { get; set; }


        [Display(Name = " فرستنده")]
        [Required(ErrorMessage = "تعیین فرستنده الزامی است")]
        public long SenderId { get; set; }
        public virtual Party Sender { get; set; }
        [Display(Name = " آدرس فرستنده")]
        [Required(ErrorMessage = "نوشتن آدرس فرستنده الزامی است")]
        public string SenderAddress { get; set; }
        public string? SenderPhone { get; set; }
        public string? RequesterPerson { get; set; }


        [Display(Name = " گیرنده")]
        [Required(ErrorMessage = "تعیین گیرنده الزامی است")]
        public long ReceiverId { get; set; }
        public virtual Party Receiver { get; set; }
        [Display(Name = " آدرس گیرنده")]
        [Required(ErrorMessage = "نوشتن آدرس گیرنده الزامی است")]
        public string ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }
        public byte[]? DigitalSignature { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }


        [Display(Name = "تعداد تحویل شده")]
        public int DeliveredCount { get; set; } = 0;


        // 1- نقدی
        // 2- اعتباری
        // 3- پسکرایه
        [Display(Name = " نوع پرداخت")]
        public short? SettelmentType { get; set; }
        public string? LastStatusDescription { get; set; }

        [Display(Name = " وضعیت بارنامه")]
        [Required(ErrorMessage = "فیلد شناسه وضعیت بارنامه الزامی است")]
        public short BillOfLadingStatusId { get; set; } = 1;
        public virtual Cu_BillOfLadingStatus BillOfLadingStatus { get; set; }

        [Display(Name = "هاب مبدأ")]
        [Required(ErrorMessage = "هاب مبدأ شناسایی نشد")]
        public Guid OriginHubId { get; set; }
        public Cu_Hub OriginHub { get; set; }


        [Display(Name = "هاب مقصد")]
        public Guid? DestinationHubId { get; set; }
        public Cu_Hub? DestinationHub { get; set; }

        [Display(Name = "شرکت صادرکننده بارنامه")]
        public int? BusinessPartnerId { get; set; }
        public Cu_BusinessPartner? BusinessPartner { get; set; }


        [Display(Name = "قرارداد همکاری")]
        public int? ContractId { get; set; }
        public Cu_SaleContract? Contract { get; set; }

        //====================================================================
        [Display(Name = "کاربر ثبت کننده")]
        [Required(ErrorMessage = "فیلد کاربر ثبت کننده الزامی است")]
        public string CreatedBy { get; set; }

        [Display(Name = "تاریخ به روزرسانی")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "کاربر ادیت کننده")]
        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        //===== اطلاعات تحویل مرسوله
        public DateTime? tg_DeliveryDate { get; set; }
        public string? tg_Name { get; set; }
        public string? tg_Phone { get; set; }
        public string? tg_NationalityCode { get; set; }
        public string? tg_Description { get; set; }
        public string? DeliveryErrorMessage { get; set; }
        public byte[]? tg_SignatureData { get; set; }
        public string? tg_CourierManUserName { get; set; }
        public bool Delivered { get; set; } = false;

        public long? InvoiceId { get; set; }
        public virtual Cu_Invoice? CuInvoice { get; set; }


        [Display(Name = "نماینده توزیع")]
        public Guid? DistributerBranchId { get; set; } = Guid.Empty;
        public virtual Cu_Branch? DistributerBranch { get; set; }

        public string? CustomerKeyword { get; set; }
        public long partnerId { get; set; } = 0;
        // مرسوله ها
        public virtual ICollection<Cu_Consignment> Consignments { get; set; } = new List<Cu_Consignment>();
        public virtual ICollection<Cu_BillCost> BillCosts { get; set; } = new List<Cu_BillCost>();
        public virtual ICollection<Cu_FinancialTransaction> FinancialTransactions { get; set; } = new List<Cu_FinancialTransaction>();
        public virtual ICollection<TreTransaction> TreTransactions { get; set; } = new List<TreTransaction>();

    }

    public enum SettelmentType : short
    {
        [Display(Name = "نقدی")]
        Cash = 1,

        [Display(Name = "اعتباری")]
        Credit = 2,

        [Display(Name = "پسکرایه")]
        PostPaid = 3
    }

}
