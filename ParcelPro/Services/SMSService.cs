using Microsoft.EntityFrameworkCore;
using ParcelPro.Interfaces;
using ParcelPro.Models;

namespace ParcelPro.Services
{
    public class SMSService : ISMSService
    {
        private readonly AppDbContext _db;

        public SMSService(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public string GenerateTrackingLink(Guid billId)
        {
            string trackingLink = $"https://hub.keyhanpost.ir/Courier/Billoflading/Bill?billOfladingId={billId}";
            return trackingLink;
        }

        public async Task<SmsServiceSetting> FindByIdAsync(int id)
        {
            var sett = await _db.SmsServiceSettings.FindAsync(id);
            if (sett != null) return sett;
            else return null;

        }

        public async Task<SmsServiceSetting> GetSettingsAsync(string ProviderName)
        {
            var sett = await _db.SmsServiceSettings.FirstOrDefaultAsync(n => n.ProviderName == ProviderName);
            if (sett != null) return sett;
            else return new SmsServiceSetting();
        }


        //-------------------------------

        private const string LineBreak = "\r\n";

        public string GenerateSenderMessage(string name, string trackingCode, string trackingLink)
        {
            string message = $"جناب آقای/سرکار خانم {name}،{LineBreak}" +
                             $"مرسوله شما با شماره قرارداد {trackingCode} در کیهان پست با موفقیت ثبت شد.{LineBreak}{LineBreak}" +
                             $"برای رهگیری مرسوله و یا اعلام هرگونه مغایرت احتمالی، لطفاً به لینک زیر مراجعه فرمایید:{LineBreak}" +
                             $"{trackingLink}{LineBreak}{LineBreak}" +
                             $"با سپاس،{LineBreak}" +
                             $"کیهان پست";

            return message;
        }
        public string GenerateSenderMessageAlt(string sender, string receiver, string destination, string trackingCode, string trackingLink)
        {
            string message = $"جناب آقای/سرکار خانم {sender}،{LineBreak}" +
                             $"بارنامه شما با شماره قرارداد {trackingCode} ثبت شد.{LineBreak}" +
                             $"گیرنده: {receiver}{LineBreak}" +
                             $"مقصد: {destination}{LineBreak}{LineBreak}" +
                             $"برای رهگیری مرسوله به لینک زیر مراجعه کنید:{LineBreak}" +
                             $"{trackingLink}{LineBreak}{LineBreak}" +
                             $"با سپاس،{LineBreak}" +
                             $"کیهان پست";

            return message;
        }

        public string GenerateRecipientMessage(string name, string trackingCode, string trackingLink)
        {
            string message = $"جناب آقای/سرکار خانم {name}،{LineBreak}" +
                             $"یک مرسوله با شماره قرارداد {trackingCode} برای شما در کیهان پست ثبت گردید.{LineBreak}{LineBreak}" +
                             $"جهت رهگیری مرسوله و اطلاع از زمان تقریبی تحویل، همچنین در صورت مشاهده هرگونه مغایرت، لطفاً به لینک زیر مراجعه فرمایید:{LineBreak}" +
                             $"{trackingLink}{LineBreak}{LineBreak}" +
                             $"با تشکر،{LineBreak}" +
                             $"کیهان پست";

            return message;
        }
        public string GenerateSendPaymentLinkMessage(string trackingCode, string trackingLink)
        {
            string message = $" کیهان پست ،{LineBreak}" +
                             $"لینک پرداخت بارنامه : {trackingCode}  {LineBreak}" +
                             $"{trackingLink}{LineBreak}{LineBreak}" +
                             $"با تشکر";

            return message;
        }
        public string GenerateSendPaymentLinkMessage(Guid billId, string trackingCode)
        {
            string trackingLink = GenerateTrackingLink(billId);

            string message = $" کیهان پست ،{LineBreak}" +
                             $"لینک پرداخت بارنامه : {trackingCode}  {LineBreak}" +
                             $"{trackingLink}{LineBreak}{LineBreak}" +
                             $"با تشکر";

            return message;
        }

    }
}
