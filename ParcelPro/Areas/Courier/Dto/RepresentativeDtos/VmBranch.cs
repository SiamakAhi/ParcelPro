using ParcelPro.Areas.Accounting.Dto.AccRepresentative;

namespace ParcelPro.Areas.Courier.Dto.RepresentativeDtos
{
    public class VmBranch
    {
        public List<BranchDto> Branches { get; set; } = new List<BranchDto>();
        public AccRepresentativeFilterDto filter { get; set; }
    }
}
