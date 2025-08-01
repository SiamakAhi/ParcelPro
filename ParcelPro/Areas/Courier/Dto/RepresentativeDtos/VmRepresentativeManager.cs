using ParcelPro.Areas.DataTransfer.Models;

namespace ParcelPro.Areas.Courier.Dto.RepresentativeDtos
{
    public class VmRepresentativeManager
    {
        public List<RepresentativeDto> Representatives { get; set; } = new List<RepresentativeDto>();
        public List<CuOld_SaleReportGrouped> SaleDailyReport { get; set; } = new List<CuOld_SaleReportGrouped>();
        public List<CuOld_SaleReportGrouped> SaleDailyReportByRepresentative { get; set; } = new List<CuOld_SaleReportGrouped>();
        public Pagination<KPOldSystemSaleReport> RepresentativeReportDetail { get; set; }
        public SaleFilterDto filter { get; set; } = new SaleFilterDto();
    }
}
