using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Treasury.Dto;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Services;

namespace ParcelPro.Areas.Accounting.Controllers
{
    [Authorize]
    [Area("Accounting")]
    public class AssistantsController : Controller
    {
        private protected IAccAsistantsService _assistant;
        private protected ITreBaseService _treBaseService;
        private readonly UserContextService _user;
        private readonly ITreBankImporterService _bankImporter;
        public AssistantsController(IAccAsistantsService assistant,
            UserContextService UserService,
            ITreBankImporterService treBankImporterService,
            ITreBaseService treBaseService)
        {
            _assistant = assistant;
            _user = UserService;
            _bankImporter = treBankImporterService;
            _treBaseService = treBaseService;
        }

        public async Task<IActionResult> BankTransactions(BankReportFilterDto filter)
        {
            if (!_user.SellerId.HasValue || !_user.PeriodId.HasValue) return NoContent();

            BankTransactionViewModel model = new BankTransactionViewModel();
            filter.SellerId = _user.SellerId.Value;
            if (filter.ShowAll)
            {
                filter.HasDoc = null;
                filter.IsChecked = null;
            }
            model.filter = filter;

            if (!string.IsNullOrEmpty(filter?.strFromDate))
                model.filter.FromDate = filter.strFromDate.PersianToLatin();
            if (!string.IsNullOrEmpty(filter?.strToDate))
                model.filter.ToDate = filter.strToDate.PersianToLatin();

            model.transactions = await _bankImporter.GetBankTransactionsAsync(filter);
            ViewBag.BankAccounts = await _treBaseService.SelectList_BankAccountsAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ImportBankTransaction()
        {
            if (!_user.SellerId.HasValue || !_user.PeriodId.HasValue) return NoContent();
            ViewBag.BankAccounts = await _treBaseService.SelectList_BankAccountsAsync();
            ViewBag.Banks = _bankImporter.Select_list_BankReportType();
            BankImporterDto model = new BankImporterDto();
            model.SellerId = _user.SellerId.Value;
            return PartialView("_ImportBankTransaction", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportBankTransaction(BankImporterDto model)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (ModelState.IsValid)
            {
                switch (model.Pattern)
                {
                    case 10:
                        result = await _bankImporter.ImportSamanAsync(model);
                        break;
                    case 20:
                        result = await _bankImporter.ImportTejaratAsync(model);
                        break;
                    case 21:
                        result = await _bankImporter.ImportTejaratInternetBankAsync(model);
                        break;
                    case 30:
                        result = await _bankImporter.ImportMelatAsync(model);
                        break;
                    case 40:
                        result = await _bankImporter.ImportEghtesadNovinAsync(model);
                        break;
                    case 41:
                        result = await _bankImporter.ImportEghtesadInternetBankAsync(model);
                        break;
                    case 50:
                        result = await _bankImporter.ImportKeshavarziAsync(model);
                        break;
                    case 60:
                        result = await _bankImporter.ImportRefahJariAsync(model);
                        break;
                    case 70:
                        result = await _bankImporter.ImportCityBankAsync(model);
                        break;
                    case 80:
                        result = await _bankImporter.ImportPostBankAsync(model);
                        break;
                    case 90:
                        result = await _bankImporter.ImportSaderat_SepehrAsync(model);
                        break;
                    case 91:
                        result = await _bankImporter.ImportSaderatAsync(model);
                        break;
                    case 100:
                        result = await _bankImporter.ImportSepahAsync(model);
                        break;
                    case 200:
                        result = await _bankImporter.ImportBankMeliAsync(model);
                        break;
                    default:
                        break;
                }

                if (result.Success)
                {
                    result.updateType = 1;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
            }

            var errors = ModelState.Values.SelectMany(e => e.Errors).ToList();
            foreach (var e in errors)
            {
                result.Message += "\n" + e.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<IActionResult> saveTransactionAsChecked(List<long> items)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            result = await _assistant.BankTransactionSaveAsCheckedAsync(items);
            if (result.Success)
            {
                result.updateType = 1;
                result.returnUrl = Request.Headers["Referer"].ToString();
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }


        [HttpGet]
        public IActionResult InsertMoadianReport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertMoadianReport(IFormFile file)
        {
            clsResult result = new clsResult();
            result.Success = false;

            if (!_user.SellerId.HasValue || !_user.PeriodId.HasValue) return NoContent();

            var report = await _assistant.ReadMoadianReportFromExcelAsync(file);
            var dataToDoc = await _assistant.PreparingToCreateMoadianDocAsync(report, true, _user.SellerId.Value, _user.PeriodId.Value, User.Identity.Name);
            result = await _assistant.InsertBulkDocsAsync(dataToDoc);
            if (result.Success)
            {
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.updateType = 1;
            }
            return Json(result.ToJsonResult());
        }
    }
}
