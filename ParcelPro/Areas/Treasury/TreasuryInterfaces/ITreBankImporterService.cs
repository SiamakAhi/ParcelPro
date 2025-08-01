using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Treasury.Dto;

namespace ParcelPro.Areas.Treasury.TreasuryInterfaces
{
    public interface ITreBankImporterService
    {
        SelectList Select_list_BankReportType();
        IQueryable<TreBankTransactionDto> GetAllTreBankTransactions(long sellerId, long accountId, DateTime fromDate, DateTime untilDate);
        Task<List<TreBankTransactionDto>> GetBankTransactionsAsync(BankReportFilterDto filter);
        Task<clsResult> ImportSamanKotahModatAsync(BankImporterDto dto);
        Task<clsResult> ImportSamanAsync(BankImporterDto dto);
        Task<clsResult> ImportTejaratAsync(BankImporterDto dto);
        Task<clsResult> ImportTejaratInternetBankAsync(BankImporterDto dto);
        Task<clsResult> ImportMelatAsync(BankImporterDto dto);
        Task<clsResult> ImportEghtesadNovinAsync(BankImporterDto dto);
        Task<clsResult> ImportEghtesadInternetBankAsync(BankImporterDto dto);
        Task<clsResult> ImportKeshavarziAsync(BankImporterDto dto);
        Task<clsResult> ImportRefahJariAsync(BankImporterDto dto);
        Task<clsResult> ImportCityBankAsync(BankImporterDto dto);
        Task<clsResult> ImportPostBankAsync(BankImporterDto dto);
        Task<clsResult> ImportSaderat_SepehrAsync(BankImporterDto dto);
        Task<clsResult> ImportSaderatAsync(BankImporterDto dto);
        Task<clsResult> ImportSepahAsync(BankImporterDto dto);
        Task<clsResult> ImportBankMeliAsync(BankImporterDto dto);
    }
}
