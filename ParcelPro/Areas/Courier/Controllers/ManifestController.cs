using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize]
    public class ManifestController : Controller
    {
        private readonly UserContextService _userContext;
        private readonly IBillofladingService _bill;
        private readonly ICuBranchService _branchServic;
        private readonly ICourierServiceService _courier;
        private readonly IGeneralService _gs;
        private readonly ICargoManifestService _manifest;
        public ManifestController(
            UserContextService userContextService
            , IBillofladingService billofladingService
            , ICuBranchService branchService
            , ICourierServiceService courierService
            , IGeneralService generalService,
              ICargoManifestService manifest)
        {
            _userContext = userContextService;
            _bill = billofladingService;
            _branchServic = branchService;
            _courier = courierService;
            _gs = generalService;
            _manifest = manifest;
        }

        public async Task<ActionResult> NewManifest()
        {

            return View();
        }
    }
}
