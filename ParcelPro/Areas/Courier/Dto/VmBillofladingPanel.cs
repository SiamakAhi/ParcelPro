using ParcelPro.Areas.Courier.Dto.BranchUserDto;
using ParcelPro.Areas.Courier.Dto.RepresentativeDtos;

namespace ParcelPro.Areas.Courier.Dto
{
    public class VmBillofladingPanel
    {
        public BranchDto? Branch { get; set; }
        public VmBranchUser? CurrentUser { get; set; }
        public BillOfLadingDto? BillOfLading { get; set; }
        public ConsigmentDto? Consigmen { get; set; }


    }
}
