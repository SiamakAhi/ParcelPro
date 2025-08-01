using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Geolocation.GeolocationInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Services;


namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize]
    public class HubController : Controller
    {
        private readonly ICuHubService _hubService;
        private readonly IGeneralService _gs;
        private readonly UserContextService _userContext;
        private readonly IGeoGeneralService _locationService;
        private readonly IBranchUserService _branchUser;
        private readonly ICuBranchService _branchServic;
        private readonly ICargoManifestService _manifest;
        private readonly IAccCodingService _accCoding;

        private long? _sellerId;

        public HubController(ICuHubService hubService
            , IGeneralService generalService
            , UserContextService userContextService
            , IGeoGeneralService geoLocationService,
              IBranchUserService branchUser,
              ICuBranchService branchServic,
              ICargoManifestService manifestService,
              IAccCodingService codingService)
        {
            _hubService = hubService;
            _gs = generalService;
            _userContext = userContextService;
            _locationService = geoLocationService;
            _sellerId = _userContext.SellerId;
            _branchUser = branchUser;
            _branchServic = branchServic;
            _manifest = manifestService;
            _accCoding = codingService;
        }

        // GET: Courier/Hub/Hubs
        public async Task<IActionResult> Hubs()
        {
            if (_sellerId == null) return NoContent();
            var hubs = await _hubService.GetHubsAsync(_sellerId.Value);
            return View(hubs);
        }

        // GET: Courier/Hub/AddHub
        [HttpGet]
        public async Task<IActionResult> AddHub()
        {
            string userName = User.Identity.Name;
            var userInfo = await _gs.UserSettingAsync(userName);
            long? sellerId = userInfo.ActiveSellerId;

            if (!sellerId.HasValue)
                return NoContent();

            ViewBag.Cities = await _locationService.SelectItems_CitiesAsync();
            return PartialView("_AddHub");
        }

        // POST: Courier/Hub/AddHub
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHub(HubDto dto)
        {
            clsResult result = new clsResult();

            if (!_sellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد.";
                return Json(result.ToJsonResult());
            }

            dto.SellerId = _sellerId.Value;

            if (ModelState.IsValid)
            {
                result = await _hubService.CreateHubAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            // Collect model validation errors
            var modelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message += "\n" + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        // GET: Courier/Hub/UpdateHubInfo/{id}
        [HttpGet]
        public async Task<IActionResult> UpdateHubInfo(Guid id)
        {
            string userName = User.Identity.Name;
            var userInfo = await _gs.UserSettingAsync(userName);
            long? sellerId = userInfo.ActiveSellerId;

            if (!sellerId.HasValue)
            {
                ViewBag.ErrorMessage = "دسترسی به شرکت فعال یافت نشد.";
                return PartialView("_UpdateHubInfo");
            }

            ViewBag.Cities = await _locationService.SelectItems_CitiesAsync();

            var hub = await _hubService.GetHubByIdAsync(id);

            return PartialView("_UpdateHubInfo", hub);
        }

        // POST: Courier/Hub/UpdateHubInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateHubInfo(HubDto dto)
        {
            clsResult result = new clsResult();

            if (!_sellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد.";
                return Json(result.ToJsonResult());
            }

            dto.SellerId = _sellerId.Value;

            if (ModelState.IsValid)
            {
                result = await _hubService.UpdateHubAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var modelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message += "\n" + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        // POST: Courier/Hub/DeleteHub/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteHub(Guid itemId)
        {
            clsResult result = new clsResult();

            if (!_sellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد.";
                return Json(result.ToJsonResult());
            }

            result = await _hubService.DeleteHubAsync(itemId);
            if (result.Success)
            {
                result.ShowMessage = true;
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.updateType = 1;
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }

        //============================================================== VEHICLE ================
        public async Task<IActionResult> Vehicles()
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            var model = await _manifest.GetVehiclesAsync(_userContext.SellerId.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddVehicle()
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");
            ViewBag.Tafsils = await _accCoding.SelectList_TafsilsAsync(_sellerId.Value);
            ViewBag.Moeins = await _accCoding.SelectList_MoeinsAsync(_sellerId.Value);

            return PartialView("_AddVehicle");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicle(VehicleDto dto)
        {
            clsResult result = new clsResult();
            if (!_sellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد.";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _sellerId.Value;

            if (ModelState.IsValid)
            {
                if (dto.MoeinId == 0)
                    dto.MoeinId = 0;
                if (dto.TafsilId == 0)
                    dto.TafsilId = 0;

                result = await _manifest.AddVehicleAsync(dto, _sellerId.Value, User.Identity.Name);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var modelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message += "</br>" + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVehicle(int id)
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");
            var model = await _manifest.FindVehicleByIdAsync(id);
            ViewBag.Tafsils = await _accCoding.SelectList_TafsilsAsync(_sellerId.Value);
            ViewBag.Moeins = await _accCoding.SelectList_MoeinsAsync(_sellerId.Value);

            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVehicle(VehicleDto dto)
        {
            clsResult result = new clsResult();
            if (!_sellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد.";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _sellerId.Value;

            if (ModelState.IsValid)
            {
                if (dto.MoeinId == 0)
                    dto.MoeinId = 0;
                if (dto.TafsilId == 0)
                    dto.TafsilId = 0;

                result = await _manifest.UpdateVehicleAsync(dto.Id, dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var modelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message += "</br>" + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        //============================================================== DRIVERS ================

        public async Task<IActionResult> Drivers()
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            var model = await _manifest.GetDriversAsync(_userContext.SellerId.Value);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddDriver()
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");
            ViewBag.Tafsils = await _accCoding.SelectList_TafsilsAsync(_sellerId.Value);
            ViewBag.Moeins = await _accCoding.SelectList_MoeinsAsync(_sellerId.Value);

            return PartialView("_AddDriver");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDriver(DriverDto dto)
        {
            clsResult result = new clsResult();
            if (!_sellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد.";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _sellerId.Value;
            dto.CreatedBy = User.Identity.Name;

            if (ModelState.IsValid)
            {
                if (dto.MoeinId == 0)
                    dto.MoeinId = 0;
                if (dto.TafsilId == 0)
                    dto.TafsilId = 0;

                result = await _manifest.AddDriverAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var modelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message += "</br>" + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        public async Task<IActionResult> UpdateDriver(int id)
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            var model = await _manifest.FindDriverByIdAsync(id);
            ViewBag.Tafsils = await _accCoding.SelectList_TafsilsAsync(_sellerId.Value);
            ViewBag.Moeins = await _accCoding.SelectList_MoeinsAsync(_sellerId.Value);

            return PartialView("_UpdateDriver", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDriver(DriverDto dto)
        {
            clsResult result = new clsResult();
            if (!_sellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد.";
                return Json(result.ToJsonResult());
            }
            if (dto.MoeinId == 0)
                dto.MoeinId = 0;
            if (dto.TafsilId == 0)
                dto.TafsilId = 0;

            dto.SellerId = _sellerId.Value;
            dto.CreatedBy = User.Identity.Name;

            if (ModelState.IsValid)
            {
                if (dto.MoeinId == 0)
                    dto.MoeinId = 0;
                if (dto.TafsilId == 0)
                    dto.TafsilId = 0;

                result = await _manifest.UpdateDriverAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var modelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message += "</br>" + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        //==============================================================================

        public async Task<IActionResult> CreateManifestHeader()
        {
            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);

            if (!_sellerId.HasValue || user == null || branch == null) return NoContent();
            return View();
        }
    }
}

