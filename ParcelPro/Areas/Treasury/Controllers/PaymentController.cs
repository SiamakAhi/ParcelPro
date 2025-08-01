using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Treasury.Dto;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Areas.Treasury.TreasuryServices;
using ParcelPro.Services;

namespace ParcelPro.Areas.Treasury.Controllers
{
    [Area("Treasury")]
    [Authorize]
    public class PaymentController : Controller
    {

        private readonly BitPayService _bitpay;
        private readonly UserContextService _user;
        private readonly IMoneyTransactionService _moneyTransactionService;
        private readonly ICourierFinancialService _courierFinancialService;

        public PaymentController(BitPayService bitPayService,
            IMoneyTransactionService moneyTransactionService,
            ICourierFinancialService courierFinancialService,
            UserContextService userContext)
        {
            _bitpay = bitPayService;
            _moneyTransactionService = moneyTransactionService;
            _courierFinancialService = courierFinancialService;
            _user = userContext;
        }

        [AllowAnonymous]
        public async Task RequestPayment(string Amount, string FactorId, string Name = "", string Email = "", string Description = "")
        {
            long amount = await _courierFinancialService.GetRemainingAmountByNumAsync(FactorId);
            if (amount <= 0)
            {
                ViewBag.PaymentResult = "این فاکتور قبلا پرداخت شده است";
                string url = Request.Headers["Referer"].ToString();
                Response.Redirect(url);
            }
            string Redirect = @"https://hub.keyhanpost.ir" + Url.Action("PaymentCallback", "Payment", new { Area = "Treasury" });
            //string Redirect = @"https://localhost:44353" + Url.Action("PaymentCallback", "Payment", new { Area = "Treasury" });

            string result = _bitpay.SendPayment(amount.ToString(), FactorId, Redirect, Name, Email, Description);
            Response.Redirect(result);
        }

        [AllowAnonymous]
        public async Task<IActionResult> PaymentCallback(int trans_id, int id_get, string factorId)
        {
            int result = _bitpay.GetPaymentResult(trans_id.ToString(), id_get.ToString(), factorId);
            Guid Id = await _moneyTransactionService.getBillIdByNumberAsync(factorId);

            if (result == 1)
            {
                var RegisterPayment = await _moneyTransactionService.BillOfLadingOnlinePaymentAsync(factorId, trans_id.ToString(), id_get.ToString(), "BitPay");
                ViewBag.PaymentResult = RegisterPayment.Message;
            }
            else
            {
                ViewBag.PaymentResult = "خطایی در پرداخت رخ داده است";
            }

            return RedirectToAction("bill", "Billoflading", new { Area = "Courier", billOfladingId = Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BillCashPay(BillCashPayDto model)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (model.DebitAmount <= 0)
            {
                result.Message = "بارنامه تسویه شده و فاقد بدهی می باشد.";
                return Json(result.ToJsonResult());
            }

            // پرداخت از طریق کارتخوان
            if (model.OperationId == 2)
            {
                if (model.PosId == null || model.PosId == 0)
                {
                    result.Message = "انتخاب دستگاه کارت خوان الزامی است";
                    return Json(result.ToJsonResult());
                }
            }
            // پرداخت از طریق حواله بانکی
            if (model.OperationId == 3)
            {

                if (string.IsNullOrEmpty(model.TransferNumber))
                {
                    result.Message = "شماره حواله یا رسید بانکی را وارد کنید";
                    return Json(result.ToJsonResult());
                }
                if (model.BankAccountId == null || model.BankAccountId == 0)
                {
                    result.Message = "حساب بانک مقصد را مشخص کنید";
                    return Json(result.ToJsonResult());
                }
            }

            if (model.OperationId == 601)
            {
                string payLink = "https://hub.keyhanpost.ir" + Url.Action("RequestPayment", "Payment", new { Area = "Treasury", Amount = model.DebitAmount.ToString(), FactorId = model.BillNumber, Name = "", Email = "", Description = "" });

                result = await _moneyTransactionService.SendPaymentLinkAsync(model, payLink);
                if (result.Success)
                {
                    result.Success = true;
                    result.updateType = 1;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
                return Json(result.ToJsonResult());

            }

            if (!string.IsNullOrEmpty(model.strPayAmount))
                model.PayAmount = Convert.ToInt64(model.strPayAmount.Replace(",", ""));
            if (!string.IsNullOrEmpty(model.strTransactionDate))
                model.TransactionDate = model.strTransactionDate.PersianToLatin();


            if (ModelState.IsValid)
            {
                result = await _moneyTransactionService.BillOfLadingCashPaymentAsync(model);
                if (result.Success)
                {
                    result.Success = true;
                    result.updateType = 1;
                    result.returnUrl = Request.Headers["Referer"].ToString();
                    return Json(result.ToJsonResult());
                }
            }
            var erroes = ModelState.Values.SelectMany(e => e.Errors).ToList();
            foreach (var error in erroes)
            {
                result.Message += "\n" + error.ErrorMessage;
            }
            return Json(result.ToJsonResult());

        }


    }
}
