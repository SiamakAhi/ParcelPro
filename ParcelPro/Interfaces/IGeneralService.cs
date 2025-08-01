using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.ViewModels.CommercialViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParcelPro.Interfaces
{
    public interface IGeneralService
    {
        Task<userSettingDto> GetUserSettingAsync(string username, Int64 SellerId, int? customerId);
        Task<userSettingDto> SetAndGetUserSettingAsync(string username, long sellerId, int? customerId, int? periodId);
        Task<userSettingDto> SetUserActivePeriodAsync(string username, int periodId, int? customerId);
        Task<userSettingDto> UserSettingAsync(string username);
        Task<SelectList> SelectList_GetCustomerSellersAsync(int customerId);
        Task<SelectList> SelectList_GetSellerPeriodsAsync(Int64 SellerId);
        Task<List<FinancePeriodsDto>> GetSellerPeriodsAsync(Int64 SellerId);
        Task<SelectList> SelectList_GetUserSellersAsync(string userName);
        Task<Int64?> GetActiveSellerIdAsync(string userName);
        Task<string> ActiveSellerName(string userName);
        Task<int?> GetActiveUserFinancePeriodIdAsync(string username);
        Task<userSettingDto> GetUserSettingAsync(string username);
        Task<userSettingDto> UpdateUserSettingAsync(userSettingDto dto);

    }
}
