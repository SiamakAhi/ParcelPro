using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Interfaces;
using ParcelPro.Services;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;

namespace ParcelPro.Areas.Representatives.Controllers
{
    [Area("Representatives")]
    [Authorize]
    public class BranchReports : Controller
    {
        private readonly UserContextService _userContext;
        private readonly ICuBranchService _branchServic;
        private readonly IBillofladingService _bill;
        private readonly IBranchUserService _branchUserService;
        private readonly IGeneralService _gs;

        public BranchReports(UserContextService userContext
            , ICuBranchService branchServic
            , IBillofladingService billofladingService
            , IBranchUserService branchUserService
            , IGeneralService generalService)
        {
            _userContext = userContext;
            _bill = billofladingService;
            _branchUserService = branchUserService;
            _branchServic = branchServic;
            _gs = generalService;

            StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";

        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        public ActionResult RpWaybills()
        {
            return View();
        }

        public async Task<IActionResult> GetPrint_RpWaybills(
            long? _ReciverId,
            int? _RoutId,
            Guid? _Distributer,
            int? _OriginCityId,
            int? _DestinationCityId,
            short? _SettelmentType,
            short[]? _BillStatus,
            short? _PaymentStatus,
            string? _IssuerUserName,
            string _BiilOdLadingNumber,
            string? _strFromDate,
            string? _strUntilDate,
            bool _ShowCancelation,
            short _personSearchtype
            )
        {

            BillOfLadingFilterDto filter = new BillOfLadingFilterDto();
            filter.ReciverId = _ReciverId;
            filter.RoutId = _RoutId;
            filter.Distributer = _Distributer;
            filter.OriginCityId = _OriginCityId;
            filter.DestinationCityId = _DestinationCityId;
            filter.SettelmentType = _SettelmentType;
            filter.BillStatus = _BillStatus;
            filter.PaymentStatus = _PaymentStatus;
            filter.IssuerUserName = _IssuerUserName;
            filter.BiilOdLadingNumber = _BiilOdLadingNumber;
            filter.strFromDate = _strFromDate;
            filter.strUntilDate = _strUntilDate;
            filter.ShowCancelation = _ShowCancelation;
            filter.personSearchtype = _personSearchtype;

            // اعتبارسنجی SellerId و BranchId
            if (!_userContext.SellerId.HasValue || _userContext.BranchId == null)
                return NoContent();

            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (branch == null)
                return NoContent();

            var currentUser = await _branchUserService.GetBUserByUsernameAsync(User.Identity.Name);
            if (currentUser == null)
                return NoContent();

            filter.SellerId = _userContext.SellerId.Value;
            filter.OriginBranchId = _userContext.BranchId.Value;
            filter.DestinationCityId = branch.CityId;
            filter.branchIsOwner = branch.IsOwnership;
            filter.BranchCityId = branch.CityId;
            if (User.IsInRole("BranchManager"))
            {
                filter.OriginBranchId = null;
                filter.IsBranchManager = true;
            }

            var bills = await _bill.GetWaybillsAsQuery(filter).ToListAsync();

            string userName = User.Identity.Name;
            var userSett = await _gs.GetUserSettingAsync(userName);
            //---------------------------------------------------------------------
            StiReport report = new StiReport();
            string path = StiNetCoreHelper.MapPath(this, @"wwwroot/Reports/courier/Cu_Waybills.mrt");

            report.Load(path);
            report.RegData("bills", bills);
            StiVariable CompanyName = new StiVariable("CompanyName", userSett.ActiveSellerName);
            StiVariable rpDate = new StiVariable("ReportDate", DateTime.Now.LatinToPersian());
            report.Dictionary.Variables.Add(CompanyName);
            report.Dictionary.Variables.Add(rpDate);

            return StiNetCoreViewer.GetReportResult(this, report);

        }

    }
}
