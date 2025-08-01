using ParcelPro.Areas.Commercial.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParcelPro.Areas.Commercial.ComercialInterfaces
{
    public interface IInvoiceService
    {
        SelectList SelectList_SettelmentType();
        IQueryable<InvoiceHeaderDto> GetInvoices(InvoiceFilterDto filter);
        Task<List<InvoiceHeaderDto>> GetSelectedInvoicesAsync(Guid[] ids);
        Task<clsResult> DeleteDuplacatedInvoicesAsync(Guid[] ids);
        Task<string> GenerateSaleInvoiceNumberAsync(long sellerId);
        Task<clsResult> CreateInvoiceHeaderAsync(InvoiceHeaderDto dto);
        Task<clsResult> UpdateInvoiceHeaderAsync(InvoiceHeaderDto dto);
        Task<InvoiceDto> GetInvoiceByIdAsync(Guid invoiceId);
        Task<clsResult> AddInvoiceItemAsync(InvoiceItemDto itemDto);
        Task<InvoiceItemDto> GetInvoiceItemByIdAsync(Guid id);
        Task<clsResult> updateInvoiceItemAsync(InvoiceItemDto dto);
        Task<clsResult> DeleteInvoiceItemAsync(Guid Id);
        Task<clsResult> DeleteInvoiceAsync(Guid Id);
        Task<List<CreateIncoiceDto>> PrepareInvoiceToCreate_AtiranAsync(InvoiceImportDto_Atiran rawData);
        Task<clsResult> CreateInvoiceInBulkAsync(List<CreateIncoiceDto> invoices);
        Task<List<InvoiceDto>> GetInvoicesFuulDataAsync(Guid[] ids);
        Task<clsResult> TagInvoicesAsync(Guid[] ids);
        Task<clsResult> UnTagInvoicesAsync(Guid[] ids);
        Task<clsResult> TagTogglerInvoicesAsync(Guid id);
        Task<clsResult> CopyInvoicesAsync(CoppyInvoiceSettingDto filter);
        //Report
        Task<List<InvoiceHeaderDto>> GetInvoicesGroupedByCustomer(InvoiceFilterDto filter);
    }
}
