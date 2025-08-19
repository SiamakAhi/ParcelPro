using BitPayDll;

namespace ParcelPro.Areas.Treasury.TreasuryServices
{
    public class BitPayService
    {
        public string SendPayment(string amount, string factorId, string Redirect, string name = "", string email = "", string description = "")
        {
            BitPay bitpay = new BitPay();
            string url = "https://bitpay.ir/payment/gateway-send";

            string api = "fe41e-9df61-908ef-2e9dc-9b7ba7e0bc328dc169c3cee17b93";
            //Test Api
            // string api = "adxcv-zzadq-polkjsad-opp13opoz-1sdf455aadzmck1244567";
            int result = bitpay.Send(url, api, amount, Redirect, factorId, name, email, description);
            if (result > 0)
            {
                return string.Format("https://bitpay.ir/payment/gateway-{0}-get", result);
            }

            return null; // or handle error appropriately
        }

        public int GetPaymentResult(string transId, string idGet, string factorId)
        {
            BitPay bitpay = new BitPay();
            string url = "https://bitpay.ir/payment/gateway-result-second";
            string api = "fe41e-9df61-908ef-2e9dc-9b7ba7e0bc328dc169c3cee17b93";

            //test Api
            //string api = "adxcv-zzadq-polkjsad-opp13opoz-1sdf455aadzmck1244567";

            int result = bitpay.Get(url, api, transId, idGet);

            return result;

        }
    }
}
