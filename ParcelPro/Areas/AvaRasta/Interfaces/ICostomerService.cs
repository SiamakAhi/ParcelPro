using ParcelPro.Areas.AvaRasta.ViewModels;
using ParcelPro.Areas.CustomerArea.Dto;
using ParcelPro.ViewModels;

namespace ParcelPro.Areas.AvaRasta.Interfaces
{
    public interface ICostomerService
    {
        IQueryable<VmCustomer> GetCustomers(string name = "");
        Task<VmCustomer> GetVmCustomerByIdAsync(int Id);
        Task<ResultDto> AddCustomerAsync(VmCustomer n);
        Task<ResultDto> UpdateCustomerAsync(VmCustomer n);
        Task<ResultDto> DeleteCustomerAsync(int id);
        Task<List<VmCustomerUsers>> GetCustomerUsersAsync(int? CustomerId);
    }
}
