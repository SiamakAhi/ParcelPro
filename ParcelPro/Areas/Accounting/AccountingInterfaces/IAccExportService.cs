using ParcelPro.Areas.Accounting.Dto;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccExportService
    {
        Task<byte[]> Export_browserKolAsync(DocFilterDto filter);
        Task<byte[]> Export_browserMoeinAsync(DocFilterDto filter);
        Task<byte[]> Export_browserTafsilAsync(DocFilterDto filter);
        Task<byte[]> Export_DocArticlesAsync(DocFilterDto filter);
    }
}
