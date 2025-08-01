using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Services;

namespace ParcelPro.Controllers
{
    [Authorize]
    public class basedataController : Controller
    {
        private readonly IAccGetBaseDataService _baseData;
        private readonly IPersonService _person;

        private readonly UserContextService _userContext;
        public basedataController(IAccGetBaseDataService baseDataService, UserContextService userContext, IPersonService person)
        {
            _baseData = baseDataService;
            _userContext = userContext;
            _person = person;
        }

        [HttpPost]
        public async Task<IActionResult> GetTafsilByGroup(int? groupId)
        {
            if (_userContext.SellerId == null) return Ok();
            var kols = await _baseData.GetTafsilByTafsilGroupAsync(_userContext.SellerId.Value, groupId);
            return Json(kols);
        }
        [HttpPost]
        public async Task<IActionResult> GetKolsByTafsil(List<long>? items)
        {
            if (_userContext.SellerId == null) return Ok();
            var kols = await _baseData.GetUsedKolsByTafsilAsync(_userContext.SellerId.Value, items);
            return Json(kols);
        }

        [HttpPost]
        public async Task<IActionResult> GetMoeinsByKolAndTafsil(List<long>? tafsils, List<int>? kols)
        {
            if (_userContext.SellerId == null) return Ok();
            var moeins = await _baseData.GetUsedMoeinsByKolAndTafsilAsync(_userContext.SellerId.Value, tafsils, kols);
            return Json(moeins);
        }
        [HttpPost]
        public async Task<IActionResult> GetMoeinsByKols(List<int>? items)
        {
            if (_userContext.SellerId == null) return Ok();
            var moeins = await _baseData.GetUsedMoeinsByKolsAsync(_userContext.SellerId.Value, items);
            return Json(moeins);
        }

        [HttpPost]
        public async Task<IActionResult> GetPersonInfo(long id)
        {

            var info = await _person.GetPersonDtoAsync(id);
            return Json(info);
        }
    }
}
