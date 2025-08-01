using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Services;
using ParcelPro.ViewModels.PartyDto;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize]
    public class BranchClientsController : Controller
    {
        private readonly IPersonService _person;
        private readonly ICuBranchService _branch;
        private readonly IBranchUserService _branchUser;
        private readonly UserContextService _userContext;
        public BranchClientsController(IPersonService person,
            ICuBranchService branchService,
            UserContextService userContextService,
            IBranchUserService branchUserService)
        {
            _person = person;
            _branch = branchService;
            _userContext = userContextService;
            _branchUser = branchUserService;
        }
        public async Task<IActionResult> Clients(PersonFilterDto filter)
        {
            var branch = await _branch.FindBranchByIdAsync(_userContext.BranchId.Value);
            var user = await _branchUser.GetBUserByUsernameAsync(User.Identity.Name);
            if (!_userContext.SellerId.HasValue || user == null || branch == null) return View();
            return View();
        }
    }
}
