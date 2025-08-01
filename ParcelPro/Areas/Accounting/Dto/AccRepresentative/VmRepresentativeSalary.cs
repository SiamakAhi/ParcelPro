namespace ParcelPro.Areas.Accounting.Dto.AccRepresentative
{
    public class VmRepresentativeSalary
    {
        public RepresentativeSalaryFilterDto filter { get; set; } = new RepresentativeSalaryFilterDto();
        public List<ReperesentativeTotalSalary> Reposrt { get; set; } = new List<ReperesentativeTotalSalary>();
        public List<ReperesentativeSalaryDto> ReposrtDetails { get; set; } = new List<ReperesentativeSalaryDto>();

    }
}
