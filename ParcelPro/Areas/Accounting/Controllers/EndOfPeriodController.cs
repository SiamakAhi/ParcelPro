using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ParcelPro.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class EndOfPeriodController : Controller
    {
        private readonly IAccEndOfPeriodService _serv;
        private readonly IAccProfitMasterService _profit;
        private readonly IGeneralService _gs;
        private readonly IAccOperationService _op;

        public EndOfPeriodController(IAccEndOfPeriodService endOfPeriodService
            , IGeneralService generalService
            , IAccOperationService Op
            , IAccProfitMasterService profit)
        {
            _serv = endOfPeriodService;
            _gs = generalService;
            _op = Op;
            _profit = profit;
        }

        public async Task<IActionResult> CloseTemporaryAccounts()
        {
            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
                ViewBag.Allert = "شرکت یا سال مالی فعال شناسایی نشد";

            long sellerId = userSett.ActiveSellerId.Value;
            ViewBag.TemporaryAccounts = await _serv.SelectList_GroupAccountsAsync(sellerId, 2);
            ViewBag.PermanentAccounts = await _serv.SelectList_PermanentAccounts_MoeinAsync(sellerId);
            EndOfPeriodDto dto = new EndOfPeriodDto();
            dto.AccountsSetting = new EndOfPeriodSettings();
            dto.AccountsSetting.SellerId = sellerId;
            dto.AccountsSetting.PeriodId = userSett.ActiveSellerPeriod;
            dto.AccountsSetting.CurrentUser = userSett.UserName;

            dto.Articles = new List<DocArticleDto>();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseTemporaryAccounts(EndOfPeriodDto dto)
        {
            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
                return BadRequest(ModelState);

            if (ModelState.IsValid)
            {
                dto.AccountsSetting.SellerId = userSett.ActiveSellerId.Value;
                dto.AccountsSetting.PeriodId = userSett.ActiveSellerPeriod.Value;
                dto.AccountsSetting.CurrentUser = userSett.UserName;
                dto.AccountsSetting.payanDore = Convert.ToInt64(dto.AccountsSetting.strPayanDore.Replace(",", ""));
                dto.Articles = await _serv.CloseTemporaryPreviewAsync(dto.AccountsSetting);
            }

            ViewBag.TemporaryAccounts = await _serv.SelectList_GroupAccountsAsync(dto.AccountsSetting.SellerId, 2);
            ViewBag.PermanentAccounts = await _serv.SelectList_PermanentAccounts_MoeinAsync(dto.AccountsSetting.SellerId);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDocCloseTemporaryAccounts(EndOfPeriodDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
            {
                result.Message = "شرکت یا سال مالی فعال شناسایی نشد";
                return Json(result.ToJsonResult());
            }

            if (dto.AccountsSetting.SummaryAccountId == null)
            {
                result.Message = "حساب خلاصه سود و زیان را مشخص کنید.";
                return Json(result.ToJsonResult());
            }

            if (ModelState.IsValid)
            {
                dto.AccountsSetting.SellerId = userSett.ActiveSellerId.Value;
                dto.AccountsSetting.PeriodId = userSett.ActiveSellerPeriod.Value;
                dto.AccountsSetting.CurrentUser = userSett.UserName;
                dto.AccountsSetting.payanDore = Convert.ToInt64(dto.AccountsSetting.strPayanDore.Replace(",", ""));
                var Articles = await _serv.CloseTemporaryPreviewAsync(dto.AccountsSetting);
                if (Articles.Count > 0)
                {
                    result = await _op.InsertSystemicDocAsync(Articles, " بستن حساب های موقت", 4);
                    if (result.Success)
                    {
                        result.returnUrl = Url.Action("Doc", "OpAccoucnting", new { Area = "", id = Articles.FirstOrDefault().DocId });
                        return Json(result.ToJsonResult());
                    }
                }
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> ClosePermanentAccounts()
        {
            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
                ViewBag.Allert = "شرکت یا سال مالی فعال شناسایی نشد";

            long sellerId = userSett.ActiveSellerId.Value;
            ViewBag.AllAccounts = await _serv.SelectList_AllAccounts_MoeinAsync(sellerId);
            EndOfPeriodDto dto = new EndOfPeriodDto();
            dto.Articles = new List<DocArticleDto>();
            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClosePermanentAccountsPreview(EndOfPeriodDto dto)
        {
            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
                return BadRequest(ModelState);

            if (ModelState.IsValid)
            {
                dto.AccountsSetting.SellerId = userSett.ActiveSellerId.Value;
                dto.AccountsSetting.PeriodId = userSett.ActiveSellerPeriod.Value;
                dto.AccountsSetting.CurrentUser = userSett.UserName;

                dto.Articles = await _serv.ClosePermanentAccountsPreviewAsync(dto.AccountsSetting);
            }

            ViewBag.AllAccounts = await _serv.SelectList_AllAccounts_MoeinAsync(dto.AccountsSetting.SellerId);

            return View("ClosePermanentAccounts", dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDocClosePermanentAccounts(EndOfPeriodDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;
            result.updateType = 1;

            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
            {
                result.Message = "شرکت یا سال مالی فعال شناسایی نشد";
                return Json(result.ToJsonResult());
            }

            dto.AccountsSetting.SellerId = userSett.ActiveSellerId.Value;
            dto.AccountsSetting.PeriodId = userSett.ActiveSellerPeriod.Value;
            dto.AccountsSetting.CurrentUser = userSett.UserName;
            result = await _serv.ClosePermanentAccountsAsync(dto.AccountsSetting);
            if (result.Success)
            {
                result.ShowMessage = true;
                result.returnUrl = Url.Action("AccountingDocs", "OpAccoucnting", new { Area = "" });
                result.ShowMessage = true;
                result.updateType = 1;
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }
        [HttpGet]
        public async Task<IActionResult> CalcProfit()
        {
            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
                ViewBag.Allert = "شرکت یا سال مالی فعال شناسایی نشد";

            long sellerId = userSett.ActiveSellerId.Value;
            ViewBag.TemporaryKolAccounts = await _serv.SelectList_TemporaryAccounts_KolAsync(sellerId);
            ViewBag.TemporaryGroups = await _serv.SelectList_GroupAccountsAsync(sellerId, 2);
            ViewBag.PermanentAccountsMoein = await _serv.SelectList_PermanentAccounts_MoeinAsync(sellerId);
            VmProfit model = new VmProfit();
            model.ProfitReport = new List<ProfitMasterDto>();
            ViewBag.ReportTitle = "گزارش سود و زیان";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalcProfit(VmProfit model)
        {
            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
                return BadRequest(ModelState);

            model.AccountsSetting.SellerId = userSett.ActiveSellerId.Value;
            model.AccountsSetting.PeriodId = userSett.ActiveSellerPeriod.Value;
            if (model.AccountsSetting.payanDore_calcSystemic)
                model.AccountsSetting.PayanDore = null;
            else
                model.AccountsSetting.PayanDore = Convert.ToInt64(model.AccountsSetting.strPayanDore.Replace(",", ""));

            model.ProfitReport = await _profit.ProfitReportAsync(model.AccountsSetting);

            ViewBag.TemporaryKolAccounts = await _serv.SelectList_TemporaryAccounts_KolAsync(model.AccountsSetting.SellerId);
            ViewBag.TemporaryGroups = await _serv.SelectList_GroupAccountsAsync(model.AccountsSetting.SellerId, 2);
            ViewBag.PermanentAccountsMoein = await _serv.SelectList_PermanentAccounts_MoeinAsync(model.AccountsSetting.SellerId);
            string reportTitle = "گزارش سود و زیان از ";
            string reportDate = " ابتدای دوره";
            if (!string.IsNullOrEmpty(model.AccountsSetting.startDate))
                reportDate = model.AccountsSetting.startDate.PersianToLatin().LatinToPersian();
            if (!string.IsNullOrEmpty(model.AccountsSetting.endDate))
                reportDate += $" لغایت {model.AccountsSetting.endDate.PersianToLatin().LatinToPersian()}";
            ViewBag.ReportTitle = reportTitle + reportDate;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PrintProfit(VmProfit model)
        {
            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            if (userSett == null || userSett.ActiveSellerId == null || userSett.ActiveSellerPeriod == null)
                return BadRequest(ModelState);

            model.AccountsSetting.SellerId = userSett.ActiveSellerId.Value;
            model.AccountsSetting.PeriodId = userSett.ActiveSellerPeriod.Value;
            if (model.AccountsSetting.payanDore_calcSystemic)
                model.AccountsSetting.PayanDore = null;
            else
                model.AccountsSetting.PayanDore = Convert.ToInt64(model.AccountsSetting.strPayanDore.Replace(",", ""));

            model.ProfitReport = await _profit.ProfitReportAsync(model.AccountsSetting);

            ViewBag.ReportDate = DateTime.Now.LatinToPersian();
            ViewBag.SellerName = userSett.ActiveSellerName;
            ViewBag.PeriodName = userSett.ActiveSellerPeriodName;
            string reportTitle = "گزارش سود و زیان از ";
            string reportDate = " ابتدای دوره";
            if (!string.IsNullOrEmpty(model.AccountsSetting.startDate))
                reportDate = model.AccountsSetting.startDate.PersianToLatin().LatinToPersian();
            if (!string.IsNullOrEmpty(model.AccountsSetting.endDate))
                reportDate += $" لغایت {model.AccountsSetting.endDate.PersianToLatin().LatinToPersian()}";
            ViewBag.ReportTitle = reportTitle + reportDate;
            return View(model);
        }

    }
}
