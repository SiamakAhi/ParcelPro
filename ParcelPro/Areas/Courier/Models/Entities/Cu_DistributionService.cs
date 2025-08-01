using System.ComponentModel.DataAnnotations;


namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_DistributionService
    {

        [Key]
        public long Id { get; set; }
        public long SellerId { get; set; }
        public long PartnerId { get; set; }

        [Display(Name = "شماره")]
        public int Shomare { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        [Display(Name = "مبلغ")]
        public decimal Mablagh { get; set; }

        [Display(Name = "مسیر")]
        public string? RouteName { get; set; }

        [Display(Name = "کرایه")]
        public long? Karaye { get; set; } = 0;

        [Display(Name = "تمبر و بارنامه")]
        public long TambreVaBarnam { get; set; } = 0;

        [Display(Name = "بسته بندی")]
        public long BasteBandi { get; set; } = 0;

        [Display(Name = "متفرقه")]
        public long Motafavvete { get; set; } = 0;

        [Display(Name = "توزیع")]
        public long Towzi { get; set; } = 0;

        [Display(Name = "متفرقه مقصد")]
        public decimal MotafavveteMobad { get; set; } = 0;

        [Display(Name = "جمع آوری")]
        public decimal JamAvary { get; set; } = 0;

        [Display(Name = "قابل پرداخت")]
        public decimal GhabelePardakht { get; set; } = 0;

        [Display(Name = "عنوان فرستنده")]
        public string OnvanFerestande { get; set; }

        [Display(Name = "عنوان گیرنده")]
        public string OnvanGirande { get; set; }

        public short SettelmentTypeId { get; set; }
        //
        public long? CreditCustomer { get; set; }
        public float? SharePercentage { get; set; } = null;
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

        public DateTime DataEntryTime { get; set; } = DateTime.Now;
        public string CreateBy { get; set; }
    }
}
