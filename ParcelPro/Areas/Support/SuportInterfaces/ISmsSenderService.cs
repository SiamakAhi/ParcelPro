using ParcelPro.Areas.Support.Dtos;

namespace ParcelPro.Areas.Support.SuportInterfaces
{
    public interface ISmsSenderService
    {
        Task<SmsResultDto> Send_KavenegarAsync(string Reciptor, string Message);
    }
}
