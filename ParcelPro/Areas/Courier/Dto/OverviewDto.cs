using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Courier.Dto
{
    public class WayBillsStatusCheckDto
    {
        [Display(Name = "تعداد بارنامه‌های در حال صدور")]
        public int IssuingWaybillsCount { get; set; } = 0;

        [Display(Name = "تعداد بارنامه‌های در انتظار جمع‌آوری")]
        public int AwaitingCollectionWaybillsCount { get; set; } = 0;

        [Display(Name = "تعداد بارنامه‌های آماده برای مانیفست")]
        public int ReadyForManifestWaybillsCount { get; set; } = 0;

        [Display(Name = "تعداد بارنامه‌های در حال تفکیک")]
        public int InSeparationWaybillsCount { get; set; } = 0;

        [Display(Name = "تعداد بارنامه‌های آماده توزیع")]
        public int ReadyForDistributionWaybillsCount { get; set; } = 0;
        public int NoSetteledCount { get; set; } = 0;

        public int TotalWaybillsCount
            => IssuingWaybillsCount + AwaitingCollectionWaybillsCount +
               ReadyForManifestWaybillsCount + InSeparationWaybillsCount +
               ReadyForDistributionWaybillsCount;
    }
}
