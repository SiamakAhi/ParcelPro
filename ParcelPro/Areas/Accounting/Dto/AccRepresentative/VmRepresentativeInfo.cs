namespace ParcelPro.Areas.Accounting.Dto.AccRepresentative
{
    public class VmRepresentativeInfo
    {
        public List<AccRepresentativeInfoDto> Branches { get; set; } = new List<AccRepresentativeInfoDto>();
        public AccRepresentativeFilterDto filter { get; set; }
    }
}
