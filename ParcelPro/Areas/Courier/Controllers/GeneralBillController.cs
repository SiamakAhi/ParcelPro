using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    public class GeneralBillController : Controller
    {
        private readonly IBillofladingService _bill;
        public GeneralBillController(IBillofladingService bill)
        {
            _bill = bill;
        }
        public async Task<IActionResult> PrintBill(Guid billOfladingId)
        {
            var model = await _bill.GetBillOfLadingDtoAsync(billOfladingId);
            return View(model);
        }
    }
}
