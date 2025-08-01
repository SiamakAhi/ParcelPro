using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Client.ClientInterfacses;
using ParcelPro.Areas.Client.Dto;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Services;
using System.Security.Claims;

namespace ParcelPro.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class ClientPanelController : Controller
    {
        private readonly UserContextService _userContext;
        private readonly IClientPanelService _client;
        public ClientPanelController(UserContextService userContextService, IClientPanelService clientPanelService)
        {
            _userContext = userContextService;
            _client = clientPanelService;
        }
        public async Task<IActionResult> UserPanel(WaybillFilterDto filter)
        {
            //if (!_userContext.SellerId.HasValue)
            //    return NoContent();
            // تنظیم مدل اصلی
            var model = new ClientPanelViewModel();
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userinfo = await _client.GetClientUserInfoAsync(userid);
            filter.SellerId = 3;
            model.filter = filter;
            model.filter.SenderId = userinfo.PartyId;
            ViewBag.Customers = await _client.SelectList_ClientCustomers(userinfo.PartyId);
            // ViewBag.Routes = await _client.SelectList_ClientRoute(userinfo.PartyId);
            ViewBag.Provinces = await _client.SelectList_ClientUsedProvincesAsync(userinfo.PartyId);
            ViewBag.Cities = await _client.SelectList_ClientUsedCitiesAsync(userinfo.PartyId);

            // دریافت بارنامه‌های صادره
            if (filter.IsFromBody)
            {
                var billsOut = _client.GetClientWaybillsAsQuery(filter);
                model.Bills = Pagination<ViewBillOfLadings>.Create(billsOut, filter.CurrentPage, filter.PageSize);
            }
            return View(model);
        }
        public async Task<IActionResult> ClientReports(WaybillFilterDto filter)
        {
            //if (!_userContext.SellerId.HasValue)
            //    return NoContent();
            // تنظیم مدل اصلی
            var model = new ClientPanelViewModel();
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userinfo = await _client.GetClientUserInfoAsync(userid);
            filter.SellerId = 3;
            model.filter = filter;
            model.filter.SenderId = userinfo.PartyId;
            ViewBag.Customers = await _client.SelectList_ClientCustomers(userinfo.PartyId);
            // ViewBag.Routes = await _client.SelectList_ClientRoute(userinfo.PartyId);
            ViewBag.Provinces = await _client.SelectList_ClientUsedProvincesAsync(userinfo.PartyId);
            ViewBag.Cities = await _client.SelectList_ClientUsedCitiesAsync(userinfo.PartyId);

            // دریافت بارنامه‌های صادره
            if (filter.IsFromBody)
            {
                var billsOut = _client.GetClientWaybillsAsQuery(filter);
                model.Bills = Pagination<ViewBillOfLadings>.Create(billsOut, filter.CurrentPage, filter.PageSize);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetCities(int? pid)
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userinfo = await _client.GetClientUserInfoAsync(userid);

            var cities = await _client.GetCitiesAsync(userinfo.PartyId, pid);
            return Json(cities);
        }
    }
}
