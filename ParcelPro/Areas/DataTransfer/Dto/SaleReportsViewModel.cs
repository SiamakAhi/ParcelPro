using ParcelPro.Areas.DataTransfer.Models;

namespace ParcelPro.Areas.DataTransfer.Dto
{
    public class SaleReportsViewModel
    {
        public List<KPOldSystemSaleReport>? Reports { get; set; }
        public SaleFilterDto? filter { get; set; }
    }
}
