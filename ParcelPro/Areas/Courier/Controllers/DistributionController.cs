using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.DitributionDto;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Interfaces.Identity;
using ParcelPro.Services;
using System.Security.Claims;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize]
    public class DistributionController : Controller
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
        private readonly IBusinessPartnerService _partner;

        long? _sellerId;


        public DistributionController(UserContextService userContextService
            , IBillofladingService billofladingService
            , ICuBranchService branchServic
            , IBranchUserService branchUser
            , IPersonService personService
            , ICourierServiceService courier
            , ICuPricingService pricingService
            , ICourierFinancialService courierFinancial
            , ITreasuryGeneralData treasuryGeneralData
            , IGeneralService gs
            , IAppIdentityUserManager usermanager,
              IBusinessPartnerService partner)
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
            _partner = partner;
        }

        [HttpGet]
        public async Task<IActionResult> CreateDistributionService()
        {
            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);
            if (!_sellerId.HasValue || user == null || branch == null) return NoContent();

            var model = new AddDistriutionDto();

            if (branch.IsOwnership)
                ViewBag.persen = await _persen.SelectList_PersenListAsync(_userContext.SellerId.Value);
            else
                ViewBag.persen = await _bill.SelectList_BranchPersenAsync(branch.Id);

            ViewBag.Clients = await _persen.SelectList_CreditClient(_sellerId.Value);
            ViewBag.routes = await _courier.SelectList_RoutesByDestinationCityAsync(_sellerId.Value, user.BranchCityId);
            ViewBag.services = await _courier.SelectList_BranchServicesAsync(branch.Id);
            ViewBag.BillNumber = "I9990" + "xxxxxx";
            ViewBag.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var IssuersBranch = new BranchFilterDto();
            IssuersBranch.SellerId = _sellerId.Value;
            IssuersBranch.IsIntercityFleet = true;

            ViewBag.IssuersBranch = await _partner.SelectList_BusinessPartnersAsync(_sellerId.Value);

            var DidtributerBranch = new BranchFilterDto();
            DidtributerBranch.SellerId = _sellerId.Value;
            DidtributerBranch.BranchId = branch.Id;
            ViewBag.DidtributerBranch = await _branchServic.SelectList_BranchesAsync(DidtributerBranch);

            ViewBag.Natures = await _pricing.SelectList_ConsigmentNatureAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDistributionService(AddDistriutionDto dto)
        {

            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            var branch = await _branchServic.FindBranchByIdAsync(_userContext.BranchId.Value);

            if (!_sellerId.HasValue || user == null || branch == null) return Json(result.ToJsonResult());

            //----
            if (dto == null)
            {
                result.Message = "اطلاعاتی یافت نشد";
                return Json(result.ToJsonResult());
            }
            if (string.IsNullOrEmpty(dto.ReferenceCode))
            {
                result.Message = "شماره برنامه مرج را وارد کنید";
                return Json(result.ToJsonResult());
            }

            //----
            var partner = await _partner.GetBusinessPartnerDtoAsync(dto.BusinessPartnerId);
            if (partner == null || partner.PersonId == null)
            {
                result.Message = "اطلاعات صادرکننده بارنامه کامل نیست";
                return Json(result.ToJsonResult());
            }
            //----
            var services = await _courier.GetServicesAsync(_sellerId.Value);
            if (services == null)
            {
                result.Message = "سرویسی در سیستم یافت نشد";
                return Json(result.ToJsonResult());
            }
            //----
            if (dto.SettelmentType == 3 && dto.PartyId == 0)
            {
                result.Message = "اگر بارنامه اعتباری است، باید طرف حساب را مشخص کنید";
                return Json(result.ToJsonResult());
            }
            int? route = await _branchServic.FindRoutByCityAsync(partner.CityId.Value, branch.CityId.Value, _sellerId.Value);
            if (route == null || route == 0)
            {
                result.Message = "با توحه به شهرهای مبدأ و مقصد، مسیری شناسایی نشد";
                return Json(result.ToJsonResult());
            }
            //----
            if (dto.PartyId == 0)
                dto.PartyId = null;

            if (dto.SettelmentType != 3)
                dto.PartyId = dto.ReceiverId;

            dto.ServiceId = services.FirstOrDefault().Id;
            dto.SellerId = _sellerId.Value;
            dto.OriginBranchId = branch.Id;
            dto.OriginHubId = branch.HubId.Value;
            dto.BusinessPartnerId = partner.Id;
            dto.BusinessPartyPersonId = partner.PersonId;
            dto.DistributerRepresentativeId = branch.Id;
            dto.RouteId = route;
            //..............................................
            if (ModelState.IsValid)
            {
                try
                {
                    dto.BillPrice = Convert.ToInt64(dto.strBillPrice.Replace(",", ""));
                    dto.SharePrice = Convert.ToInt64(dto.strSharePrice.Replace(",", ""));
                    dto.OtherCost = Convert.ToInt64(dto.strOtherCost.Replace(",", ""));

                }
                catch
                {
                    result.Message = "مبالغ را با فرمت صحیح وارد کنید";
                    return Json(result.ToJsonResult());
                }

                result = await _bill.CreateDistributionBillAsync(dto);
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

    }
}
