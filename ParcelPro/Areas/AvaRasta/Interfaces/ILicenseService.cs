using ParcelPro.Areas.AvaRasta.Dto;
using ParcelPro.Areas.AvaRasta.Models.Entities;

namespace ParcelPro.Areas.AvaRasta.Interfaces
{
    public interface ILicenseService
    {
        Task<clsResult> AddLicenseAsync(AddLicenseDto dto);
        Task<List<License>> GetLicensesByCustomerIdAsync(int customerId);
        Task<clsResult> RemoveLicenseAsync(Guid licenseId);
    }
}
