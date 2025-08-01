using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.Dto.SaleManagementDtos;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.DataTransfer.DataTransferInterfaces;
using ParcelPro.Areas.DataTransfer.Models;
using ParcelPro.Areas.Representatives.Dtos;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Interfaces.Identity;
using ParcelPro.Services;
using ParcelPro.ViewModels;
using ParcelPro.ViewModels.IdentityViewModels;
using ParcelPro.ViewModels.PartyDto;

namespace ParcelPro.Areas.Representatives.Controllers
{
    [Area("Representatives")]
    [Authorize]
    public class kpController : Controller
    {
        private readonly UserContextService _userContext;
        private readonly ICuBranchService _branchServic;
        private readonly IBranchUserService _branchUserService;
        private readonly IKPDataTransferService _sale;
        private readonly IBillofladingService _bill;
        private readonly IPersonService _persen;
        private readonly ICourierServiceService _courier;
        private readonly ICourierFinancialService _courierFinancialService;
        private readonly ITreasuryGeneralData _treasuryGeneralData;
        private readonly IAppIdentityUserManager _usermanager;
        private readonly IBusinessPartnerService _partner;
        public kpController(UserContextService userContext
            , ICuBranchService branchServic
            , IBranchUserService branchUserService
            , IKPDataTransferService sale
            , IBillofladingService billOfLadingImportService
            , IPersonService persen
            , ICourierServiceService courier
            , ICourierFinancialService courierFinancialService
            , ITreasuryGeneralData treasuryGeneralData
            , IAppIdentityUserManager usermanager,
              IBusinessPartnerService partner)
        {
            _userContext = userContext;
            _branchServic = branchServic;
            _branchUserService = branchUserService;
            _sale = sale;
            _bill = billOfLadingImportService;
            _persen = persen;
            _courier = courier;
            _courierFinancialService = courierFinancialService;
            _treasuryGeneralData = treasuryGeneralData;
            _usermanager = usermanager;
            _partner = partner;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Seller"))
                return RedirectToAction("IssuingWaybills");
            else if (User.IsInRole("CourierMan"))
                return RedirectToAction("PendingDistribution");

            return View();
        }

        //بارهای صادره
        public async Task<IActionResult> IssuedBills(BillOfLadingFilterDto filter)
        {
            // اعتبارسنجی SellerId و BranchId
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();

            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();

            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();

            // تنظیم مدل اصلی
            var model = new VmBranchUserPanel();
            model.Branch = branch;
            model.CurrentUser = currentUser;

            filter.SellerId = _userContext.SellerId.Value;
            filter.OriginBranchId = _userContext.BranchId.Value;
            model.filter = filter;

            // دریافت لیست درصدها و مسیرها از سرویس‌های مربوطه
            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.routes = await _courier.SelectList_RoutesByOriginCityAsync(_userContext.SellerId.Value, branch.CityId.Value);

            // دریافت بارنامه‌های صادره
            var billsOut = _bill.GetBillsAsQuery(filter);
            // var billsOut = _bill.GetSimpleBillsAsQuery(filter);
            model.Bills = Pagination<ViewBillOfLadings>.Create(billsOut, filter.CurrentPage, filter.PageSize);
            return View(model);
        }

        // بارنامه های وارده
        public async Task<IActionResult> IncomingBills(BillOfLadingFilterDto filter)
        {
            // اعتبارسنجی SellerId و BranchId
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();
            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();

            // تنظیم فیلتر مربوط به بارنامه‌های وارده
            filter.SellerId = _userContext.SellerId.Value;
            filter.DestinationCityId = branch.CityId;

            // تنظیم مدل اصلی
            var model = new VmBranchUserPanel
            {
                Branch = branch,
                CurrentUser = currentUser,
                filter = filter
            };


            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.routes = await _courier.SelectList_RoutesByDestinationCityAsync(_userContext.SellerId.Value, branch.CityId.Value);
            // دریافت بارنامه‌های وارده
            var billsIn = _bill.GetInputBillsAsQuery(filter);
            model.Bills = Pagination<ViewBillOfLadings>.Create(billsIn, filter.CurrentPage, filter.PageSize);
            return View(model);
        }


        public async Task<IActionResult> loadLadding(long id)
        {
            var model = new VmBilloflading();
            model.Billodlading = await _sale.FindBillofladdingByIdAsync(id);
            return PartialView("_loadLadding", model);
        }

        public async Task<IActionResult> IncomingBillsOfLading(DataTransfer.Dto.SaleFilterDto filter)
        {

            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null) return NoContent();
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null) return NoContent();
            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null) return NoContent();

            var model = new VmBranchUserPanel();
            model.Branch = branch;
            model.CurrentUser = currentUser;

            var data = _sale.GetIncomingBillsOfLadings(filter);
            //model.Pagin_IncomingBillsOfLading = Pagination<KPOldSystemSaleReport>.Create(data, filter.currentPage, filter.pageSize);
            return View();
        }

        public async Task<IActionResult> GetIncomingBillsOfLading(DataTransfer.Dto.SaleFilterDto filter)
        {
            var data = _sale.GetIncomingBillsOfLadings(filter);
            var model = Pagination<KPOldSystemSaleReport>.Create(data, filter.currentPage, filter.pageSize);
            return PartialView("GetIncomingBillsOfLading", model);
        }

        public async Task<IActionResult> Members(PersonFilterDto filter)
        {

            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();

            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();

            Guid? branchId = null;
            if (branch.IsOwnership)
                branchId = branch.Id;

            var model = new MembersViewmodel();
            model.filter = filter;
            model.filter.SellerId = _userContext.SellerId.Value;
            if (!branch.IsOwnership)
                model.filter.BranchId = branchId;

            var persenQuery = _persen.PersenAsQuery(filter);
            model.Persen = Pagination<PersonDto>.Create(persenQuery, filter.CurrentPage, filter.PageSize);

            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            return View(model);
        }

        //=============================================================================================================
        //========                         ============================================================================
        //=============================================================================================================

        public async Task<IActionResult> IssuingWaybills(BillOfLadingFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null) return NoContent();
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null) return NoContent();
            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null) return NoContent();


            var model = new VmBranchUserPanel();
            model.Branch = branch;
            model.CurrentUser = currentUser;

            filter.SellerId = _userContext.SellerId.Value;
            filter.OriginBranchId = _userContext.BranchId.Value;
            filter.DestinationBranchId = branch.Id;
            filter.BillStatus = new short[] { 1, 2 };

            model.filter = filter;
            model.OverView = await _bill.getDashboardDataAsync(filter, branch.CityId.Value);
            // دریافت بارنامه‌های وارده
            var billsIn = _bill.GetIssuindBillsAsQuery(filter);
            model.Bills = Pagination<ViewBillOfLadings>.Create(billsIn, filter.CurrentPage, filter.PageSize);


            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.routes = await _courier.SelectList_RoutesByDestinationCityAsync(_userContext.SellerId.Value, branch.CityId.Value);

            return View(model);
        }

        public async Task<IActionResult> AwaitingCollectionWaybills(BillOfLadingFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null) return NoContent();
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null) return NoContent();
            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null) return NoContent();


            var model = new VmBranchUserPanel();
            model.Branch = branch;
            model.CurrentUser = currentUser;

            filter.SellerId = _userContext.SellerId.Value;
            filter.OriginBranchId = _userContext.BranchId.Value;
            filter.DestinationBranchId = branch.Id;
            filter.BillStatus = new short[] { 3 };

            model.filter = filter;
            model.OverView = await _bill.getDashboardDataAsync(filter, branch.CityId.Value);
            // دریافت بارنامه‌های وارده
            var billsIn = _bill.GetIssuindBillsAsQuery(filter);
            model.Bills = Pagination<ViewBillOfLadings>.Create(billsIn, filter.CurrentPage, filter.PageSize);


            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.routes = await _courier.SelectList_RoutesByDestinationCityAsync(_userContext.SellerId.Value, branch.CityId.Value);

            return View(model);
        }

        public async Task<IActionResult> PendingDistribution(BillOfLadingFilterDto filter)
        {
            // اعتبارسنجی SellerId و BranchId
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();
            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();

            // تنظیم فیلتر مربوط به بارنامه‌های وارده
            filter.SellerId = _userContext.SellerId.Value;
            filter.DestinationCityId = branch.CityId;
            filter.DestinationBranchId = branch.Id;
            filter.branchIsOwner = branch.IsOwnership;
            // تنظیم مدل اصلی
            var model = new VmBranchUserPanel
            {
                Branch = branch,
                CurrentUser = currentUser,
                filter = filter
            };


            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.routes = await _courier.SelectList_RoutesByDestinationCityAsync(_userContext.SellerId.Value, branch.CityId.Value);
            ViewBag.IssuersBranch = await _partner.SelectList_BusinessPartnersAsync(_userContext.SellerId.Value);
            // دریافت بارنامه‌های وارده
            var billsIn = _bill.GetPendingDistributionAsQuery(filter);
            model.Bills = Pagination<ViewBillOfLadings>.Create(billsIn, filter.CurrentPage, filter.PageSize);

            BillOfLadingFilterDto overviewFilter = new BillOfLadingFilterDto();
            overviewFilter.SellerId = _userContext.SellerId.Value;
            overviewFilter.OriginBranchId = _userContext.BranchId.Value;
            overviewFilter.DestinationCityId = null;
            overviewFilter.DestinationBranchId = branch.Id;
            overviewFilter.branchIsOwner = branch.IsOwnership;

            // model.OverView = await _bill.getDashboardDataAsync(overviewFilter, branch.CityId.Value);

            return View(model);
        }

        public async Task<IActionResult> NoSetteledBills(BillOfLadingFilterDto filter)
        {
            // اعتبارسنجی SellerId و BranchId
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();
            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();

            // تنظیم فیلتر مربوط به بارنامه‌های وارده
            filter.SellerId = _userContext.SellerId.Value;
            filter.OriginBranchId = _userContext.BranchId.Value;
            // تنظیم مدل اصلی
            var model = new VmBranchUserPanel
            {
                Branch = branch,
                CurrentUser = currentUser,
                filter = filter
            };


            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.routes = await _courier.SelectList_RoutesByDestinationCityAsync(_userContext.SellerId.Value, branch.CityId.Value);
            // دریافت بارنامه‌های وارده
            var billsIn = _bill.GetNoSetteledBranchBillsAsQuery(filter, branch.CityId.Value);
            model.Bills = Pagination<ViewBillOfLadings>.Create(billsIn, filter.CurrentPage, filter.PageSize);

            BillOfLadingFilterDto overviewFilter = new BillOfLadingFilterDto();
            overviewFilter.SellerId = _userContext.SellerId.Value;
            overviewFilter.OriginBranchId = _userContext.BranchId.Value;
            overviewFilter.DestinationCityId = null;
            overviewFilter.DestinationBranchId = branch.Id;

            // model.OverView = await _bill.getDashboardDataAsync(overviewFilter, branch.CityId.Value);

            return View(model);
        }


        //=============================================================================================================
        //========                         ============================================================================
        //=============================================================================================================

        public async Task<IActionResult> DestributeService()
        {
            return View();
        }

        public async Task<IActionResult> CourierService()
        {
            return View();
        }

        public async Task<IActionResult> BranchCashbox()
        {
            return View();
        }

        public async Task<IActionResult> Waybills(BillOfLadingFilterDto filter)
        {
            // اعتبارسنجی SellerId و BranchId
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();

            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();

            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();

            // تنظیم مدل اصلی
            var model = new VmBranchUserPanel();
            model.Branch = branch;
            model.CurrentUser = currentUser;

            filter.SellerId = _userContext.SellerId.Value;
            filter.OriginBranchId = _userContext.BranchId.Value;
            filter.DestinationCityId = branch.CityId;
            filter.branchIsOwner = branch.IsOwnership;
            filter.BranchCityId = branch.CityId;
            ViewBag.Issuers = null;
            ViewBag.routes = await _courier.SelectList_RoutesByOriginCityAsync(_userContext.SellerId.Value, branch.CityId.Value);
            if (User.IsInRole("BranchManager"))
            {
                filter.OriginBranchId = null;
                filter.IsBranchManager = true;
                ViewBag.routes = await _courier.SelectList_RoutesAsync(_userContext.SellerId.Value);
                ViewBag.Issuers = await _bill.SelectList_IssuersUsersAsync(_userContext.SellerId.Value);
            }
            model.filter = filter;

            // دریافت لیست درصدها و مسیرها از سرویس‌های مربوطه
            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.Distributers = await _branchServic.SelectList_DestributerAsync(_userContext.SellerId.Value);

            // دریافت بارنامه‌های صادره
            if (filter.IsFromBody)
            {
                var billsOut = _bill.GetWaybillsAsQuery(filter);
                model.Bills = Pagination<ViewBillOfLadings>.Create(billsOut, filter.CurrentPage, filter.PageSize);
            }
            return View(model);
        }

        //=============================================================================================================
        //========                         ============================================================================
        //=============================================================================================================

        public async Task<IActionResult> BranchPartyBills(PartyBillsFilterDto filter)
        {
            // اعتبارسنجی SellerId و BranchId
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();

            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();

            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();


            ViewBag.Parties = await _courierFinancialService.SelectList_CreditCustomersAsync(_userContext.SellerId.Value);
            var model = new PartyBillsViewModel();
            if (!filter.PartyId.HasValue)
                return View(model);

            model.PartyInfo = await _persen.GetPersonDtoAsync(filter.PartyId.Value);
            model.Bills = await _courierFinancialService.GetPartyBillsAsync(filter);
            model.filter = filter;

            return View(model);

        }

        //...................... User Profile 
        public async Task<IActionResult> AppUserProfile(string? message = null)
        {
            if (message != null)
            {
                ViewBag.msg = message;
            }
            VmUserProfile model = new VmUserProfile();
            model.UpdateProfile = await _usermanager.GetUserVmAsync(User.Identity.Name);
            model.UserRoles = await _usermanager.GetUserRolesByNameAsync(User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserProfile(VmUpdateProfile user)
        {
            clsResult result = new clsResult();
            user.BirthDate = user.StrBirthDate?.PersianToLatin();
            if (ModelState.IsValid)
            {
                var res = await _usermanager.UpdateProfile(user);
                if (res.Succeeded)
                {
                    return RedirectToAction("AppUserProfile");
                }
            }

            return RedirectToAction("AppUserProfile");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(VmChangePass model)
        {
            var user = await _usermanager.FindByNameAsync(model.UserName);
            string msg = "";
            if (ModelState.IsValid)
            {
                var res = await _usermanager.RemovePasswordAsync(user);
                if (res.Succeeded)
                {
                    string token = await _usermanager.GeneratePasswordResetTokenAsync(user);
                    await _usermanager.ResetPasswordAsync(user, token, model.NewPassword);
                    msg = "بروزرسانی رمز عبور شما با موفقیت انجام شد";
                    return RedirectToAction("AppUserProfile", new { message = msg });
                }
            }
            msg = "مشکلی در تغییر کلمه عبور پیش آمده است. لطفاً مجددا سعی کنید";
            return RedirectToAction("AppUserProfile", new { message = msg });
        }

    }

}
