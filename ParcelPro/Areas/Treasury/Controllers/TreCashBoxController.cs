using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Treasury.Dto;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Interfaces; // فرض می‌کنیم این اینترفیس شامل IGeneralService است.

namespace ParcelPro.Areas.Treasury.Controllers
{
    [Area("Treasury")]
    [Authorize]
    public class TreCashBoxController : Controller
    {
        private readonly ITreCashBoxService _cashBoxService;
        private readonly ITreasuryGeneralData _treasuryGeneral;
        private readonly ICuBranchService _branchService;
        private readonly IAccCodingService _accCoding;

        private readonly IGeneralService _gs;

        public TreCashBoxController(ITreCashBoxService cashBoxService,
            IGeneralService generalService,
            ITreasuryGeneralData treasuryGeneral,
            ICuBranchService branchService,
            IAccCodingService codingService)
        {
            _cashBoxService = cashBoxService;
            _gs = generalService;
            _treasuryGeneral = treasuryGeneral;
            _branchService = branchService;
            _accCoding = codingService;
        }

        public async Task<IActionResult> Index()
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            if (!sellerId.HasValue)
            {
                // در صورت عدم وجود فروشنده فعال، می‌توانید به صفحه‌ای مناسب هدایت کنید.
                return RedirectToAction("Error", "Home");
            }
            var model = await _cashBoxService.GetCashBoxesAsync(sellerId.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddCashBox()
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            if (!sellerId.HasValue)
                return NoContent();

            ViewBag.Currencies = await _treasuryGeneral.SelectList_CurrenciesAsync();
            ViewBag.Branches = await _branchService.SelectList_BranchesAsync(sellerId.Value);
            ViewBag.Tafsils = await _accCoding.SelectList_TafsilsAsync(sellerId.Value);

            var model = new TreCashBoxDto();
            return PartialView("_AddCashBox", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCashBox(TreCashBoxDto dto)
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            clsResult result = new clsResult();
            if (!sellerId.HasValue)
            {
                result.Message = "فروشنده فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = sellerId.Value;
            if (!string.IsNullOrEmpty(dto.strOpeningDate))
                dto.OpeningDate = dto.strOpeningDate.PersianToLatin();

            if (ModelState.IsValid)
            {
                result = await _cashBoxService.AddCashBoxAsync(dto);
                if (result.Success)
                    result.returnUrl = Request.Headers["Referer"].ToString();
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var er in errors)
            {
                result.Message += "\n " + er.ErrorMessage;
            }
            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> EditCashBox(Guid id)
        {
            var cashBox = await _cashBoxService.GetCashBoxByIdAsync(id);
            if (cashBox == null)
            {
                return NotFound();
            }
            return PartialView("_AddCashBox", cashBox);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCashBox(TreCashBoxDto dto)
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            clsResult result = new clsResult();
            if (!sellerId.HasValue)
            {
                result.Message = "فروشنده فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = sellerId.Value;
            if (!string.IsNullOrEmpty(dto.strOpeningDate))
                dto.OpeningDate = dto.strOpeningDate.PersianToLatin();

            if (ModelState.IsValid)
            {
                result = await _cashBoxService.UpdateCashBoxAsync(dto);
                if (result.Success)
                    result.returnUrl = Request.Headers["Referer"].ToString();
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var er in errors)
            {
                result.Message += "\n " + er.ErrorMessage;
            }
            return Json(result.ToJsonResult());
        }

        //=========================================================================
        //======================== POS ============================================
        // لیست دستگاه‌های پوز
        [HttpGet]
        public async Task<IActionResult> PosList()
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            if (!sellerId.HasValue) return RedirectToAction("Error", "Home");
            var posList = await _cashBoxService.GetPosDevicesAsync(sellerId.Value);
            return View("PosList", posList); // ویوی PosList.cshtml
        }

        // افزودن دستگاه پوز - نمایش فرم
        [HttpGet]
        public async Task<IActionResult> AddPosDevice()
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            if (!sellerId.HasValue) return NoContent();

            // تنظیم داده‌های مورد نیاز برای انتخاب از لیست‌ها
            ViewBag.BankAccounts = await _treasuryGeneral.SelectList_BankAccountsAsync(sellerId.Value);
            ViewBag.Branches = await _branchService.SelectList_BranchesAsync(sellerId.Value);
            ViewBag.Currencies = await _treasuryGeneral.SelectList_CurrenciesAsync();
            ViewBag.SellerId = sellerId.Value;

            return PartialView("_AddPosDevice");
        }

        // افزودن دستگاه پوز - دریافت اطلاعات فرم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPosDevice(TreBankPosUcDto dto)
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            clsResult result = new clsResult();
            if (!sellerId.HasValue)
            {
                result.Message = "فروشنده فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = sellerId.Value;
            if (ModelState.IsValid)
            {
                result = await _cashBoxService.AddPosDeviceAsync(dto);
                if (result.Success)
                    result.returnUrl = Request.Headers["Referer"].ToString();
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var er in errors)
            {
                result.Message += "\n " + er.ErrorMessage;
            }
            return Json(result.ToJsonResult());
        }

        // ویرایش دستگاه پوز - نمایش فرم
        [HttpGet]
        public async Task<IActionResult> EditPosDevice(int id)
        {
            var posDto = await _cashBoxService.GetPosDeviceByIdAsync(id);
            if (posDto == null) return NotFound();
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            ViewBag.BankAccounts = await _treasuryGeneral.SelectList_BankAccountsAsync(sellerId.Value);
            ViewBag.Branches = await _branchService.SelectList_BranchesAsync(sellerId.Value);
            ViewBag.Currencies = await _treasuryGeneral.SelectList_CurrenciesAsync();
            return PartialView("_AddPosDevice", posDto);
        }

        // ویرایش دستگاه پوز - دریافت اطلاعات فرم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPosDevice(TreBankPosUcDto dto)
        {
            long? sellerId = await _gs.GetActiveSellerIdAsync(User.Identity.Name);
            clsResult result = new clsResult();
            if (!sellerId.HasValue)
            {
                result.Message = "فروشنده فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = sellerId.Value;
            if (ModelState.IsValid)
            {
                result = await _cashBoxService.UpdatePosDeviceAsync(dto);
                if (result.Success)
                    result.returnUrl = Request.Headers["Referer"].ToString();
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var er in errors)
            {
                result.Message += "\n " + er.ErrorMessage;
            }
            return Json(result.ToJsonResult());
        }

        // حذف دستگاه پوز - نمایش تایید حذف
        [HttpGet]
        public async Task<IActionResult> DeletePosDevice(int id)
        {
            ViewBag.PosId = id;
            return PartialView("_DeletePosDevice");
        }

        // حذف دستگاه پوز - دریافت تایید حذف
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePosDeviceConfirmed(int id)
        {
            clsResult result = await _cashBoxService.DeletePosDeviceAsync(id);
            if (result.Success)
            {
                result.returnUrl = Request.Headers["Referer"].ToString();
            }
            return Json(result.ToJsonResult());
        }

    }
}
