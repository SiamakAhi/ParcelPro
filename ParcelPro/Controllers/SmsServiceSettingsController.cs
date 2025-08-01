using Microsoft.AspNetCore.Mvc;
using ParcelPro.Interfaces;
using ParcelPro.Services;

namespace ParcelPro.Controllers
{
    public class SmsServiceSettingsController : Controller
    {
        private readonly ISMSService _smsSender;
        private readonly SmsSenderPersiaFava _sender;
        public SmsServiceSettingsController(ISMSService smsSenderService, SmsSenderPersiaFava smsSenderPersiaFava)
        {
            _smsSender = smsSenderService;
            _sender = smsSenderPersiaFava;
        }

        public IActionResult PersiaFavaSetting()
        {
            return View();
        }


        public async Task<IActionResult> SendSms()
        {
            string number = "09161114954";

            string message = _smsSender.GenerateSenderMessage("سیامک آهی", "615500000015", "https://hub.keyhanpost.ir");
            var sendResult = await _sender.SendSmsAsync(number, message);
            if (sendResult.Success)
            {
                return Ok();
            }
            return BadRequest();
        }

        public async Task<IActionResult> GeneratePaymentSettlementLink(Guid id, string BillNumber, string reciverNumber)
        {

            return Ok();
        }
    }
}
