using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class DistributionWaybillDto
    {

        public Guid BillOdLadingId { get; set; }
        public long SellerId { get; set; }

        [Display(Name = "شماره بارنامه")]
        public string WaybillNumber { get; set; }

        [Display(Name = "تاریخ صدور")]
        public DateTime IssuanceDate { get; set; }

        [Display(Name = " مسیر")]
        public int RouteId { get; set; }

        [Display(Name = " سرویس")]
        public int ServiceId { get; set; }

        [Display(Name = "شعبه صادرکننده")]
        public Guid OriginBranchId { get; set; }

        [Display(Name = "شعبه صادرکننده")]
        public string? OriginBranchName { get; set; }

        [Display(Name = "مقصد")]
        public string? DestinationCity { get; set; }

        [Display(Name = "حداکثر درصد مجاز برای تخفیف")]
        public decimal? MaxDiscountRate { get; set; }


        public int? ZoneId { get; set; }
        public int ParcelQty { get; set; }
        public float VatRate { get; set; } = 0;

    }
}
