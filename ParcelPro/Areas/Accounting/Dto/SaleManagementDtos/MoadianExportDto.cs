using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class MoadianExportDto
    {

        [Display(Name = "کد صورتحساب در سیستم حسابداری")]
        public string? AccountingInvoiceCode { get; set; } = "xx";

        [Display(Name = "شماره صورتحساب")]
        public string? InvoiceNumber { get; set; }

        [Display(Name = "تاریخ صورتحساب")]
        public string? InvoiceDate { get; set; } = DateTime.Now.LatinToPersian();

        [Display(Name = "نوع صورتحساب")]
        public short? InvoiceType { get; set; } = 1;

        [Display(Name = "نام کامل خریدار")]
        public string? BuyerFullName { get; set; }

        [Display(Name = "نوع شخص حقیقی یا حقوقی")]
        public short BuyerType { get; set; }

        [Display(Name = "شماره / شناسه ملی")]
        [Required]
        public string? NationalId { get; set; }

        [Display(Name = "کد اقتصادی جدید")]
        [Required]
        public string? EconomicCode { get; set; }

        [Display(Name = "کدپستی")]
        public string? PostalCode { get; set; }

        [Display(Name = "آدرس")]
        public string? Address { get; set; }

        [Display(Name = "کالا / خدمت")]
        public short IsService { get; set; } = 1;

        [Display(Name = "شناسه 13 رقمی کالا یا خدمت")]
        public string? ItemIdentifier13 { get; set; } = "2330001421466";

        [Display(Name = "شرح کالا یا خدمت")]
        public string? ItemDescription => ("حمل بار جاده ای به شماره بارنامه " + AccountingInvoiceCode);

        [Display(Name = "کد واحد اندازه گیری کالا یا خدمت")]
        public string? UnitCode { get; set; } = "167";

        [Display(Name = "تعداد")]
        [Required]
        public long Quantity { get; set; } = 1;

        [Display(Name = "تخفیف")]
        [Required]
        public long Discount { get; set; } = 0;

        [Display(Name = "نرخ ارزش افزوده")]
        [Required]
        public float VATRate { get; set; } = 0;

        [Display(Name = "مبلغ ارزش افزوده")]
        [Required]
        public long VATAmount { get; set; } = 0;

        [Display(Name = "نوع تسویه حساب")]
        public short SettlementType { get; set; } = 1;

        public long BasePrice { get; set; } = 0;
        public long TotalCost { get; set; }
        public long TotalPriceBeforDiscount => BasePrice + TotalCost;
        public long TotalDiscount { get; set; } = 0;
        public long TotalPriceAfterDiscount => statusId <= 11 ? BasePrice + TotalCost - TotalDiscount : 0;
        public long VatPrice { get; set; } = 0;
        public long UnitPrice => TotalPriceAfterDiscount + VatPrice;

        public short statusId { get; set; } = 1;

    }
}
