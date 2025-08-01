using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.DataTransfer.Dto;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccImportService
    {
        Task<clsResult> GetCodingFromExcelAsync(IFormFile excelFile, long sellerId);
        List<ImportDocDto> GetDocFromExl_Sepidar(IFormFile excelFile);
        List<ImportDocDto> GetDocFromExl_General(IFormFile excelFile);
        List<ImportDocDto> AssignDocumentNumbers(List<ImportDocDto> documents, long sellerId, int periodId);
        Task<clsResult> AddBulkDocsAsync(List<ImportDocDto> documents, string userName, long sellerId, int peropdId);
        Task<clsResult> AddBulkKpDocsAsync(List<ImportSaleDocDto> documents, string userName, long sellerId, int peropdId, int? subsystemId = null);
    }
}
