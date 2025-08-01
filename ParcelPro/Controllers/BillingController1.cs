using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;

namespace ParcelPro.Controllers
{


    public class BillingController1 : Controller
    {
        private readonly IBillofladingService _bill;
        public BillingController1(IBillofladingService billOfLadingService)
        {
            _bill = billOfLadingService;
        }

        public IActionResult PresentInvoiceAndPay(Guid BillId)
        {

            return View();
        }
    }
}
