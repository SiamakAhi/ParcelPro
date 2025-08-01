using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Geolocation.GeolocationInterfaces;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize]
    public class CourierServiceController : Controller
    {
        protected readonly ICourierServiceService _courierService;
        protected readonly ICuPricingService _pricing;
        protected readonly UserContextService _userContext;
        protected readonly IGeoGeneralService _geo;
        public CourierServiceController(ICourierServiceService courierServiceService
            , ICuPricingService pricingService
           , UserContextService userContext
            , IGeoGeneralService geoService)
        {
            _courierService = courierServiceService;
            _pricing = pricingService;
            _userContext = userContext;
            _geo = geoService;
        }
        public async Task<IActionResult> CourierSrvices()
        {
            if (_userContext == null || !_userContext.SellerId.HasValue) return NoContent();
            var model = await _courierService.GetServicesAsync(_userContext.SellerId.Value);

            return View(model);
        }

        [HttpGet]
        public IActionResult AddCourierService()
        {
            ViewBag.RatingType = _courierService.ServiceRatingType();
            return PartialView("_AddCourierService");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddCourierService(Cu_ServiceDto dto)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _userContext.SellerId.Value;
            if (ModelState.IsValid)
            {
                dto.SellerId = _userContext.SellerId.Value;
                result = await _courierService.AddServiceAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCourierService(int id)
        {
            var model = await _courierService.FindServiceByIdAsync(id);
            ViewBag.RatingType = _courierService.ServiceRatingType();
            return PartialView("_UpdateCourierService", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> UpdateCourierService(Cu_ServiceDto dto)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _userContext.SellerId.Value;
            if (ModelState.IsValid)
            {
                dto.SellerId = _userContext.SellerId.Value;
                result = await _courierService.UpdateServiceAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }


        //================================================= Route
        public async Task<IActionResult> CourierRoutes()
        {
            if (_userContext == null || !_userContext.SellerId.HasValue) return NoContent();
            var model = await _courierService.GetRoutesAsync(_userContext.SellerId.Value);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddRoute()
        {
            if (_userContext == null || !_userContext.SellerId.HasValue) return NoContent();

            ViewBag.Cities = await _geo.SelectItems_CitiesAsync();
            ViewBag.zones = await _pricing.SelectList_ZonesAsync(_userContext.SellerId.Value);
            //ViewBag.Zones = null;
            return PartialView("_AddRoute");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddRoute(RouteDto dto)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _userContext.SellerId.Value;
            if (ModelState.IsValid)
            {
                dto.SellerId = _userContext.SellerId.Value;
                result = await _courierService.AddRouteAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRoute(int id)
        {
            var model = await _courierService.FindRouteByIdAsync(id);
            ViewBag.Cities = await _geo.SelectItems_CitiesAsync();
            ViewBag.zones = await _pricing.SelectList_ZonesAsync(_userContext.SellerId.Value);
            // ViewBag.Zones = null;
            return PartialView("_UpdateRoute", model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> UpdateRoute(RouteDto dto)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _userContext.SellerId.Value;
            if (ModelState.IsValid)
            {
                dto.SellerId = _userContext.SellerId.Value;
                result = await _courierService.UpdateRouteAsync(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<ActionResult> BranchServices(Guid id, string name)
        {
            ViewBag.services = await _courierService.SelectList_ServicesAsync(_userContext.SellerId.Value);
            ViewBag.BranchName = name;
            ViewBag.Id = id;
            var model = await _courierService.GetBranchServicesAsync(id);
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddServiceToBranch(int ServiceId, Guid BranchId)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            if (ServiceId == 0)
            {
                result.Message = "سرویس موردنظر را انتخاب کنید";
                return Json(result.ToJsonResult());
            }

            result = await _courierService.AddServiceToBranchAsync(BranchId, ServiceId, _userContext.SellerId.Value);
            if (result.Success)
            {
                result.ShowMessage = true;
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.updateType = 1;

            }
            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<ActionResult> RemoveService(long itemId)
        {
            clsResult result = new clsResult();
            result.Success = false;

            result = await _courierService.RemoveBranchServiceAsync(itemId);
            if (result.Success)
            {
                result.ShowMessage = true;
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.updateType = 1;

            }
            result.Message = "خطا در حذف سرویس";
            return Json(result.ToJsonResult());
        }


    }
}

