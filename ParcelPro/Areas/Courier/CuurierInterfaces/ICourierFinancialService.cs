using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Accounting.Dto.SaleManagementDtos;
using ParcelPro.Areas.Courier.Dto.FinancialDtos;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface ICourierFinancialService
    {
        Task<SelectList> SelectList_CreditCustomersAsync(long SellerId);
        Task<FinancialTransactionDto> GetBillDataAsync(Guid BillOdLadingId, int SettelmentType, string UserId);
        Task<bool> RegisterFinancialTransactionAsync(FinancialTransactionDto dto);
        Task<clsResult> SetSettelmentAsync(FinancialTransactionDto dto);
        Task<long> GetRemainingAmountByIdAsync(Guid billId);
        Task<long> GetRemainingAmountByNumAsync(string billNumber);

        IQueryable<AccBillViewModel> GetBillsFinanceAsQuery(AccSalesFilterDto filter);
        Task<List<CreditAccountBill>> FetchCurrentDebtStatusAsync(AccSalesFilterDto filter);
        IQueryable<SaleMoneyTransaction> GetSaleMoneyTransactionsAsync(SaleMoneyTransactionFilterDto filter);

        Task<clsResult> MoveToAnotherAccountAsync(MoveAccountDto dto);
        Task<List<AccBillViewModel>> GetPartyBillsAsync(PartyBillsFilterDto filter);
        Task<List<MoadianExportDto>> CreateMoadiansAsync(PartyBillsFilterDto filter);
        Task<clsResult> CHangeBillPartyAsync(ChangePartyDto dto);
    }
}
