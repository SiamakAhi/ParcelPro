using ParcelPro.Areas.Commercial.ComercialInterfaces;
using ParcelPro.Areas.Commercial.Dtos;
using ParcelPro.Areas.Warehouse.Dto;
using ParcelPro.Areas.Warehouse.WarehouseInterfaces;
using ParcelPro.Classes;
using ParcelPro.Interfaces;
using ParcelPro.Interfaces.CommercialInterfaces;
using ParcelPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ParcelPro.Areas.Commercial.Controllers
{
    [Area("Commercial")]
    [Authorize]
    public class saleController : Controller
    {
        private readonly IInvoiceService _saleService;
        private readonly UserContextService _userContext;
        private readonly IBuyerService _buyerService;
        private readonly IWhProductService _product;
        private readonly IPersonService _person;
        private readonly IGeneralService _gs;
        public saleController(IInvoiceService saleInvoiceService
            , UserContextService userContextService
            , IBuyerService buyerService
            , IWhProductService whProductService
            , IPersonService personService,
              IGeneralService gs)
        {
            _saleService = saleInvoiceService;
            _userContext = userContextService;
            _buyerService = buyerService;
            _product = whProductService;
            _person = personService;
            _gs = gs;
        }

        public async Task<IActionResult> saleInvoices(InvoiceFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NotFound();
            filter.SellerId = _userContext.SellerId.Value;
            filter.PeriodId = _userContext.PeriodId;
            filter.Invoicetype = 2;

            VmInvoices model = new VmInvoices();
            model.filter = filter;

            var invoices = _saleService.GetInvoices(filter);
            var paginatedInvoices = Pagination<InvoiceHeaderDto>.Create(invoices, filter.CurrentPage, filter.PageSize);
            model.Invoices = paginatedInvoices;
            ViewBag.buyers = await _buyerService.SelectList_Buyers(_userContext.SellerId.Value);
            ////Total
            //ViewBag.totalTaxable = await invoices.SumAsync(n => n.TotalTaxable);
            //ViewBag.totalNoTaxable = await invoices.SumAsync(n => n.TotalNoTaxable);
            //ViewBag.totalDiscount = await invoices.SumAsync(n => n.TotalDiscount);
            //ViewBag.totalAfterDiscount = await invoices.SumAsync(n => n.TotalPriceAfterDiscount);
            //ViewBag.totalVat = await invoices.SumAsync(n => n.TotalVatPrice);
            //ViewBag.totalFinalPrice = await invoices.SumAsync(n => n.TotalFinalPrice);

            return View(model);
        }

        public async Task<IActionResult> saleReports(InvoiceFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NotFound();
            filter.SellerId = _userContext.SellerId.Value;
            filter.PeriodId = _userContext.PeriodId;
            filter.Invoicetype = 2;

            VmInvoices model = new VmInvoices();
            model.filter = filter;

            var invoices = await _saleService.GetInvoicesGroupedByCustomer(filter);
            var paginatedInvoices = Pagination<InvoiceHeaderDto>.Create(invoices.AsQueryable(), filter.CurrentPage, filter.PageSize);
            model.Invoices = paginatedInvoices;
            ViewBag.buyers = await _buyerService.SelectList_Buyers(_userContext.SellerId.Value);
            return View(model);
        }

        [HttpGet]
        public IActionResult comSaleInvoices(InvoiceFilterDto filter)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateSaleInvoice()
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();
            ViewBag.InvoiceNumber = await _saleService.GenerateSaleInvoiceNumberAsync(_userContext.SellerId.Value);
            ViewBag.Buyers = await _buyerService.SelectList_Buyers(_userContext.SellerId.Value);
            ViewBag.SettelmentType = _saleService.SelectList_SettelmentType();
            return PartialView("_CreateSaleInvoice");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSaleInvoice(InvoiceHeaderDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (_userContext.SellerId == null || _userContext.PeriodId == null)
            {
                result.Message = "شرکت فعال و یا سال مالی انتخاب نشده است.";
                return Json(result.ToJsonResult());
            }
            dto.SellerId = _userContext.SellerId.Value;
            dto.FinancePeriodId = _userContext.PeriodId;
            dto.InvoiceSubject = 1;
            dto.CreatorUserId = User.Identity.Name;
            dto.InvoiceDate = dto.strInvoiceDate.PersianToLatin();

            if (ModelState.IsValid)
            {
                result = await _saleService.CreateInvoiceHeaderAsync(dto);

                if (result.Success)
                {
                    result.ShowMessage = false;
                    result.updateType = 1;
                    result.returnUrl = Url.Action("invoice", "sale", new { Area = "Commercial", id = dto.Id });
                    return Json(result.ToJsonResult());
                }
            }

            // در صورت وجود خطا در ModelState
            var modelErrors = ModelState.Values.SelectMany(e => e.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message += "<br>" + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        public async Task<IActionResult> invoice(Guid id)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();
            var invoice = await _saleService.GetInvoiceByIdAsync(id);

            ViewBag.Buyers = await _buyerService.SelectList_Buyers(_userContext.SellerId.Value);
            ViewBag.SettelmentType = _saleService.SelectList_SettelmentType();
            ViewBag.products = await _product.SelectList_ProductsAsync(new ProductFilter() { SellerId = _userContext.SellerId.Value });

            return View(invoice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHeaderInvoice(InvoiceDto model)
        {
            clsResult result = new clsResult();
            result.Success = false;
            if (!_userContext.SellerId.HasValue) return NoContent();
            model.InvoiceHeader.EditorUserId = User.Identity.Name;
            model.InvoiceHeader.LastUpdate = DateTime.Now;
            model.InvoiceHeader.SellerId = _userContext.SellerId.Value;

            result = await _saleService.UpdateInvoiceHeaderAsync(model.InvoiceHeader);
            if (result.Success)
            {
                result.ShowMessage = false;
                result.updateType = 1;
                result.returnUrl = Url.Action("invoice", "sale", new { Area = "Commercial", id = model.InvoiceHeader.Id });
                return Json(result.ToJsonResult());

            }

            return Json(result.ToJsonResult());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addInvoiceItem(InvoiceDto model)
        {
            clsResult result = new clsResult();
            result.Success = false;
            if (!_userContext.SellerId.HasValue) return NoContent();
            model.dto.CreatorUserId = User.Identity.Name;
            model.dto.CreationTime = DateTime.Now;

            result = await _saleService.AddInvoiceItemAsync(model.dto);
            if (result.Success)
            {
                result.ShowMessage = false;
                result.updateType = 1;
                result.returnUrl = Url.Action("invoice", "sale", new { Area = "Commercial", id = model.dto.InvoiceId });
                return Json(result.ToJsonResult());

            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> EditInvoiceItem(Guid id)
        {

            var item = await _saleService.GetInvoiceItemByIdAsync(id);
            if (item == null || !_userContext.SellerId.HasValue) return NoContent();
            ViewBag.products = await _product.SelectList_ProductsAsync(new ProductFilter() { SellerId = _userContext.SellerId.Value });

            return PartialView("_EditInvoiceItem", item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInvoiceItem(InvoiceItemDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            if (!_userContext.SellerId.HasValue) return NoContent();
            dto.EditorUserId = User.Identity.Name;
            dto.LastUpdate = DateTime.Now;

            result = await _saleService.updateInvoiceItemAsync(dto);
            if (result.Success)
            {
                result.ShowMessage = false;
                result.updateType = 1;
                result.returnUrl = Url.Action("invoice", "sale", new { Area = "Commercial", id = dto.InvoiceId });
                return Json(result.ToJsonResult());

            }

            return Json(result.ToJsonResult());
        }

        //
        [HttpPost]
        public async Task<IActionResult> DeleteInvoiceItem(Guid itemId)
        {
            clsResult result = new clsResult();
            result.Success = true;
            result.updateType = 1;
            result.ShowMessage = true;

            if (_userContext.SellerId == null || _userContext.PeriodId == null)
            {
                result.Message = "شرکت فعال و یا سال مالی انتخاب نشده است.";
                return Json(result.ToJsonResult());
            }

            result = await _saleService.DeleteInvoiceItemAsync(itemId);

            if (result.Success)
            {
                result.ShowMessage = false;
                result.returnUrl = Request.Headers["Referer"].ToString();
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }
        [HttpPost]
        public async Task<IActionResult> DeleteInvoice(Guid itemId)
        {
            clsResult result = new clsResult();
            result.Success = true;
            result.updateType = 1;
            result.ShowMessage = true;

            if (_userContext.SellerId == null || _userContext.PeriodId == null)
            {
                result.Message = "شرکت فعال و یا سال مالی انتخاب نشده است.";
                return Json(result.ToJsonResult());
            }

            result = await _saleService.DeleteInvoiceAsync(itemId);

            if (result.Success)
            {
                result.ShowMessage = false;
                result.returnUrl = Request.Headers["Referer"].ToString();
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }


        [HttpGet]
        public async Task<IActionResult> GetProductInfo(long id)
        {
            var product = await _product.GetProductByIdAsync(id);
            if (product == null)
                return NoContent();
            return Json(product);
        }

        [HttpGet]
        public IActionResult bulkCreateInvoice()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> bulkCreateInvoice(IFormFile ExcelFile)
        {
            clsResult result = new clsResult();
            result.Success = false;
            ExcelImporter importer = new ExcelImporter();
            var data = importer.ReadInvoicesFromAtiranExcel(ExcelFile);
            if (data.Errors.Count > 0)
            {
                foreach (var er in data.Errors)
                {
                    result.Message += $"\n {er.Code} - {er.Error}";
                }
                result.Success = false;
                result.ShowMessage = true;
                return Json(result.ToJsonResult());
            }

            var dataToAdd = await _saleService.PrepareInvoiceToCreate_AtiranAsync(data);
            result = await _saleService.CreateInvoiceInBulkAsync(dataToAdd);
            if (result.Success)
            {
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.Success = true;
            }

            return Json(result.ToJsonResult());
        }
        public async Task<IActionResult> print_Invoice(Guid id)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();
            var invoice = await _saleService.GetInvoiceByIdAsync(id);
            invoice.buyerInfo = await _person.GetPersonDtoAsync(invoice.InvoiceHeader.PartyId);
            invoice.sellerInfo = await _person.GetPersonDtoAsync(invoice.InvoiceHeader.SellerId.Value);
            return View(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> print_Invoices(Guid[] items)
        {
            var invoices = await _saleService.GetInvoicesFuulDataAsync(items);
            return View(invoices);
        }

        public async Task<IActionResult> print_saleInvoices(InvoiceFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NotFound();
            filter.SellerId = _userContext.SellerId.Value;
            filter.PeriodId = _userContext.PeriodId;

            VmInvoices model = new VmInvoices();
            model.filter = filter;

            var invoices = _saleService.GetInvoices(filter);
            var paginatedInvoices = Pagination<InvoiceHeaderDto>.Create(invoices, filter.CurrentPage, filter.PageSize);
            model.Invoices = paginatedInvoices;

            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);

            string startDate = !string.IsNullOrEmpty(filter.srtFromDate) ? filter.srtFromDate.PersianToLatin().LatinToPersian() : " ابتدای دوره ";
            string endDate = !string.IsNullOrEmpty(filter.srtToDate) ? filter.srtToDate.PersianToLatin().LatinToPersian() : " ...";

            ViewBag.ReportDate = $" از {startDate} لغایت {endDate}";
            ViewBag.SellerName = userSett.ActiveSellerName;
            ViewBag.PeriodName = userSett.ActiveSellerPeriodName;
            ViewBag.ReportTitle = "گزارش فروش";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TagInvoices(Guid[] items)
        {
            var result = await _saleService.TagInvoicesAsync(items);
            if (result.Success)
            {
                result.updateType = 1;
                result.returnUrl = Request.Headers["Referer"].ToString();
            }
            return Json(result.ToJsonResult());
        }
        [HttpPost]
        public async Task<IActionResult> UnTagInvoices(Guid[] items)
        {
            var result = await _saleService.UnTagInvoicesAsync(items);
            if (result.Success)
            {
                result.updateType = 1;
                result.returnUrl = Request.Headers["Referer"].ToString();
            }
            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<IActionResult> PrintSelectedInvoice(Guid[] items)
        {
            if (!_userContext.SellerId.HasValue)
                return NotFound();
            var model = await _saleService.GetSelectedInvoicesAsync(items);

            var userSett = await _gs.GetUserSettingAsync(User.Identity.Name);
            ViewBag.SellerName = userSett.ActiveSellerName;
            ViewBag.PeriodName = userSett.ActiveSellerPeriodName;
            ViewBag.ReportTitle = "گزارش فروش";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TagInvoice(Guid id)
        {
            var result = await _saleService.TagTogglerInvoicesAsync(id);
            if (result.Success)
            {
                result.updateType = 1;
                result.returnUrl = Request.Headers["Referer"].ToString();
            }
            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public IActionResult CopySelectedInvoices(Guid[] items)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();

            CoppyInvoiceSettingDto model = new CoppyInvoiceSettingDto();
            model.InvoicesId = items.ToList();
            model.SellerId = _userContext.SellerId.Value;
            model.PeriodId = _userContext.PeriodId.Value;

            return PartialView("_CopySelectedInvoices", model);
        }
        [HttpPost]
        public async Task<IActionResult> deleteDuplacated(Guid[] items)
        {
            var result = await _saleService.DeleteDuplacatedInvoicesAsync(items);

            if (result.Success)
            {
                result.updateType = 1;
                result.returnUrl = Request.Headers["Referer"].ToString();
            }
            return Json(result.ToJsonResult());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSelectedInvoices(CoppyInvoiceSettingDto model)
        {
            clsResult result = new clsResult();
            if (model.InvoicesId.Count == 0)
            {
                result.Message = "اطلاعاتی برای کپی کردن دریافت نشد";
                result.ShowMessage = true;
                return Json(result.ToJsonResult());
            }

            result = await _saleService.CopyInvoicesAsync(model);
            result.updateType = 1;
            result.returnUrl = Request.Headers["Referer"].ToString();

            return Json(result.ToJsonResult());
        }
    }
}
