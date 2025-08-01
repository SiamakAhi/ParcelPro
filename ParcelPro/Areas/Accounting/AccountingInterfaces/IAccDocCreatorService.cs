using ParcelPro.Areas.Accounting.Dto;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccDocCreatorService
    {
        Task<clsResult> CreateBankDocAsync(BankTransactionsCreateDocDto dto);

    }
}
