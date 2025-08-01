using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.BranchUserDto;
using ParcelPro.Areas.Courier.Dto.RepresentativeDtos;

namespace ParcelPro.Areas.Representatives.Dtos
{
    public class VmBranchUserPanel
    {
        public BillOfLadingFilterDto filter { get; set; } = new BillOfLadingFilterDto();
        public BranchDto Branch { get; set; }
        public VmBranchUser CurrentUser { get; set; }
        public Pagination<ViewBillOfLadings> Bills { get; set; }
        public WayBillsStatusCheckDto OverView { get; set; } = new WayBillsStatusCheckDto();

    }
}
