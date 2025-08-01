using ParcelPro.Areas.Courier.Dto;

namespace ParcelPro.Areas.Client.Dto
{
    public class ClientPanelViewModel
    {
        public WaybillFilterDto filter { get; set; } = new WaybillFilterDto();
        public Pagination<ViewBillOfLadings> Bills { get; set; }
    }
}
