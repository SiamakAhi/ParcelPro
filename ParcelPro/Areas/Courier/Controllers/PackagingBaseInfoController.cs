using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Warehouse.WarehouseInterfaces;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize]
    public class PackagingBaseInfoController : Controller
    {
        private readonly ICourierServiceService _packagingService;
        private readonly IWhProductService _productService;
        private readonly UserContextService _userContext;

        public PackagingBaseInfoController(ICourierServiceService CourierService, IWhProductService ProductService, UserContextService userContextService)
        {
            _packagingService = CourierService;
            _productService = ProductService;
            _userContext = userContextService;
        }

        public async Task<IActionResult> Packagings()
        {
            if (_userContext == null || !_userContext.SellerId.HasValue) return NoContent();
            var model = await _packagingService.GetPackagingsAsync(_userContext.SellerId.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddPackaging()
        {
            if (_userContext == null || !_userContext.SellerId.HasValue) return NoContent();

            ViewBag.WarehouseCategories = await _productService.SelectList_CategoriesFullnameAsync(_userContext.SellerId.Value);
            return PartialView("_AddPackaging");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddPackaging(PackagingDto dto)
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
                result = await _packagingService.AddPackagingAsync(dto);
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

        [HttpGet]
        public async Task<IActionResult> UpdatePackaging(int id)
        {
            var model = await _packagingService.GetPackagingDtoAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.WarehouseCategories = await _productService.SelectList_CategoriesFullnameAsync(_userContext.SellerId.Value);
            return PartialView("_UpdatePackaging", model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> UpdatePackaging(PackagingDto dto)
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
                result = await _packagingService.UpdatePackagingAsync(dto);
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

        [HttpPost]
        public async Task<ActionResult> DeletePackaging(int id)
        {
            clsResult result = new clsResult();
            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }

            result = await _packagingService.DeletePackagingAsync(id);
            if (result.Success)
            {
                result.ShowMessage = true;
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.updateType = 1;
            }
            return Json(result.ToJsonResult());
        }
    }
}
