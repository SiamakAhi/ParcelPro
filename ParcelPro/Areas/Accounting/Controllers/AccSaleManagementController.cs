using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.Dto.SaleManagementDtos;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Geolocation.GeolocationInterfaces;
using ParcelPro.Areas.Representatives.Dtos;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Services;
using ParcelPro.ViewModels;
using ParcelPro.ViewModels.PartyDto;

namespace ParcelPro.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    [Authorize(Roles = "AccountingManager, AccountingBoss, BranchManager")]
    public class AccSaleManagementController : Controller
    {
        private readonly ICourierFinancialService _courierFinancialService;
        private readonly UserContextService _userContext;
        private readonly IPersonService _persen;
        private readonly ICourierServiceService _courier;
        private readonly IBillofladingService _bill;
        private readonly ITreasuryGeneralData _treasuryGeneralData;
        private readonly IGeoGeneralService _geo;

        public AccSaleManagementController(ICourierFinancialService courierFinancialService
                , UserContextService userContext
                , IPersonService person
                , ICourierServiceService courier
                , IBillofladingService bill
                , ITreasuryGeneralData treasuryGeneralData
            , IGeoGeneralService geoLocationService)
        {
            _courierFinancialService = courierFinancialService;
            _userContext = userContext;
            _persen = person;
            _courier = courier;
            _bill = bill;
            _treasuryGeneralData = treasuryGeneralData;
            _geo = geoLocationService;
        }

        public async Task<IActionResult> SaleAccounting(AccSalesFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();

            var model = new SaleManagementViewModel();
            filter.SellerId = _userContext.SellerId.Value;
            model.filter = filter;

            ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            ViewBag.Issuers = await _bill.SelectList_IssuersUsersAsync(_userContext.SellerId.Value);
            ViewBag.Cities = await _geo.SelectItems_CitiesAsync();

            var Allbills = _courierFinancialService.GetBillsFinanceAsQuery(filter);
            model.BillsPagin = Pagination<AccBillViewModel>.Create(Allbills, filter.CurrentPage, filter.PageSize);
            return View(model);
        }

        public async Task<IActionResult> HandleCreditAccounts(AccSalesFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();

            var model = new SaleManagementViewModel();
            filter.SellerId = _userContext.SellerId.Value;

            model.filter = filter;
            ViewBag.persen = await _courierFinancialService.SelectList_CreditCustomersAsync(_userContext.SellerId.Value);
            model.CreditSaleCurrentDebt = await _courierFinancialService.FetchCurrentDebtStatusAsync(filter);
            return View(model);
        }
        public async Task<IActionResult> MoneyTransactionsManagement(SaleMoneyTransactionFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();

            var model = new SaleMoneyTransactionVM();
            model.filter = filter;
            model.filter.SellerId = _userContext.SellerId.Value;
            var DataQuery = _courierFinancialService.GetSaleMoneyTransactionsAsync(filter);
            model.Transactions = Pagination<SaleMoneyTransaction>.Create(DataQuery, filter.CurrentPage, filter.PageSize);

            ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            ViewBag.PaymentMethod = await _treasuryGeneralData.SelectList_PaymentMethodsAsync();
            ViewBag.BankAccounts = await _treasuryGeneralData.SelectList_BankAccountsAsync(_userContext.SellerId.Value);
            ViewBag.Poses = await _treasuryGeneralData.SelectList_PosDevicesAsync(_userContext.SellerId.Value);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MoveAccounts()
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();

            ViewBag.persen = await _courierFinancialService.SelectList_CreditCustomersAsync(_userContext.SellerId.Value);
            ViewBag.Clients = await _persen.SelectList_CreditClient(_userContext.SellerId.Value);
            return PartialView("_MoveAccounts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveAccounts(MoveAccountDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "خطا در شناسایی لایسنس";
            }
            result = await _courierFinancialService.MoveToAnotherAccountAsync(dto);
            if (result.Success)
            {
                result.ShowMessage = true;
                result.updateType = 1;
                result.returnUrl = Request.Headers["Referer"].ToString();
            }

            return Json(result.ToJsonResult());

        }

        public async Task<IActionResult> thePartyBills(long id)
        {

            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            var model = new PartyBillsViewModel();
            model.filter = new PartyBillsFilterDto
            {
                IsPayed = false,
                PartyId = id,
            };

            model.PartyInfo = await _persen.GetPersonDtoAsync(model.filter.PartyId.Value);
            model.Bills = await _courierFinancialService.GetPartyBillsAsync(model.filter);

            ViewBag.Parties = await _courierFinancialService.SelectList_CreditCustomersAsync(_userContext.SellerId.Value);

            return View("PartyBills", model);

        }

        public async Task<IActionResult> PartyBills(PartyBillsFilterDto filter)
        {

            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            ViewBag.Parties = await _courierFinancialService.SelectList_CreditCustomersAsync(_userContext.SellerId.Value);
            var model = new PartyBillsViewModel();
            if (!filter.PartyId.HasValue)
                return View(model);

            model.PartyInfo = await _persen.GetPersonDtoAsync(filter.PartyId.Value);
            model.Bills = await _courierFinancialService.GetPartyBillsAsync(filter);
            model.filter = filter;

            return View(model);

        }

        //SaleByClient
        public async Task<IActionResult> SaleByClient(PartyBillsFilterDto filter)
        {

            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            ViewBag.Parties = await _persen.SelectList_PersenAsync(_userContext.SellerId.Value);
            var model = new PartyBillsViewModel();
            if (!filter.PartyId.HasValue)
                return View(model);

            model.PartyInfo = await _persen.GetPersonDtoAsync(filter.PartyId.Value);
            model.Bills = await _courierFinancialService.GetPartyBillsAsync(filter);
            model.filter = filter;

            return View(model);

        }

        //CreditClients
        public async Task<IActionResult> CreditClientsList(PersonFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();
            filter.IsCreditClient = true;
            var model = new MembersViewmodel();
            model.filter = filter;
            model.filter.IsCreditClient = true;
            model.filter.SellerId = _userContext.SellerId.Value;
            var data = _persen.PersenAsQuery(filter);
            model.Persen = Pagination<PersonDto>.Create(data, model.filter.CurrentPage, model.filter.PageSize);

            ViewBag.Persen = await _persen.SelectList_PersenAsync(_userContext.SellerId.Value);
            ViewBag.Clients = await _persen.SelectList_CreditClient(_userContext.SellerId.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExportMoadianBill(PartyBillsFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            var bills = await _courierFinancialService.CreateMoadiansAsync(filter);
            var party = bills.FirstOrDefault();
            if (bills == null || bills.Count == 0)
                return NotFound("صورتحسابی برای خروجی یافت نشد.");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("صورتحساب‌ها");

            worksheet.Cell(1, 1).Value = "کد صورتحساب در سیستم حسابداری";
            worksheet.Cell(1, 2).Value = "شماره صورتحساب";
            worksheet.Cell(1, 3).Value = "تاریخ صورتحساب";
            worksheet.Cell(1, 4).Value = "نوع صورتحساب";
            worksheet.Cell(1, 5).Value = "نام کامل خریدار";
            worksheet.Cell(1, 6).Value = "نوع شخص حقیقی یا حقوقی";
            worksheet.Cell(1, 7).Value = "شماره / شناسه ملی";
            worksheet.Cell(1, 8).Value = "کد اقتصادی جدید";
            worksheet.Cell(1, 9).Value = "کدپستی";
            worksheet.Cell(1, 10).Value = "آدرس";
            worksheet.Cell(1, 11).Value = "کالا / خدمت";
            worksheet.Cell(1, 12).Value = "شناسه 13 رقمی کالا یا خدمت";
            worksheet.Cell(1, 13).Value = "شرح کالا یا خدمت";
            worksheet.Cell(1, 14).Value = "کد واحد اندازه گیری کالا یا خدمت";
            worksheet.Cell(1, 15).Value = "قیمت واحد (فی)";
            worksheet.Cell(1, 16).Value = "تعداد";
            worksheet.Cell(1, 17).Value = "تخفیف";
            worksheet.Cell(1, 18).Value = "نرخ ارزش افزوده";
            worksheet.Cell(1, 19).Value = "مبلغ ارزش افزوده";
            worksheet.Cell(1, 20).Value = "نوع تسویه حساب";

            var row = 2;
            foreach (var bill in bills)
            {
                worksheet.Cell(row, 1).Value = bill.AccountingInvoiceCode;
                worksheet.Cell(row, 2).Value = bill.InvoiceNumber;
                worksheet.Cell(row, 3).Value = bill.InvoiceDate;
                worksheet.Cell(row, 4).Value = bill.InvoiceType;
                worksheet.Cell(row, 5).Value = bill.BuyerFullName;
                worksheet.Cell(row, 6).Value = bill.BuyerType;
                worksheet.Cell(row, 7).Value = bill.NationalId;
                worksheet.Cell(row, 8).Value = bill.EconomicCode;
                worksheet.Cell(row, 9).Value = bill.PostalCode;
                worksheet.Cell(row, 10).Value = bill.Address;
                worksheet.Cell(row, 11).Value = bill.IsService;
                worksheet.Cell(row, 12).Value = bill.ItemIdentifier13;
                worksheet.Cell(row, 13).Value = bill.ItemDescription;
                worksheet.Cell(row, 14).Value = bill.UnitCode;
                worksheet.Cell(row, 15).Value = bill.UnitPrice;
                worksheet.Cell(row, 16).Value = bill.Quantity;
                worksheet.Cell(row, 17).Value = bill.Discount;
                worksheet.Cell(row, 18).Value = bill.VATRate;
                worksheet.Cell(row, 19).Value = bill.VATAmount;
                worksheet.Cell(row, 20).Value = bill.SettlementType;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"گزارش صورتحساب-مودیان-{party.BuyerFullName}.xlsx";

            return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }


        [HttpGet]
        public async Task<IActionResult> ChangeBillParty(Guid Id, string sender, string reciver, string party, string billnumber, long price)
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("خطا در شناسایی لایسنس");

            var model = new ChangePartyDto();
            model.Billnumber = billnumber;
            model.CurrentPartyName = party;
            model.Sender = sender;
            model.Reciver = reciver;
            model.Price = price;
            ViewBag.Clients = await _persen.SelectList_PersenAsync(_userContext.SellerId.Value);

            return PartialView("_ChangeBillParty", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeBillParty(ChangePartyDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (dto == null)
            {
                result.Message = "اطلاعاتی یافت نشد";
                return Json(result.ToJsonResult());
            }

            result = await _courierFinancialService.CHangeBillPartyAsync(dto);
            if (result.Success)
            {
                result.returnUrl = Request.Headers["Referer"].ToString();
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }

    }
}
