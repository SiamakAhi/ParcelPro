using ParcelPro.Areas.Support.Dtos;
using ParcelPro.ViewModels.IdentityViewModels;

namespace ParcelPro.Areas.Support.SuportInterfaces
{
    public interface ISupportService
    {
        Task<SmsResultDto> SendSmsToSupportAsync(string message);
        Task<VmUserInfo>? GetUserInfoAsync(string userName);
    }
}
