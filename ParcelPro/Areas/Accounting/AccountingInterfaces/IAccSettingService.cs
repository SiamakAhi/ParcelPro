using ParcelPro.Areas.Accounting.Dto;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccSettingService
    {
        Task<AccSettingDto> GetSettingAsync(long sellerId);
        Task<clsResult> UpdateSettingAsync(AccSettingDto settingDto);
        Task<clsResult> AddSettingAsync(AccSettingDto settingDto);
        Task<clsResult> DeleteSettingAsync(long id);
    }
}
