using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.Accounting.Models.Entities;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccAsistantsService
    {
        Task<clsResult> InsertMoadianReportAsync(long sellerId, List<Acc_MoadianReport> report);
        Task<List<Acc_MoadianReport>> ReadMoadianReportFromExcelAsync(IFormFile file);
        Task<BulkDocDto> PreparingToCreateMoadianDocAsync(List<Acc_MoadianReport> report, bool isSale, long sellerId, int periodId, string currentUser);
        Task<clsResult> InsertBulkDocsAsync(BulkDocDto dto);
        Task<clsResult> BankTransactionSaveAsCheckedAsync(List<long> items);

    }
}
