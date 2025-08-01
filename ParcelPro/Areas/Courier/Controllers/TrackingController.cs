using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;

namespace ParcelPro.Areas.Courier.Controllers
{
    public class TrackingController : Controller
    {
        private readonly ITrachkingService _tracking;

        public TrackingController(ITrachkingService trackingService)
        {
            _tracking = trackingService;
        }

        public async Task<IActionResult> BillTracking(Guid id)
        {
            var model = _tracking.TrackingAsync(id);
            return View(id);
        }
    }
}
