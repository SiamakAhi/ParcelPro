using ParcelPro.Areas.Treasury.Dto;

namespace ParcelPro.Areas.Treasury.TreasuryInterfaces
{
    public interface IMoneyTransactionService
    {
        Task<Guid> getBillIdByNumberAsync(string BillNumber);
        Task<bool> CreateTransactionAsync(TransactionDto transactionDto);
        Task<clsResult> BillOfLadingOnlinePaymentAsync(string billNumber, string transId, string idGet, string providerName);
        Task<clsResult> BillOfLadingCashPaymentAsync(BillCashPayDto model);
        Task<clsResult> SendPaymentLinkAsync(BillCashPayDto model, string linkAddress);

    }
}
