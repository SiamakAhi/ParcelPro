using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Client.ClientInterfacses;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Services;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Mvc;

namespace ParcelPro.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class ClientReportsController : Controller
    {
        private readonly UserContextService _userContext;
        private readonly IClientPanelService _client;
        public ClientReportsController(UserContextService userContextService, IClientPanelService panelService)
        {

            _userContext = userContextService;
            _client = panelService;
            StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHkgpgFGkUl79uxVs8X+uspx6K+tqdtOB5G1S6PFPRrlVNvMUiSiNYl724EZbrUAWwAYHlGLRbvxMviMExTh2l9xZJ2xc4K1z3ZVudRpQpuDdFq+fe0wKXSKlB6okl0hUd2ikQHfyzsAN8fJltqvGRa5LI8BFkA/f7tffwK6jzW5xYYhHxQpU3hy4fmKo/BSg6yKAoUq3yMZTG6tWeKnWcI6ftCDxEHd30EjMISNn1LCdLN0/4YmedTjM7x+0dMiI2Qif/yI+y8gmdbostOE8S2ZjrpKsgxVv2AAZPdzHEkzYSzx81RHDzZBhKRZc5mwWAmXsWBFRQol9PdSQ8BZYLqvJ4Jzrcrext+t1ZD7HE1RZPLPAqErO9eo+7Zn9Cvu5O73+b9dxhE2sRyAv9Tl1lV2WqMezWRsO55Q3LntawkPq0HvBkd9f8uVuq9zk7VKegetCDLb0wszBAs1mjWzN+ACVHiPVKIk94/QlCkj31dWCg8YTrT5btsKcLibxog7pv1+2e4yocZKWsposmcJbgG0";
        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        public IActionResult RpWaybillsByRecivers()
        {
            return View();
        }
        public IActionResult GetReport_RpWaybillsBtyRecivers(
            long? _ReciverId
            ,int? _RoutId
            ,Guid? _Distributer
            ,int? _OriginCityId
            ,int? _DestinationCityId
            ,short? _SettelmentType
            ,short? _BillStatus
            ,short? _PaymentStatus
            ,string? _IssuerUserName
            ,string? _BiilOdLadingNumber
            ,string? _strFromDate
            ,string? _strUntilDate
            ,bool? _ShowCancelation
            ,short? _personSearchtype
            )
        {

            return View();
        }

    }
}
