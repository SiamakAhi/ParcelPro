using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto.AccRepresentative;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Geolocation.GeolocationInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Services;

namespace ParcelPro.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    [Authorize]
    public class AccRepresentativeController : Controller
    {
        private readonly IPersonService _person;
        private readonly ICuBranchService _branch;
        private readonly UserContextService _userContext;
        private readonly IGeoGeneralService _locationService;
        private readonly IPersonService _persen;
        private readonly IAccRepresentativeService _rep;
        public AccRepresentativeController(IPersonService person,
            ICuBranchService branch,
            UserContextService userContextService,
            IGeoGeneralService locationService,
            IPersonService persen
            , IAccRepresentativeService representativeService)
        {
            _person = person;
            _branch = branch;
            _userContext = userContextService;
            _locationService = locationService;
            _persen = persen;
            _rep = representativeService;
        }

        public async Task<IActionResult> BranchesInfo(AccRepresentativeFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();
            filter.SellerId = _userContext.SellerId.Value;
            var model = new VmRepresentativeInfo();
            model.Branches = await _rep.GetRepresentativesInfo(filter);
            ViewBag.Cities = await _locationService.SelectItems_CitiesAsync();
            ViewBag.BranchesList = await _branch.SelectList_BranchesAsync(_userContext.SellerId.Value);
            ViewBag.Persen = await _persen.SelectList_PersenAsync(_userContext.SellerId.Value);
            return View(model);
        }

        public async Task<IActionResult> ReperesentativesIncome(RepresentativeSalaryFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();
            var model = new VmRepresentativeSalary();
            model.filter = filter;
            filter.SellerId = _userContext.SellerId.Value;
            if (filter.FromBody)
            {
                model.Reposrt = await _rep.ReperesentativeTotalSalariesAsync(filter);
            }

            ViewBag.BranchesList = await _branch.SelectList_BranchesAsync(_userContext.SellerId.Value);
            return View(model);
        }

        public async Task<IActionResult> BranchSalary(RepresentativeSalaryFilterDto filter)
        {
            if (!_userContext.SellerId.HasValue)
                return NoContent();
            var model = new VmRepresentativeSalary();
            model.filter = filter;
            filter.SellerId = _userContext.SellerId.Value;
            if (filter.FromBody)
            {
                model.ReposrtDetails = await _rep.ReperesentativeSalaryAsync(filter);
            }

            ViewBag.BranchesList = await _branch.SelectList_BranchesAsync(_userContext.SellerId.Value);
            return View(model);
        }
    }
}
