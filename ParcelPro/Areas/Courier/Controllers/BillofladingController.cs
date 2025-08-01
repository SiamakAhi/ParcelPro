using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParcelPro.Areas.Courier.Classes;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.FinancialDtos;
using ParcelPro.Areas.Courier.Dto.RepresentativeDtos;
using ParcelPro.Areas.Representatives.Dtos;
using ParcelPro.Areas.Treasury.Dto;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Interfaces.Identity;
using ParcelPro.Services;
using ParcelPro.ViewModels;
using ParcelPro.ViewModels.PartyDto;
using System.Security.Claims;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize]
    public class BillofladingController : Controller
    {

        private readonly UserContextService _userContext;
        private readonly IBillofladingService _bill;
        private readonly ICuBranchService _branchServic;
        private readonly IBranchUserService _branchUser;
        private readonly IPersonService _persen;
        private readonly ICourierServiceService _courier;
        private readonly ICuPricingService _pricing;
        private readonly ICourierFinancialService _courierFinancial;
        private readonly ITreasuryGeneralData _treasuryGeneralData;
        private readonly IGeneralService _gs;
        private readonly IAppIdentityUserManager _usermanager;

        long? _sellerId;


        public BillofladingController(UserContextService userContextService
            , IBillofladingService billofladingService
            , ICuBranchService branchServic
            , IBranchUserService branchUser
            , IPersonService personService
            , ICourierServiceService courier
            , ICuPricingService pricingService
            , ICourierFinancialService courierFinancial
            , ITreasuryGeneralData treasuryGeneralData
            , IGeneralService gs
            , IAppIdentityUserManager usermanager)
        {
            _userContext = userContextService;
            _bill = billofladingService;
            _branchServic = branchServic;
            _branchUser = branchUser;
            _persen = personService;
            _courier = courier;
            _pricing = pricingService;
            _courierFinancial = courierFinancial;
            _sellerId = _userContext.SellerId;
            _treasuryGeneralData = treasuryGeneralData;
            _gs = gs;
            _usermanager = usermanager;

        }

        [HttpGet]
        public async Task<IActionResult> CreateBillofladingHeader()
        {
            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (!_sellerId.HasValue || user == null || branch == null) return NoContent();

            var model = new VmBillofladingPanel();
            model.Branch = branch;
            model.CurrentUser = user;

            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.routes = await _courier.SelectList_RoutesByOriginCityAsync(_sellerId.Value, user.BranchCityId);
            ViewBag.services = await _courier.SelectList_BranchServicesAsync(branch.Id);
            ViewBag.BillNumber = user.BranchCode + "xxxxxx";// await _bill.GenerateBillNumberAsync(_sellerId.Value, user.BranchCode);
            ViewBag.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            // ViewBag.Distributers = await _branchServic.SelectList_DestributerAsync(_sellerId.Value);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBillofladingHeader(VmBillofladingPanel model)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (!_sellerId.HasValue || user == null || branch == null) return NoContent();

            if (!branch.HubId.HasValue)
            {
                result.Message = "هاب متصل به شعبه شناسایی نشد";
                return Json(result.ToJsonResult());
            }


            //if (model.BillOfLading.bill.DistributerRepresentativeId == Guid.Empty)
            //{
            //    result.Message = "نماینده توزیع را مشخص کنید";
            //    return Json(result.ToJsonResult());
            //}

            model.BillOfLading.bill.SellerId = _sellerId.Value;
            model.BillOfLading.bill.BillOfLadingStatusId = 1;
            model.BillOfLading.bill.LastStatusDescription = "درحال صدور";

            if (ModelState.IsValid)
            {

                result = await _bill.CreateNewBillOfLadingAsync(model.BillOfLading.bill);
                if (result.Success)
                {
                    result.ShowMessage = false;
                    result.updateType = 1;
                    result.returnUrl = Url.Action("Billoflading", new { id = model.BillOfLading.bill.Id, Area = "Courier" });
                    return Json(result.ToJsonResult());
                }
            }

            model.Branch = branch;
            model.CurrentUser = user;

            var erroes = ModelState.Values.SelectMany(e => e.Errors).ToList();
            foreach (var error in erroes)
            {
                result.Message += "</br>" + error.ErrorMessage;
            }
            return Json(result.ToJsonResult());
        }



        [HttpGet]
        public async Task<IActionResult> EditBillofladingHeader(Guid Id)
        {
            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (!_sellerId.HasValue || user == null || branch == null) return NoContent();

            var model = new VmBillofladingPanel();
            model.Branch = branch;
            model.CurrentUser = user;

            ViewBag.persen = await _persen.SelectList_PersenListAsync(_sellerId.Value);
            ViewBag.routes = await _courier.SelectList_RoutesByOriginCityAsync(_sellerId.Value, user.BranchCityId);
            ViewBag.services = await _courier.SelectList_ServicesAsync(_sellerId.Value);
            ViewBag.BillNumber = await _bill.GenerateBillNumberAsync(_sellerId.Value, user.BranchCode);
            ViewBag.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return PartialView("_EditBillofladingHeader", model);
        }

        [HttpGet]
        public async Task<IActionResult> Billoflading(Guid id)
        {
            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (!_sellerId.HasValue || user == null || branch == null) return NoContent();


            var model = new VmBillofladingPanel();
            model.Branch = branch;
            model.CurrentUser = user;
            model.BillOfLading = await _bill.GetBillOfLadingDtoAsync(id);

            ViewBag.persen = await _persen.SelectList_PersenListAsync(_sellerId.Value);
            ViewBag.Clients = await _persen.SelectList_CreditClient(_sellerId.Value);
            ViewBag.routes = await _courier.SelectList_RoutesByOriginCityAsync(_sellerId.Value, user.BranchCityId);
            ViewBag.services = await _courier.SelectList_ServicesAsync(_sellerId.Value);
            ViewBag.BillNumber = await _bill.GenerateBillNumberAsync(_sellerId.Value, user.BranchCode);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddConsigment(Guid id)
        {
            if (!_sellerId.HasValue) return NoContent();
            var model = new AddParcelDto();
            model.BillOfLading = await _bill.GetParcelHeaderInfoAsync(id);


            var package = await _courier.GetPackagingsAsync(_sellerId.Value);
            long catId = package.FirstOrDefault()?.WarehouseProductCategoryId ?? 0;
            ViewBag.Packages = await _courier.SelectLst_PackagesAsync(catId);

            ViewBag.Natures = await _pricing.SelectList_ConsigmentNatureAsync();

            ParcelPricingItemDto priceItemDto = new ParcelPricingItemDto();
            priceItemDto.SellerId = _sellerId.Value;
            priceItemDto.ServiceId = model.BillOfLading.ServiceId;
            priceItemDto.RouteId = model.BillOfLading.RouteId;
            var basePrice = await _pricing.GetParcellPriceAsync(priceItemDto);
            ViewBag.defaulPrice = basePrice.Price.ToPrice();
            var insurance = await _pricing.GetInsuranceSettingAsync(_sellerId.Value);
            ViewBag.defaulInsuranceCost = insurance.BaseCost.ToPrice();
            ViewBag.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return PartialView("_AddConsigment", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConsigment(AddParcelDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);

            if (!_sellerId.HasValue || user == null || branch == null) return NoContent();
            if (dto.Consigmen == null)
            {
                result.Message = "اطلاعاتی یافت نشد";
                return Json(result.ToJsonResult());
            }

            //..............................................
            if (ModelState.IsValid)
            {
                dto.Consigmen.Value = Convert.ToInt64(dto.Consigmen.strValue.Replace(",", ""));
                dto.Consigmen.CargoFare = Convert.ToInt64(dto.Consigmen.strCargoFare.Replace(",", ""));
                dto.Consigmen.PackagingCost = Convert.ToInt64(dto.Consigmen.strPackagingCost.Replace(",", ""));
                dto.Consigmen.OtherCost = Convert.ToInt64(dto.Consigmen.strOtherCost.Replace(",", ""));
                dto.Consigmen.InsuranceCost = Convert.ToInt64(dto.Consigmen.strInsuranceCost.Replace(",", ""));
                dto.Consigmen.Discount = Convert.ToInt64(dto.Consigmen.strDiscount.Replace(",", ""));
                dto.Consigmen.VatPrice = Convert.ToInt64(dto.Consigmen.strVatPrice.Replace(",", ""));
                dto.Consigmen.TotalPrice = Convert.ToInt64(dto.Consigmen.strTotalPrice.Replace(",", ""));

                result = await _bill.AddNewParcelAsync(dto.Consigmen);
                if (result.Success)
                {
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "</br>" + er.ErrorMessage;
            }
            result.Success = false;
            result.ShowMessage = true;
            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<IActionResult> computeParcellPrice(ParcelPricingItemDto dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid input data.");
            }

            ComputedPriceDto price = await _pricing.GetParcellPriceAsync(dto);

            if (price == null)
            {
                return NotFound("Unable to compute price.");
            }
            string jsonResult = JsonConvert.SerializeObject(price);
            return Json(jsonResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetPakcageCost(long id)
        {
            long price = await _pricing.GetPackagePriceAsync(id);

            return Json(price);
        }

        [HttpPost]
        public async Task<IActionResult> BillSettelment(Guid BillId, short SettelmentTypeId, long? partyId = null, Guid? branchId = null)
        {
            if (!_sellerId.HasValue) return NoContent();

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return NoContent();

            if (SettelmentTypeId == 1)
            {
                var model = await _bill.GetBillCashPayDtoAsync(BillId);
                model.UserId = userId;
                model.SettelmentType = 1;
                ViewBag.BankAccounts = await _treasuryGeneralData.SelectList_BankAccountsAsync(_sellerId.Value);
                if (branchId == null)
                    ViewBag.Pos = await _treasuryGeneralData.SelectList_PosDevicesAsync(_sellerId.Value);
                else
                    ViewBag.Pos = await _treasuryGeneralData.SelectList_BranchPOSesAsync(branchId.Value);

                return PartialView("_chashSettelment", model);
            }
            else if (SettelmentTypeId == 4)
            {
                if (SettelmentTypeId == 4 && partyId == null)
                {
                    MessageBoxDto msg = new MessageBoxDto
                    {
                        Title = "صدور بارنامه",
                        Message = "برای تهاتر مبلغ بارنامه، انتخاب طرف حساب الزامی ست"
                    };
                    return PartialView("_modalmessage", msg);
                }
                var model = await _bill.GetBillCashPayDtoAsync(BillId);
                model.UserId = userId;
                model.SettelmentType = SettelmentTypeId;
                if (SettelmentTypeId == 4 && partyId.HasValue)
                {
                    var party = await _persen.GetPersonDtoAsync(partyId.Value);
                    model.AccountPartyId = party.Id;
                    model.PartyName = party.Name;
                }
                return PartialView("_tahatorSettlement", model);

            }
            else
            {
                if (SettelmentTypeId == 3 && partyId == null)
                {
                    MessageBoxDto msg = new MessageBoxDto
                    {
                        Title = "صدور بارنامه",
                        Message = "برای بارنامه های اعتباری انتخاب طرف حساب الزامی ست"
                    };
                    return PartialView("_modalmessage", msg);
                }

                var model = await _bill.GetBillCashPayDtoAsync(BillId);
                model.UserId = userId;
                model.SettelmentType = SettelmentTypeId;
                if (SettelmentTypeId == 3 && partyId.HasValue)
                {
                    var party = await _persen.GetPersonDtoAsync(partyId.Value);
                    model.AccountPartyId = party.Id;
                    model.PartyName = party.Name;
                }
                return PartialView("_creditSettlement", model);
            }

        }

        public IActionResult modalmessage(MessageBoxDto dto)
        {
            return PartialView("_modalmessage", dto);
        }
        public async Task<IActionResult> BillCashSettelment(Guid BillId, Guid? branchId = null)
        {
            if (!_sellerId.HasValue) return NoContent();

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return NoContent();

            var model = await _bill.GetBillCashPayDtoAsync(BillId);
            model.UserId = userId;
            model.SettelmentType = 1;
            ViewBag.BankAccounts = await _treasuryGeneralData.SelectList_BankAccountsAsync(_sellerId.Value);

            if (branchId == null)
                ViewBag.Pos = await _treasuryGeneralData.SelectList_PosDevicesAsync(_sellerId.Value);
            else
                ViewBag.Pos = await _treasuryGeneralData.SelectList_BranchPOSesAsync(branchId.Value);

            return PartialView("_chashSettelment", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetSettelment(FinancialTransactionDto model)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (ModelState.IsValid)
            {
                result = await _courierFinancial.SetSettelmentAsync(model);
                if (result.Success)
                {
                    result.ShowMessage = false;
                    result.updateType = 1;
                    result.returnUrl = Url.Action("Billoflading", new { id = model.BillOfLadingId, Area = "Courier" });
                    return Json(result.ToJsonResult());
                }
            }
            return Json(result.ToJsonResult());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IssueBillOfLading(BillCashPayDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (dto.SettelmentType == 0)
            {
                result.Message = "نوع تسویه حساب را انتخاب کنید";
                return Json(result.ToJsonResult());
            }
            var model = await _courierFinancial.GetBillDataAsync(dto.BillOfLadingId.Value, dto.SettelmentType, userId);
            if (model == null)
            {
                result.Message = "مشکلی در ثبت تسویه حساب بارنامه رخ داده است";
                return Json(result.ToJsonResult());
            }
            if (dto.SettelmentType == 3)
            {
                model.AccountPartyId = dto.AccountPartyId;
                model.PartyName = dto.PartyName;
            }
            result = await _courierFinancial.SetSettelmentAsync(model);

            if (result.Success)
            {
                result.ShowMessage = false;
                result.updateType = 1;
                result.returnUrl = Url.Action("Billoflading", new { id = model.BillOfLadingId, Area = "Courier" });
                result.Message = "صدور بارنامه انجام شد";
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }
        public async Task<IActionResult> BillPayment(Guid billId)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return NoContent();
            var model = await _courierFinancial.GetBillDataAsync(billId, 1, userId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SendPaymentLink(Guid billId)
        {
            return PartialView("_SendPaymentLink");
        }

        [AllowAnonymous]
        public async Task<IActionResult> bill(Guid billOfladingId)
        {
            var model = await _bill.GetBillOfLadingDtoAsync(billOfladingId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SaveReceiverSign(Guid id)
        {
            var bill = await _bill.GetBillOfLadingDtoAsync(id);
            if (!_sellerId.HasValue || bill == null) return NoContent();

            ParcelDeliveryDto model = new ParcelDeliveryDto();
            model.WayBillNumber = bill.bill.WaybillNumber;
            model.BillOfLadingId = id;
            model.SellerId = _sellerId.Value;
            model.SenderUserName = User.Identity.Name;
            model.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            model.ReceiverMobile = bill.bill.ReciverPhone;
            model.ReceiverName = bill.bill.ReciverName;

            return View(model);
            // return PartialView("_SaveReceiverSign", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveReceiverSign(ParcelDeliveryDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (ModelState.IsValid)
            {
                result = await _bill.SaveParcelDeliveryAsync(dto);
                if (result.Success)
                {
                    result.returnUrl = Url.Action("bill", "Billoflading", new { Area = "Courier", billOfladingId = dto.BillOfLadingId });
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }
            var modelErrors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message = "\n " + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }


        [HttpGet]
        public async Task<IActionResult> NewPerson()
        {
            string userName = User.Identity.Name;
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);

            if (!_sellerId.HasValue || branch == null) return NoContent();
            ViewBag.customerId = 2;
            ViewBag.SellerId = await _gs.GetActiveSellerIdAsync(userName);
            ViewBag.PartyTypes = await _persen.SelectList_PartyTypeAsync();
            ViewBag.BranchId = branch.Id;

            return PartialView("_NewPerson");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPerson(PersonDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            string userName = User.Identity.Name;
            int? cusId = 2;
            long? sellerId = await _gs.GetActiveSellerIdAsync(userName);

            if (string.IsNullOrEmpty(userName) || sellerId == null)
            {
                result.Message = "از لحاظ امنیتی و دسترسی مشکلی پیش آمده است. مجددا لاگین کنید";
                return Json(result.ToJsonResult());
            }

            ViewBag.customerId = cusId;
            ViewBag.SellerId = sellerId;

            if (ModelState.IsValid)
            {
                result = await _persen.AddPersonAsync(dto);
                if (result.Success)
                {
                    result.Success = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.ShowMessage = true;
                    return Json(result.ToJsonResult());
                }
            }
            var modelErrors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message = "\n " + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> DellBill(Guid id)
        {
            var bill = await _bill.GetBillDataAsync(id);

            return PartialView("_DellBill", bill);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DellBill(BillDataViewModel dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (ModelState.IsValid)
            {
                result = await _bill.DeleteBillAsync(dto);
                if (result.Success)
                {
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }
            var modelErrors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message = "\n " + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public IActionResult SendPayment(Guid id, string trackingCode, string reciverNumber)
        {
            ViewBag.Reciver = reciverNumber;
            ViewBag.Id = id;
            ViewBag.Tracking = trackingCode;

            return PartialView("_SendPayment");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendPaymentLink(Guid id, string trackingCode, string reciverNumber)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            result = await _bill.SendPaymentLinkAsync(id, trackingCode, reciverNumber);
            if (result.Success)
            {
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.updateType = 1;
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<IActionResult> DelParcel(Guid id)
        {

            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;
            result = await _bill.DeleteParcelAsync(id);

            if (result.Success)
            {
                result.returnUrl = Request.Headers["Referer"].ToString();
                result.updateType = 1;
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> ChangeSenderOrReciver(Guid id, bool isSender)
        {
            var model = await _bill.GetSenderOrResiverData(id, isSender);
            if (model == null)
                return BadRequest();
            model.UserName = User.Identity.Name;
            model.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            ViewBag.persen = await _persen.SelectList_PersenListAsync(_sellerId.Value);

            return PartialView("_ChangeSenderOrReciver", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeSenderOrReciver(ChangeSenderOrReciverDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            string userName = User.Identity.Name;
            long? sellerId = await _gs.GetActiveSellerIdAsync(userName);

            if (string.IsNullOrEmpty(userName) || sellerId == null)
            {
                result.Message = "از لحاظ امنیتی و دسترسی مشکلی پیش آمده است. مجددا لاگین کنید";
                return Json(result.ToJsonResult());
            }

            if (ModelState.IsValid)
            {
                result = await _bill.ChangeSenderOrReciverAsync(dto);
                if (result.Success)
                {
                    result.Success = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.ShowMessage = true;
                    return Json(result.ToJsonResult());
                }
            }
            var modelErrors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message = "\n " + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<IActionResult> billView(string billnumber)
        {
            var model = await _bill.FindBillOfLadingDtoByNumberAsync(billnumber);
            return PartialView("_billView", model);
        }


        [HttpGet]
        public async Task<IActionResult> CancelationBill(Guid billId)
        {
            var bill = await _bill.GetBillDataAsync(billId);

            return PartialView("_CancelationBill", bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelationBill(BillDataViewModel dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (ModelState.IsValid)
            {
                result = await _bill.BillCancelationAsync(dto);
                if (result.Success)
                {
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }
            var modelErrors = ModelState.Values.SelectMany(x => x.Errors).ToList();
            foreach (var error in modelErrors)
            {
                result.Message = "\n " + error.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> UpdateConsigment(Guid id)
        {
            if (!_sellerId.HasValue) return NoContent();
            var model = new AddParcelDto();
            model.Consigmen = await _bill.GetParcelByIdAsync(id);
            model.BillOfLading = await _bill.GetParcelHeaderInfoAsync(model.Consigmen.BillOfLadingId);


            var package = await _courier.GetPackagingsAsync(_sellerId.Value);
            long catId = package.FirstOrDefault()?.WarehouseProductCategoryId ?? 0;
            ViewBag.Packages = await _courier.SelectLst_PackagesAsync(catId);

            ViewBag.Natures = await _pricing.SelectList_ConsigmentNatureAsync();

            ParcelPricingItemDto priceItemDto = new ParcelPricingItemDto();
            priceItemDto.SellerId = _sellerId.Value;
            priceItemDto.ServiceId = model.BillOfLading.ServiceId;
            priceItemDto.RouteId = model.BillOfLading.RouteId;
            var basePrice = await _pricing.GetParcellPriceAsync(priceItemDto);
            ViewBag.defaulPrice = basePrice.Price.ToPrice();
            var insurance = await _pricing.GetInsuranceSettingAsync(_sellerId.Value);
            ViewBag.defaulInsuranceCost = insurance.BaseCost.ToPrice();
            ViewBag.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return PartialView("_UpdateConsigment", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateConsigment(AddParcelDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (dto.Consigmen == null)
            {
                result.Message = "اطلاعاتی یافت نشد";
                return Json(result.ToJsonResult());
            }

            //..............................................
            if (ModelState.IsValid)
            {
                dto.Consigmen.Value = Convert.ToInt64(dto.Consigmen.strValue.Replace(",", ""));
                dto.Consigmen.CargoFare = Convert.ToInt64(dto.Consigmen.strCargoFare.Replace(",", ""));
                dto.Consigmen.PackagingCost = Convert.ToInt64(dto.Consigmen.strPackagingCost.Replace(",", ""));
                dto.Consigmen.OtherCost = Convert.ToInt64(dto.Consigmen.strOtherCost.Replace(",", ""));
                dto.Consigmen.InsuranceCost = Convert.ToInt64(dto.Consigmen.strInsuranceCost.Replace(",", ""));
                dto.Consigmen.Discount = Convert.ToInt64(dto.Consigmen.strDiscount.Replace(",", ""));
                dto.Consigmen.VatPrice = Convert.ToInt64(dto.Consigmen.strVatPrice.Replace(",", ""));
                dto.Consigmen.TotalPrice = Convert.ToInt64(dto.Consigmen.strTotalPrice.Replace(",", ""));

                result = await _bill.UpdateParcelAsync(dto.Consigmen);
                if (result.Success)
                {
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "</br>" + er.ErrorMessage;
            }
            result.Success = false;
            result.ShowMessage = true;
            return Json(result.ToJsonResult());
        }


        //======================= مشتریان اعتباری
        public async Task<IActionResult> CreditClients(PersonFilterDto filter)
        {
            if (!_sellerId.HasValue) return NoContent();
            filter.IsCreditClient = true;
            var model = new MembersViewmodel();
            model.filter = filter;
            model.filter.IsCreditClient = true;
            model.filter.SellerId = _sellerId.Value;
            var data = _persen.PersenAsQuery(filter);
            model.Persen = Pagination<PersonDto>.Create(data, model.filter.CurrentPage, model.filter.PageSize);

            ViewBag.Persen = await _persen.SelectList_PersenAsync(_sellerId.Value);
            ViewBag.Clients = await _persen.SelectList_CreditClient(_sellerId.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> WayBillDistributerBranch(Guid id)
        {
            if (!_sellerId.HasValue)
                return NoContent();
            var model = await _bill.GetWaybillDistributerByIdAsync(id);
            ViewBag.Distributers = await _branchServic.SelectList_DestributerAsync(_sellerId.Value);
            return PartialView("_WayBillDistributerBranch", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WayBillDistributerBranch(WaybillDitributerUpdateDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (dto == null)
            {
                result.Message = "اطلاعاتی یافت نشد";
                return Json(result.ToJsonResult());
            }

            //..............................................
            if (ModelState.IsValid)
            {
                if (dto.DistributerId == null || dto.DistributerId == Guid.Empty)
                    dto.DistributerId = null;

                result = await _bill.SetWaybillDistributerAsync(dto);
                if (result.Success)
                {
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "</br>" + er.ErrorMessage;
            }
            result.Success = false;
            result.ShowMessage = true;
            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<IActionResult> WaybillLabel(Guid id)
        {
            try
            {
                var dtos = await _bill.WaybillLabelsAsync(id);

                foreach (var dto in dtos)
                {
                    var zpl = GenerateZPL(dto);
                    bool success = RawPrinterHelper.SendStringToPrinter("BIXLON", zpl);

                    if (!success)
                    {
                        return Json(new
                        {
                            success = false,
                            message = $"چاپ لیبل برای بارنامه {dto.WaybillNimber} ناموفق بود"
                        });
                    }
                }

                return Json(new { success = true }); // موفقیت بی‌نیاز از پیام
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "خطای سیستمی: " + ex.Message
                });
            }
        }

        private string GenerateZPL(WaybillLabelDto dto)
        {
            return $@"
                    ^XA
                    ^CF0,40
                    ^FO30,30^FDکیهان پست^FS
                    ^FO30,80^BY2
                    ^BCN,80,Y,N,N
                    ^FD{dto.WaybillNimber}^FS
                    ^FO30,180^FDمبدأ: {dto.OriginCity}^FS
                    ^FO30,220^FDمقصد: {dto.Destination}^FS
                    ^FO30,260^FDتعداد: {dto.ParcelNumber}/{dto.TotalCountParcel}^FS
                    ^FO30,300^FDوزن: {dto.weight} Kg^FS
                    ^FO30,340^BQN,2,4^FDLA:https://keyhanpost.ir^FS
                    ^XZ";
        }

        public async Task<IActionResult> BillPaymentRecords(Guid id)
        {
            if (!_sellerId.HasValue) return NoContent();
            var model = await _bill.GetBillCashPayDtoAsync(id);
            return PartialView("_BillPaymentRecords", model);

        }

        [HttpGet]
        public async Task<IActionResult> ChangeSettelmenttype(Guid id)
        {
            if (!_sellerId.HasValue) return NoContent();
            var bill = await _bill.GetBillDataAsync(id);
            ViewBag.Clients = await _persen.SelectList_CreditClient(_sellerId.Value);
            return PartialView("_ChangeSettelmenttype", bill);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeSettelmenttype(BillDataViewModel dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (dto == null)
            {
                result.Message = "اطلاعاتی یافت نشد";
                return Json(result.ToJsonResult());
            }

            //..............................................

            if (dto.ChangeSettelmentModel.settelmentTypeId == 3 && dto.ChangeSettelmentModel.PartyId == 0)
            {
                result.Message = "برای بارنامه های اعتباری تعیین طرف حساب الزامی است";
            }

            result = await _bill.ChangeSettelmentAsync(dto.ChangeSettelmentModel);
            if (result.Success)
            {
                result.returnUrl = Request.Headers["Referer"].ToString();
                return Json(result.ToJsonResult());
            }

            return Json(result.ToJsonResult());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDestributer(List<Guid> items)
        {
            if (!_sellerId.HasValue) return NoContent();
            ChangeDestribiuterDto dto = new ChangeDestribiuterDto();
            dto.BillsId = items;
            ViewBag.Branches = await _branchServic.SelectList_BranchesAsync(_sellerId.Value);
            return PartialView("_ChangeDestributer", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeDestributerInBulk(ChangeDestribiuterDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (dto == null)
            {
                result.Message = "اطلاعاتی یافت نشد";
                return Json(result.ToJsonResult());
            }
            if (dto.BranchId == null)
            {
                result.Message = "نمانیده را مشخص نکردید";
                return Json(result.ToJsonResult());
            }
            if (ModelState.IsValid)
            {
                result = await _bill.ChangeDestributerBulkAsync(dto);
                if (result.Success)
                {
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(n => n.Errors).ToList();
                foreach (var er in errors)
                {
                    result.Message += er.ErrorMessage + "<br>";
                }
            }

            return Json(result.ToJsonResult());
        }


        [HttpPost]
        public async Task<IActionResult> GetDistributers(int Id)
        {
            if (!_sellerId.HasValue) return NoContent();

            var data = await _branchServic.GetDistributersByDestinationSityAsync(Id, _sellerId.Value);
            return Json(data);

        }
    }

}


