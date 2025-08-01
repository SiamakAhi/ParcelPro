using ParcelPro.Models;

namespace ParcelPro.Interfaces
{
    public interface ISMSService
    {

        Task<SmsServiceSetting> FindByIdAsync(int id);
        Task<SmsServiceSetting> GetSettingsAsync(string ProviderName);

        //......
        string GenerateSenderMessage(string name, string trackingCode, string trackingLink);
        string GenerateSenderMessageAlt(string sender, string receiver, string destination, string trackingCode, string trackingLink);
        string GenerateRecipientMessage(string name, string trackingCode, string trackingLink);
        string GenerateSendPaymentLinkMessage(string name, string trackingLink);
        string GenerateSendPaymentLinkMessage(Guid billId, string trackingCode);

    }
}
