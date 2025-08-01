using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.Commercial.ComercialInterfaces;
using ParcelPro.Areas.Commercial.Dtos;
using ParcelPro.Interfaces;
using ParcelPro.Interfaces.CommercialInterfaces;
using ParcelPro.Services;

namespace ParcelPro.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    [Authorize]
    public class DocCreatorController : Controller
    {
        private readonly IAccImportService _importService;
        private readonly IGeneralService _gs;
        private readonly IAccSettingService _sett;
        private readonly IInvoiceService _saleService;
        private readonly UserContextService _userContext;
        private readonly IBuyerService _buyerService;
        private readonly IPersonService _person;
        private readonly IAccDocCreatorService _docCreator;
        private readonly IAccCodingService _coding;

        public DocCreatorController(IAccImportService service
            , IGeneralService gs
            , IAccSettingService accSettingService
            , IInvoiceService saleService
            , UserContextService userContext
            , IBuyerService buyerService
            , IPersonService person)
        {
            _importService = service;
            _gs = gs;
            _sett = accSettingService;
            _saleService = saleService;
            _userContext = userContext;
            _buyerService = buyerService;
            _person = person;
        }

        public async Task<IActionResult> GetData(InvoiceFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NotFound();
            VmInvoices model = new VmInvoices();
            model.filter = new InvoiceFilterDto();
            if (filter.Invoicetype != null && filter.Invoicetype != 0)
            {
                filter.SellerId = _userContext.SellerId.Value;
                filter.PeriodId = _userContext.PeriodId;
                filter.Invoicetype = filter.Invoicetype;


                model.filter = filter;

                var invoices = _saleService.GetInvoices(filter);
                var paginatedInvoices = Pagination<InvoiceHeaderDto>.Create(invoices, filter.CurrentPage, filter.PageSize);
                model.Invoices = paginatedInvoices;
            }

            ViewBag.buyers = await _buyerService.SelectList_Buyers(_userContext.SellerId.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatetransactionDoc(List<long> items, int transactionsType, int BankId)
        {
            if (!_userContext.SellerId.HasValue || !_userContext.PeriodId.HasValue) return NoContent();

            BankTransactionsCreateDocDto model = new BankTransactionsCreateDocDto();
            model.SellerId = _userContext.SellerId.Value;
            model.PeriodId = _userContext.PeriodId.Value;
            model.TransactionsId = items;
            model.TransactionsType = transactionsType;
            model.BankAccountId = BankId;

            ViewBag.Moeins = await _coding.SelectList_MoeinsAsync(model.SellerId);
            ViewBag.Tafsils = await _coding.SelectList_TafsilsAsync(model.SellerId);

            return PartialView("_CreatetransactionDoc", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatetBankReportDoc(BankTransactionsCreateDocDto model)
        {
            if (!_userContext.SellerId.HasValue || !_userContext.PeriodId.HasValue) return NoContent();

            clsResult result = new clsResult();
            result.Success = false;

            if (ModelState.IsValid)
            {
                result = await _docCreator.CreateBankDocAsync(model);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.updateType = 1;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
            }

            var errors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in errors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }
    }
}
