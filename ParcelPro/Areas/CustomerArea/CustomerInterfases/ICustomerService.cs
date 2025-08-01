using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.CustomerArea.Dto;
using ParcelPro.ViewModels;
using ParcelPro.ViewModels.CommercialViewModel;

namespace ParcelPro.Areas.CustomerArea.CustomerInterfases
{
    public interface ICustomerService
    {
        Task<List<VmCustomerUsers>> GetCustomerUsersAsync(int? CustomerId);
        Task<int?> GetCustomerIdByUsernameAsync(string UserName);
        int? GetCustomerIdByUsername(string UserName);
        Task<SelectList> Selectlist_CustomerSellersAsync(string UserName);
        Task<ResultDto> CreateCustomerUserAsync(AddCustomerUserDto dto);
        Task<UpdatePermissionDto> GetPermissionInfoAsync(string userName);
        Task<ResultDto> UpdateUserSettingAsync(userSettingDto dto);
        Task<ResultDto> AddSellerToUserAsync(UserSellerDto dto);
        Task<ResultDto> RemoveSellerFromUserAsync(Int64 Id);
        Task<ResultDto> DelCustomerUserAsync(string userName);
    }
}
