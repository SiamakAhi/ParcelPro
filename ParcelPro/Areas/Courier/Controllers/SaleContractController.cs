using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto.ContractDto;
using ParcelPro.Interfaces;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.Controllers
{
    [Area("Courier")]
    [Authorize(Roles = "AccountingManager")]
    public class SaleContractController : Controller
    {
        private readonly ICu_SaleContractService _contract;
        private readonly UserContextService _userContext;
        private readonly IPersonService _persen;

        public SaleContractController(ICu_SaleContractService contract, IPersonService persen, UserContextService userContextService)
        {
            _contract = contract;
            _userContext = userContextService;
            _persen = persen;
        }


        [HttpGet]
        public async Task<IActionResult> Contracts()
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("شناسه فروشنده یافت نشد");

            var data = await _contract.GetContracts(_userContext.SellerId.Value);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var data = await _contract.GetContractById(id);
            if (data == null)
                return NotFound("قرارداد یافت نشد");

            return PartialView("_DetailSaleContract", data);
        }

        [HttpGet]
        public async Task<IActionResult> AddSaleContract()
        {
            ViewBag.clients = await _persen.SelectList_CreditClient(_userContext.SellerId ?? 0);
            return PartialView("_AddSaleContract");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddSaleContract(SaleContractDto dto)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }

            dto.SellerId = _userContext.SellerId.Value;

            if (ModelState.IsValid)
            {
                result = await _contract.AddContract(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(SaleContractDto dto)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }

            dto.SellerId = _userContext.SellerId.Value;

            if (ModelState.IsValid)
            {
                result = await _contract.UpdateContract(dto);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contract.DeleteContract(id);
            result.ShowMessage = true;
            result.returnUrl = Request.Headers["Referer"].ToString();
            result.updateType = 1;
            return Json(result.ToJsonResult());
        }

        [HttpGet]
        public async Task<IActionResult> ContractsUsers()
        {
            if (!_userContext.SellerId.HasValue)
                return BadRequest("شناسه فروشنده یافت نشد");

            var data = await _contract.ClientUsersAsync(_userContext.SellerId.Value);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> AddClientUser(int contractId)
        {
            if (!_userContext.SellerId.HasValue || !_userContext.CustomerId.HasValue)
                return BadRequest("شناسه فروشنده یافت نشد");
            AddClientUserDto model = new AddClientUserDto();
            model.CustomerId = _userContext.CustomerId.Value;
            model.SellerId = _userContext.SellerId.Value;
            model.DepartmentCode = 201;
            ViewBag.Contracts = await _contract.SelectList_ContractsAsync(_userContext.SellerId.Value);
            return View("_AddClientUser", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddClientUser(AddClientUserDto model)
        {
            clsResult result = new clsResult();

            if (!_userContext.SellerId.HasValue || !_userContext.CustomerId.HasValue)
            {
                result.Message = "دسترسی به شرکت فعال یافت نشد";
                return Json(result.ToJsonResult());
            }
            model.CustomerId = _userContext.CustomerId.Value;
            model.SellerId = _userContext.SellerId.Value;
            model.DepartmentCode = 201;
            model.IdentityRols = new string[] { "OnlineShop" };

            if (ModelState.IsValid)
            {
                result = await _contract.AddClidentUserAsync(model);
                if (result.Success)
                {
                    result.ShowMessage = true;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    result.updateType = 1;
                    return Json(result.ToJsonResult());
                }
            }

            var ModelErrors = ModelState.Values.SelectMany(n => n.Errors).ToList();
            foreach (var er in ModelErrors)
            {
                result.Message += "\n" + er.ErrorMessage;
            }

            return Json(result.ToJsonResult());
        }

    }
}
