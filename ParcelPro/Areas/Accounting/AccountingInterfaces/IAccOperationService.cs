using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.Accounting.Dto.PrintDto;
using ParcelPro.Areas.Accounting.Models.Entities;

namespace ParcelPro.Areas.Accounting.AccountingInterfaces
{
    public interface IAccOperationService
    {

        Task<int> DocNumberGeneratorAsync(long SellerId, int PeriodId);
        Task<int> DocAutoNumberGeneratorAsync(long SellerId, int PeriodId);
        Task<bool> IsDupplicateDocNumberAsync(long SellerId, int PeriodId, int number);
        Task<bool> IsDupplicateDocNumberAsync(DocDto dto);

        //=====
        Task<List<DocDto>> GetDocsAsync(DocFilterDto filter);
        IQueryable<DocDto> GetDocs(DocFilterDto filter);
        Task<List<DocDto>> GetDeletedDocsAsync(DocFilterDto filter);
        Task<Acc_Document?> FindDocAsync(Guid id);
        Task<DocDto> GetDocDtoAsync(Guid id);
        Task<DocDto> GetLastUserDocAsync(long sellerId, int periodId, string username);
        Task<DocDto> GetDocWithNumberAsync(long sellerId, int priodId, int docNumber);
        Task<clsResult> CreateDocHeaderAsync(DocDto_AddNew dto);
        Task<clsResult> SaveDraftDocHeaderAsync(DocDto dto);
        Task<clsResult> DeleteDocAsync(Guid Id, string username);
        Task<clsResult> DeleteDocsAsync(Guid[] Ids, string username);
        Task<clsResult> BulkDeleteDocumentsAsync(Guid[] ids, string username);
        Task<clsResult> DeleteAllPeriodDocAsync(long sellerId, int periodId);
        Task<clsResult> UndeleteDocAsync(Guid Id, string username);

        //Article
        Task<clsResult> AddArticleAsync(DocArticleDto dto);
        Task<clsResult> AddRangeArticlesAsync(List<DocArticleDto> arts);
        Task<clsResult> UpdateArticleAsync(DocArticleDto dto);
        Task<DocArticleDto> GetDocArticleDtoAsync(Guid Id);
        Task<clsResult> DeleteDocArticleAsync(Guid Id);
        Task<clsResult> DeleteDocArticlesAsync(List<Guid> Ids, string userName);
        Task<clsResult> ArticlesRenumberRows(List<SetNumberDto> articlesList, string editorUserName);

        //
        Task<MoeinStatusDto> GetMoeinStatusByIdAsync(int moeinId, long sellerId, int periodId);
        Task<bool> DocsSortingAsync(long sellerId, int periodId, Guid[] DocsId, int startNumber);
        Task<clsResult> InsertSystemicDocAsync(List<DocArticleDto> articles, string docDesc, short docType);
        Task<clsResult> InsertBulkDocAsync(List<DocArticleDto> articles, string docDesc, short docType);
        Task<List<DocMerge_Article>> GetMergeDocsArticlesAsync(Guid[] Docs, bool mergeAccount = false, bool keepTafsil = true);
        Task<clsResult> AddMergedDocsAsync(DocMerge_Header dto);
        Task<clsResult> CopyDocsAsync(DocMerge_Header dto);
        //
        Task<DocPrintDto> GetDocPrintAsync(Guid id);
        Task<StructuredDocPrintDto> GetStructuredDocPrintAsync(Guid id);
        Task<DocPrintDto> GetDocPrintKolAsync(Guid id);
        Task<DocPrintDto> GetDocPrintMoeinAsync(Guid id);
        byte[] GenerateAccountingDocumentExcel(DocDto doc);

    }
}
