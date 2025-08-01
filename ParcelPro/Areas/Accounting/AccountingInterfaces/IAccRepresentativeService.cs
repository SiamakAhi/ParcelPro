using ParcelPro.Areas.Accounting.Dto.AccRepresentative;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccRepresentativeService
    {
        Task<List<AccRepresentativeInfoDto>> GetRepresentativesInfo(AccRepresentativeFilterDto filter);
        Task<List<ReperesentativeTotalSalary>> ReperesentativeTotalSalariesAsync(RepresentativeSalaryFilterDto filter);
        Task<List<ReperesentativeSalaryDto>> ReperesentativeSalaryAsync(RepresentativeSalaryFilterDto filter);
    }
}
