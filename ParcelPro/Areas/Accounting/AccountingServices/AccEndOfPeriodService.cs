﻿using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.Accounting.Models.Entities;
using ParcelPro.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ParcelPro.Areas.Accounting.AccountingServices
{
    public class AccEndOfPeriodService : IAccEndOfPeriodService
    {
        private readonly AppDbContext _db;
        private readonly IAccOperationService _op;
        public AccEndOfPeriodService(AppDbContext db, IAccOperationService op)
        {
            _db = db;
            _op = op;
        }

        //لیست گروه های حساب
        public async Task<SelectList> SelectList_GroupAccountsAsync(long sellerId, Int16 typeid)
        {
            var accounts = await _db.Acc_Coding_Groups.Where(n => n.SellerId == sellerId && n.TypeId == typeid)
               .Select(n => new { id = n.Id, name = n.GroupCode + " - " + n.GroupName, code = n.GroupCode }).OrderBy(n => n.code).ToListAsync();

            return new SelectList(accounts, "id", "name");
        }
        // لیست سرفصل های موقت
        public async Task<SelectList> SelectList_TemporaryAccounts_KolAsync(long sellerId)
        {
            var accounts = await _db.Acc_Coding_Kols.Where(n => n.SellerId == sellerId && n.KolGroup.TypeId == 2)
               .Select(n => new { id = n.Id, name = n.KolCode + " - " + n.KolName, code = n.KolCode }).OrderBy(n => n.code).ToListAsync();

            return new SelectList(accounts, "id", "name");
        }
        // لیست معین های موقت
        public async Task<SelectList> SelectList_TemporaryAccounts_MoeinAsync(long sellerId)
        {
            var accounts = await _db.Acc_Coding_Moeins.Where(n => n.SellerId == sellerId && n.MoeinKol.KolGroup.TypeId == 2)
               .Select(n => new { id = n.Id, name = n.MoeinCode + " - " + n.MoeinName, code = n.MoeinCode }).OrderBy(n => n.code).ToListAsync();

            return new SelectList(accounts, "id", "name");
        }
        // لیست سرفصل های دائم
        public async Task<SelectList> SelectList_PermanentAccounts_KolAsync(long sellerId)
        {
            var accounts = await _db.Acc_Coding_Kols.Where(n => n.SellerId == sellerId && n.KolGroup.TypeId == 1)
                .Select(n => new { id = n.Id, name = n.KolCode + " - " + n.KolName, code = n.KolCode }).OrderBy(n => n.code).ToListAsync();

            return new SelectList(accounts, "id", "name");
        }
        // لیست معین های دائم
        public async Task<SelectList> SelectList_PermanentAccounts_MoeinAsync(long sellerId)
        {
            var accounts = await _db.Acc_Coding_Moeins.Where(n => n.SellerId == sellerId && n.MoeinKol.KolGroup.TypeId == 1)
               .Select(n => new { id = n.Id, name = n.MoeinCode + " - " + n.MoeinName, code = n.MoeinCode }).OrderBy(n => n.code).ToListAsync();

            return new SelectList(accounts, "id", "name");
        }
        public async Task<SelectList> SelectList_AllAccounts_MoeinAsync(long sellerId)
        {
            var accounts = await _db.Acc_Coding_Moeins.Where(n => n.SellerId == sellerId)
               .Select(n => new { id = n.Id, name = n.MoeinCode + " - " + n.MoeinName, code = n.MoeinCode }).OrderBy(n => n.code).ToListAsync();

            return new SelectList(accounts, "id", "name");
        }
        //--------------------------

        public bool IsDocumentBalanced(Acc_Document document)
        {
            long totalBed = document.DocArticles.Sum(a => a.Bed);
            long totalBes = document.DocArticles.Sum(a => a.Bes);

            return totalBed == totalBes;
        }
        public bool AreDocumentsBalanced(IEnumerable<Acc_Document> documents)
        {
            foreach (var document in documents)
            {
                long totalBed = document.DocArticles.Sum(a => a.Bed);
                long totalBes = document.DocArticles.Sum(a => a.Bes);

                if (totalBed != totalBes)
                {
                    return false;
                }
            }
            return true;
        }

        // Method to check if all documents are balanced
        public async Task<bool> AreDocumentsBalancedAsync(long sellerId, int periodId)
        {
            // Fetch all documents with their articles
            var documents = await _db.Acc_Documents.Include(d => d.DocArticles)
                .Where(d => d.StatusId == sellerId && d.PeriodId == periodId && d.IsDeleted == false).ToListAsync();
            foreach (var document in documents)
            {
                // Calculate the sum of Bed and Bes for the document's articles
                long totalBed = document.DocArticles.Sum(a => a.Bed);
                long totalBes = document.DocArticles.Sum(a => a.Bes);

                // Check if the document is balanced
                if (totalBed != totalBes)
                {
                    return false; // If any document is not balanced, return false
                }
            }
            return true; // If all documents are balanced, return true
        }

        // Method to check if document dates are in correct order
        public async Task<bool> AreDocumentDatesInOrderAsync(long sellerId, int periodId)
        {
            var documents = await _db.Acc_Documents
                 .Where(d => d.StatusId == sellerId && d.PeriodId == periodId && d.IsDeleted == false)
                 .OrderBy(d => d.DocNumber).ToListAsync();

            for (int i = 1; i < documents.Count; i++)
            {
                if (documents[i].DocDate < documents[i - 1].DocDate)
                {
                    return false; // If any document date is less than the previous one, return false
                }
            }
            return true; // If all document dates are in correct order, return true
        }

        //Method to che all documents are final
        public async Task<clsResult> AreAllDocumentsApprovedAsync(long sellerId, int periodId)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;
            var documents = await _db.Acc_Documents
                .Where(d => d.StatusId == sellerId && d.PeriodId == periodId && d.IsDeleted == false)
                .ToListAsync();

            var notApprovedDocs = documents.Where(d => d.StatusId != 3).Select(d => d.DocNumber).ToList();
            if (notApprovedDocs.Count > 0)
            {
                result.Message = "اسناد زیر نهایی نشده‌اند: " + string.Join(", ", notApprovedDocs);
                return result;
            }
            result.Success = true;
            result.Message = "همه اسناد نهایی شده‌اند";
            return result;
        }

        public async Task<clsResult> UpdateDocumentStatusAsync(Guid documentId, short newStatusId, string userName)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            // Fetch the document by ID
            var document = await _db.Acc_Documents
                .Include(d => d.DocArticles)
                .SingleOrDefaultAsync(d => d.Id == documentId && !d.IsDeleted);

            if (document == null)
            {
                result.Message = "سند یافت نشد یا حذف شده است.";
                return result;
            }

            // Check if the document needs to be balanced
            if ((newStatusId == 2 || newStatusId == 3) && !IsDocumentBalanced(document))
            {
                result.Message = "سند تراز نیست و نمی‌توان وضعیت آن را تغییر داد.";
                return result;
            }

            // Update the status and other fields
            document.StatusId = newStatusId;
            document.LastUpdateDate = DateTime.Now;
            document.EditorUserName = userName;

            try
            {
                if (await _db.SaveChangesAsync() > 0)
                {
                    result.Success = true;
                    result.Message = "وضعیت سند با موفقیت تغییر یافت.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "خطایی در زمان تغییر وضعیت سند رخ داده است. /n" + ex.Message;
            }

            return result;
        }
        public async Task<clsResult> UpdateDocumentsStatusAsync(Guid[] documentIds, short newStatusId, string userName)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            // Fetch the documents by IDs
            var documents = await _db.Acc_Documents
                .Include(d => d.DocArticles)
                .Where(d => documentIds.Contains(d.Id) && !d.IsDeleted)
                .ToListAsync();

            if (documents.Count != documentIds.Length)
            {
                result.Message = "یکی یا چند سند یافت نشد یا حذف شده‌اند.";
                return result;
            }

            // Check if the documents need to be balanced
            if ((newStatusId == 2 || newStatusId == 3) && !AreDocumentsBalanced(documents))
            {
                result.Message = "یکی یا چند سند تراز نیستند و نمی‌توان وضعیت آن‌ها را تغییر داد.";
                return result;
            }

            // Update the status and other fields
            var documentIdsToUpdate = documents.Select(d => d.Id).ToArray();
            try
            {
                var affectedRows = await _db.Acc_Documents
                    .Where(d => documentIdsToUpdate.Contains(d.Id))
                    .ExecuteUpdateAsync(d => d
                        .SetProperty(doc => doc.StatusId, newStatusId)
                        .SetProperty(doc => doc.LastUpdateDate, DateTime.Now)
                        .SetProperty(doc => doc.EditorUserName, userName));

                if (affectedRows > 0)
                {
                    result.Success = true;
                    result.Message = "وضعیت اسناد با موفقیت تغییر یافت.";
                }
            }
            catch (Exception ex)
            {
                result.Message = "خطایی در زمان تغییر وضعیت اسناد رخ داده است. /n" + ex.Message;
            }

            return result;
        }


        public async Task<clsResult> FinalizeAllDocumentsAsync(long sellerId, int periodId, string userName)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            // Check if all documents are balanced for the specified seller and period
            if (!await AreDocumentsBalancedAsync(sellerId, periodId))
            {
                result.Message = "همه اسناد تراز نیستند";
                return result;
            }

            // Fetch all documents for the specified seller and period
            var documents = await _db.Acc_Documents
                .Where(d => d.SellerId == sellerId && d.PeriodId == periodId && d.IsDeleted == false)
                .ToListAsync();

            foreach (var document in documents)
            {
                document.StatusId = 3; // Change status to finalized
                document.LastUpdateDate = DateTime.Now;
                document.EditorUserName = userName;
            }

            try
            {
                if (await _db.SaveChangesAsync() > 0)
                {
                    result.Success = true;
                    result.Message = "وضعیت همه اسناد به نهایی تغییر یافت";
                }
            }
            catch (Exception ex)
            {
                result.Message = "خطایی در زمان تغییر وضعیت اسناد رخ داده است. /n" + ex.Message;
            }

            return result;
        }

        public async Task<List<Report_BrowserDto>> AccountsBanaceAsync(long sellerId, int periodId, List<int> accountsId)
        {
            List<Report_BrowserDto> accounts = new List<Report_BrowserDto>();


            var query = _db.Acc_Articles
                .Include(n => n.Doc)
                .Include(n => n.Moein).ThenInclude(n => n.MoeinKol).ThenInclude(n => n.KolGroup)
                .Where(n =>
                (accountsId.Contains(n.MoeinId))
                && (n.IsDeleted == false && n.Doc.IsDeleted == false)
                && n.Doc.SellerId == sellerId
                && n.PeriodId == periodId)
                .AsQueryable();

            accounts = await query.GroupBy(n => n.MoeinId)
                .Select(n => new Report_BrowserDto
                {
                    MoeinId = n.Key,
                    GroupId = n.Max(x => x.Moein.MoeinKol.GroupId),
                    GroupName = n.Max(x => x.Moein.MoeinKol.KolGroup.GroupName),
                    KolId = n.Max(x => x.Moein.KolId),
                    KolCode = n.Max(x => x.Moein.MoeinKol.KolCode),
                    KolName = n.Max(x => x.Moein.MoeinKol.KolName),
                    MoeinCode = n.Max(x => x.Moein.MoeinCode),
                    MoeinName = n.Max(x => x.Moein.MoeinName),
                    Description = n.Max(x => x.Moein.MoeinKol.Description),
                    Nature = n.Max(x => x.Moein.Nature),
                    SellerId = n.Max(s => s.Doc.SellerId),
                    PeriodId = n.Max(s => s.Doc.PeriodId),
                    Id = n.Max(x => x.Moein.KolId),

                    Bed = n.Sum(s => s.Bed),
                    Bes = n.Sum(s => s.Bes),
                    Mandeh = (n.Sum(s => s.Bed) > n.Sum(s => s.Bes)) ? n.Sum(s => s.Bed) - n.Sum(s => s.Bes) : n.Sum(s => s.Bes) - n.Sum(s => s.Bed),
                    MandehNature = Convert.ToInt16((n.Sum(s => s.Bed) > n.Sum(s => s.Bes)) ? 1 : ((n.Sum(s => s.Bed) < n.Sum(s => s.Bes)) ? 2 : 3))

                }).ToListAsync();

            return accounts;
        }

        //----------------------------------------
        public async Task<List<DocArticleDto>> CloseTemporaryPreviewAsync(EndOfPeriodSettings dto)
        {
            List<DocArticleDto> articles = new List<DocArticleDto>();
            articles.Clear();
            Guid docId = Guid.NewGuid();
            string username = dto.CurrentUser;

            int rownumber = 1;
            //حساب خلاصه سود و زیان
            var SummaryAccount = await _db.Acc_Coding_Moeins.FindAsync(dto.SummaryAccountId.Value);
            //
            var temroratyGroups = await _db.Acc_Coding_Groups.Where(n => n.SellerId == dto.SellerId && n.TypeId == 2).ToListAsync();

            foreach (var g in temroratyGroups)
            {
                var GroupArticles = await _db.Acc_Articles
                .Include(n => n.Moein).ThenInclude(n => n.MoeinKol).Where(n =>
                    n.Doc.SellerId == dto.SellerId
                    && n.Doc.PeriodId == dto.PeriodId
                    && (n.IsDeleted == false && n.Doc.IsDeleted == false)
                    && n.Moein.MoeinKol.GroupId == g.Id
                    ).ToListAsync();

                var BalancedAccounts = GroupArticles.GroupBy(n => n.MoeinId)
                       .Select(n => new CloseAccountArticleDto
                       {
                           MoeinId = n.Key,
                           SellerId = dto.SellerId,
                           PeriodId = dto.PeriodId,
                           GroupAccountName = $"بابت بستن حساب های   {g.GroupName}",
                           KolId = n.Max(x => x.Moein.KolId),
                           MoeinCode = n.Max(x => x.Moein.MoeinCode),
                           Nature = n.Max(x => x.Moein.Nature),
                           MoeinName = n.Max(x => x.Moein.MoeinName),
                           Bed = n.Sum(x => x.Bed),
                           Bes = n.Sum(x => x.Bes),
                           Mandeh = (n.Sum(x => x.Bed) > n.Sum(x => x.Bes)) ? n.Sum(x => x.Bed) - n.Sum(x => x.Bes) : n.Sum(x => x.Bes) - n.Sum(x => x.Bed),
                           MandehNature = (n.Sum(x => x.Bed) > n.Sum(x => x.Bes)) ? (Int16)1 : ((n.Sum(x => x.Bed) < n.Sum(x => x.Bes)) ? (Int16)2 : (Int16)3)

                       }).ToList();


                foreach (var x in BalancedAccounts)
                {
                    DocArticleDto a = new DocArticleDto();
                    a.Id = Guid.NewGuid();
                    a.DocId = docId;
                    a.CreatorUserName = username;
                    a.RowNumber = rownumber;
                    a.SellerId = x.SellerId;
                    a.PeriodId = x.PeriodId.Value;
                    if (x.Mandeh > 0)
                    {
                        if (x.MandehNature == 1)
                        {
                            a.Bes = x.Mandeh;
                            a.strBes = x.Mandeh.ToPrice();
                            a.Bed = 0;
                        }
                        else
                        {
                            a.Bed = x.Mandeh;
                            a.strBed = x.Mandeh.ToPrice();
                            a.Bes = 0;
                        }
                    }
                    a.Amount = x.Mandeh;
                    a.Comment = $"بابت بستن حساب  {x.MoeinName} با " + SummaryAccount.MoeinName;
                    a.KolId = x.KolId;
                    a.MoeinCode = x.MoeinCode;
                    a.MoeinId = x.MoeinId;
                    a.MoeinName = x.MoeinName;
                    if (x.Nature != x.MandehNature && x.MandehNature != 3)
                        a.Comment += "( مانده خلاف ماهیت است )";
                    a.IsDeleted = false;

                    articles.Add(a);
                    rownumber++;

                    // طرف سود و زیان
                    DocArticleDto summaryRow = new DocArticleDto();
                    summaryRow.Id = Guid.NewGuid();
                    summaryRow.DocId = docId;
                    summaryRow.RowNumber = rownumber;
                    summaryRow.SellerId = dto.SellerId;
                    summaryRow.PeriodId = dto.PeriodId.Value;
                    summaryRow.KolId = SummaryAccount.KolId;
                    summaryRow.MoeinId = SummaryAccount.Id;
                    summaryRow.MoeinCode = SummaryAccount.MoeinCode;
                    summaryRow.MoeinName = SummaryAccount.MoeinName;
                    summaryRow.Bed = a.Bes > 0 ? a.Bes : 0;
                    summaryRow.Bes = a.Bed > 0 ? a.Bed : 0;
                    summaryRow.Amount = a.Bed > 0 ? a.Bed : a.Bes;
                    summaryRow.IsDeleted = false;
                    summaryRow.Comment = $"بابت بستن حساب   {x.MoeinName} ";
                    summaryRow.CreatorUserName = username;
                    articles.Add(summaryRow);
                    rownumber++;

                }
            }

            // بستن اول دوره
            var avalDoreh = await _db.Acc_Articles.Include(n => n.Moein)
                     .Where(n =>
                     n.Doc.SellerId == dto.SellerId
                     && n.Doc.PeriodId == dto.PeriodId
                     && n.MoeinId == dto.MojoodiKalaAccount
                     && (n.Doc.DocNumber == 1 || n.Doc.TypeId == 2)
                      ).FirstOrDefaultAsync();

            if (avalDoreh != null)
            {
                DocArticleDto SummaryAvalDore = new DocArticleDto();
                SummaryAvalDore.Id = Guid.NewGuid();
                SummaryAvalDore.DocId = docId;
                SummaryAvalDore.RowNumber = rownumber;
                SummaryAvalDore.SellerId = dto.SellerId;
                SummaryAvalDore.PeriodId = dto.PeriodId.Value;
                SummaryAvalDore.KolId = SummaryAccount.KolId;
                SummaryAvalDore.MoeinId = SummaryAccount.Id;
                SummaryAvalDore.MoeinCode = SummaryAccount.MoeinCode;
                SummaryAvalDore.MoeinName = SummaryAccount.MoeinName;
                SummaryAvalDore.Bed = avalDoreh != null ? avalDoreh.Amount : 0;
                SummaryAvalDore.Bes = 0;
                SummaryAvalDore.Amount = avalDoreh != null ? avalDoreh.Amount : 0;
                SummaryAvalDore.IsDeleted = false;
                SummaryAvalDore.Comment = $"بابت بستن حساب موجودی اول دوره ";
                SummaryAvalDore.CreatorUserName = username;
                articles.Add(SummaryAvalDore);
                rownumber++;

                DocArticleDto avadoreArt = new DocArticleDto();
                avadoreArt.Id = Guid.NewGuid();
                avadoreArt.DocId = docId;
                avadoreArt.RowNumber = rownumber;
                avadoreArt.SellerId = dto.SellerId;
                avadoreArt.PeriodId = dto.PeriodId.Value;
                avadoreArt.KolId = avalDoreh.Moein.KolId;
                avadoreArt.MoeinId = avalDoreh.MoeinId;
                avadoreArt.MoeinCode = avalDoreh.Moein.MoeinCode;
                avadoreArt.MoeinName = avalDoreh.Moein.MoeinName;
                avadoreArt.Bed = 0;
                avadoreArt.Bes = avalDoreh != null ? avalDoreh.Amount : 0;
                avadoreArt.Amount = avalDoreh != null ? avalDoreh.Amount : 0;
                avadoreArt.IsDeleted = false;
                avadoreArt.Comment = $"بابت بستن حساب موجودی اول دوره با خلاصه سودوزیان ";
                avadoreArt.CreatorUserName = username;
                articles.Add(avadoreArt);
                rownumber++;
            }



            if (dto.payanDore > 0)
            {
                var payanDore = await _db.Acc_Coding_Moeins.FindAsync(dto.PayanDoreAccount.Value);
                long payanAmount = dto.payanDore;
                if (payanDore != null)
                {
                    var getPayanDore = await _db.Acc_Articles
                     .Where(n =>
                     n.Doc.SellerId == dto.SellerId
                     && n.Doc.PeriodId == dto.PeriodId
                     && n.MoeinId == payanDore.Id
                     && n.IsDeleted == false)
                     .FirstOrDefaultAsync();

                    if (getPayanDore != null)
                    {
                        getPayanDore.Bed = payanAmount;
                        getPayanDore.Bes = 0;
                        getPayanDore.Amount = payanAmount;
                        getPayanDore.EditorUserName = username;
                        getPayanDore.LastUpdateDate = DateTime.Now;
                        _db.Acc_Articles.Update(getPayanDore);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        DocArticleDto payanArt = new DocArticleDto();
                        payanArt.Id = Guid.NewGuid();
                        payanArt.DocId = docId;
                        payanArt.RowNumber = rownumber;
                        payanArt.SellerId = dto.SellerId;
                        payanArt.PeriodId = dto.PeriodId.Value;
                        payanArt.KolId = payanDore.KolId;
                        payanArt.MoeinId = payanDore.Id;
                        payanArt.MoeinCode = payanDore.MoeinCode;
                        payanArt.MoeinName = payanDore.MoeinName;
                        payanArt.Bed = payanAmount;
                        payanArt.Bes = 0;
                        payanArt.Amount = avalDoreh != null ? avalDoreh.Amount : 0;
                        payanArt.IsDeleted = false;
                        payanArt.Comment = $"بابت ایجاد حساب موجودی پایان دوره  ";
                        payanArt.CreatorUserName = username;
                        articles.Add(payanArt);
                        rownumber++;

                        DocArticleDto SummaryPayanDore = new DocArticleDto();
                        SummaryPayanDore.Id = Guid.NewGuid();
                        SummaryPayanDore.DocId = docId;
                        SummaryPayanDore.RowNumber = rownumber;
                        SummaryPayanDore.SellerId = dto.SellerId;
                        SummaryPayanDore.PeriodId = dto.PeriodId.Value;
                        SummaryPayanDore.KolId = SummaryAccount.KolId;
                        SummaryPayanDore.MoeinId = SummaryAccount.Id;
                        SummaryPayanDore.MoeinCode = SummaryAccount.MoeinCode;
                        SummaryPayanDore.MoeinName = SummaryAccount.MoeinName;
                        SummaryPayanDore.Bed = 0;
                        SummaryPayanDore.Bes = payanAmount;
                        SummaryPayanDore.Amount = payanAmount;
                        SummaryPayanDore.IsDeleted = false;
                        SummaryPayanDore.Comment = $"بابت تشکیل حساب موجودی پایان دوره ";
                        SummaryPayanDore.CreatorUserName = username;
                        articles.Add(SummaryPayanDore);
                        rownumber++;
                    }
                }

            }


            //---------------------------------------------------------------------
            // بستن حساب خلاصه سود و زیان با سود و زیان انباشته
            if (dto.RetainedEarningsAccountId != null)
            {
                var RetainedEarningsAccount = await _db.Acc_Coding_Moeins.FindAsync(dto.RetainedEarningsAccountId.Value);

                long SummaryBed = articles.Where(n => n.MoeinId == SummaryAccount.Id).Sum(x => x.Bed);
                long SummaryBes = articles.Where(n => n.MoeinId == SummaryAccount.Id).Sum(x => x.Bes);
                long netIncome = SummaryBed > SummaryBes ? SummaryBed - SummaryBes : SummaryBes - SummaryBed;
                int SummaryNature = SummaryBed > SummaryBes ? 1 : (SummaryBed < SummaryBes ? 2 : 3);
                if (netIncome == 0)
                    return articles;

                DocArticleDto summaryArt = new DocArticleDto();
                summaryArt.Id = Guid.NewGuid();
                summaryArt.DocId = docId;
                summaryArt.RowNumber = rownumber;
                summaryArt.SellerId = dto.SellerId;
                summaryArt.PeriodId = dto.PeriodId.Value;
                summaryArt.KolId = SummaryAccount.KolId;
                summaryArt.MoeinId = SummaryAccount.Id;
                summaryArt.MoeinCode = SummaryAccount.MoeinCode;
                summaryArt.MoeinName = SummaryAccount.MoeinName;
                if (SummaryNature == 2)
                {
                    summaryArt.Bed = netIncome;
                    summaryArt.strBed = netIncome.ToPrice();
                    summaryArt.Bes = 0;
                }
                else
                {
                    summaryArt.Bes = netIncome;
                    summaryArt.strBes = netIncome.ToPrice();
                    summaryArt.Bed = 0;
                }
                summaryArt.IsDeleted = false;
                summaryArt.Comment = "بابت بستن حساب خلاصه سود و زیان با  " + RetainedEarningsAccount.MoeinName;
                summaryArt.CreatorUserName = username;
                //
                DocArticleDto RetainedArt = new DocArticleDto();
                RetainedArt.Id = Guid.NewGuid();
                RetainedArt.DocId = docId;
                RetainedArt.RowNumber = rownumber;
                RetainedArt.SellerId = dto.SellerId;
                RetainedArt.PeriodId = dto.PeriodId.Value;
                RetainedArt.KolId = RetainedEarningsAccount.KolId;
                RetainedArt.MoeinId = RetainedEarningsAccount.Id;
                RetainedArt.MoeinCode = RetainedEarningsAccount.MoeinCode;
                RetainedArt.MoeinName = RetainedEarningsAccount.MoeinName;
                RetainedArt.CreatorUserName = username;
                if (SummaryNature == 2)
                {
                    RetainedArt.Bes = netIncome;
                    RetainedArt.strBes = netIncome.ToPrice();
                    RetainedArt.Bed = 0;
                }
                else
                {
                    RetainedArt.Bed = netIncome;
                    RetainedArt.strBed = netIncome.ToPrice();
                    RetainedArt.Bes = 0;
                }
                RetainedArt.Amount = netIncome;
                RetainedArt.IsDeleted = false;
                RetainedArt.Comment = "بابت بستن حساب خلاصه سود و زیان   ";


                // Add to articles
                if (SummaryNature == 2)
                {
                    articles.Add(summaryArt);
                    articles.Add(RetainedArt);
                }
                else
                {
                    articles.Add(RetainedArt);
                    articles.Add(summaryArt);
                }

            }

            return articles;
        }

        public async Task<List<DocArticleDto>> CloseTemporaryPreviewOldAsync(EndOfPeriodSettings dto)
        {
            List<DocArticleDto> articles = new List<DocArticleDto>();
            int rownumber = 1;

            //حساب خلاصه سود و زیان
            // سود و زیاد انباشته
            var SummaryAccount = await _db.Acc_Coding_Moeins.FindAsync(dto.SummaryAccountId.Value);

            //حساب های درآمد
            var IncomeArts = await _db.Acc_Articles
                .Include(n => n.Moein).ThenInclude(n => n.MoeinKol).Where(n =>
                    n.Doc.SellerId == dto.SellerId
                    && n.Doc.PeriodId == dto.PeriodId
                    && n.IsDeleted == false
                    && dto.IncomeAccountIds.Contains(n.Moein.MoeinKol.GroupId)
                    ).ToListAsync();

            var incomes = IncomeArts.GroupBy(n => n.MoeinId)
           .Select(n => new CloseAccountArticleDto
           {
               MoeinId = n.Key,
               SellerId = dto.SellerId,
               PeriodId = dto.PeriodId,
               GroupAccountName = "بابت بستن حساب های درآمد",
               KolId = n.Max(x => x.Moein.KolId),
               MoeinCode = n.Max(x => x.Moein.MoeinCode),
               Nature = n.Max(x => x.Moein.Nature),
               MoeinName = n.Max(x => x.Moein.MoeinName),
               Bed = n.Sum(x => x.Bed),
               Bes = n.Sum(x => x.Bes),
               Mandeh = (n.Sum(x => x.Bed) > n.Sum(x => x.Bes)) ? n.Sum(x => x.Bed) - n.Sum(x => x.Bes) : n.Sum(x => x.Bes) - n.Sum(x => x.Bed),
               MandehNature = (n.Sum(x => x.Bed) > n.Sum(x => x.Bes)) ? (Int16)1 : ((n.Sum(x => x.Bed) < n.Sum(x => x.Bes)) ? (Int16)2 : (Int16)3)

           }).ToList();
            //
            List<DocArticleDto> Incomearticles = new List<DocArticleDto>();

            foreach (var x in incomes)
            {
                DocArticleDto a = new DocArticleDto();
                a.Id = Guid.NewGuid();
                a.RowNumber = rownumber;
                a.SellerId = x.SellerId;
                a.PeriodId = x.PeriodId.Value;
                if (x.Nature == 1)
                {
                    a.Bes = x.Mandeh;
                    a.strBes = x.Mandeh.ToPrice();
                    a.Bed = 0;
                }
                else
                {
                    a.Bed = x.Mandeh;
                    a.strBed = x.Mandeh.ToPrice();
                    a.Bes = 0;
                }
                a.Comment = "بابت بستن حساب های درآمد با " + SummaryAccount.MoeinName;
                a.KolId = x.KolId;
                a.MoeinCode = x.MoeinCode;
                a.MoeinId = x.MoeinId;
                a.MoeinName = x.MoeinName;
                if (x.Nature != x.MandehNature)
                    a.Comment = "( مانده خلاف ماهیت است )";
                a.IsDeleted = false;

                rownumber++;
                Incomearticles.Add(a);
            }
            //بستن حساب درآمد با خلاصه سود و زیان

            long incomBed = Incomearticles.Sum(x => x.Bed);
            long incomBes = Incomearticles.Sum(x => x.Bes);
            int incomeNature = incomBed > incomBes ? 1 : (incomBed < incomBes ? 2 : 3);
            long incomeMande = incomeNature == 1 ? incomBed - incomBes : incomBes - incomBed;


            DocArticleDto closeIncomeArt = new DocArticleDto();
            closeIncomeArt.Id = Guid.NewGuid();
            closeIncomeArt.RowNumber = rownumber;
            closeIncomeArt.SellerId = dto.SellerId;
            closeIncomeArt.PeriodId = dto.PeriodId.Value;
            closeIncomeArt.KolId = SummaryAccount.KolId;
            closeIncomeArt.MoeinId = SummaryAccount.Id;
            closeIncomeArt.MoeinCode = SummaryAccount.MoeinCode;
            closeIncomeArt.MoeinName = SummaryAccount.MoeinName;
            closeIncomeArt.Bed = incomeNature == 1 ? 0 : incomeMande;
            closeIncomeArt.Bes = incomeNature == 1 ? incomeMande : 0;
            closeIncomeArt.IsDeleted = false;
            closeIncomeArt.Comment = "بابت بستن حساب های درآمد ";

            rownumber++;
            articles.AddRange(Incomearticles);
            articles.Add(closeIncomeArt);
            //---------------------------------------------------------------------

            //حساب های هزینه
            var Expenses = await _db.Acc_Articles
                .Include(n => n.Moein).ThenInclude(n => n.MoeinKol).Where(n =>
                    n.Doc.SellerId == dto.SellerId
                    && n.Doc.PeriodId == dto.PeriodId
                    && n.IsDeleted == false
                    && dto.ExpenseAccountIds.Contains(n.Moein.MoeinKol.GroupId)
                    ).ToListAsync();
            var costs = Expenses.GroupBy(n => n.MoeinId)
          .Select(n => new CloseAccountArticleDto
          {
              MoeinId = n.Key,
              SellerId = dto.SellerId,
              PeriodId = dto.PeriodId,
              GroupAccountName = "بابت بستن حساب های هزینه",
              KolId = n.Max(x => x.Moein.KolId),
              MoeinCode = n.Max(x => x.Moein.MoeinCode),
              Nature = n.Max(x => x.Moein.Nature),
              MoeinName = n.Max(x => x.Moein.MoeinName),
              Bed = n.Sum(x => x.Bed),
              Bes = n.Sum(x => x.Bes),
              Mandeh = (n.Sum(x => x.Bed) > n.Sum(x => x.Bes)) ? n.Sum(x => x.Bed) - n.Sum(x => x.Bes) : n.Sum(x => x.Bes) - n.Sum(x => x.Bed),
              MandehNature = (n.Sum(x => x.Bed) > n.Sum(x => x.Bes)) ? (Int16)1 : ((n.Sum(x => x.Bed) < n.Sum(x => x.Bes)) ? (Int16)2 : (Int16)3)

          }).ToList();
            //
            List<DocArticleDto> costArticles = new List<DocArticleDto>();
            foreach (var x in costs)
            {
                DocArticleDto a = new DocArticleDto();
                a.Id = Guid.NewGuid();
                a.RowNumber = rownumber;
                a.SellerId = x.SellerId;
                a.PeriodId = x.PeriodId.Value;
                if (x.Nature == 1)
                {
                    a.Bes = x.Mandeh;
                    a.strBes = x.Mandeh.ToPrice();
                    a.Bed = 0;
                }
                else
                {
                    a.Bed = x.Mandeh;
                    a.strBed = x.Mandeh.ToPrice();
                    a.Bes = 0;
                }
                a.Comment = "بابت بستن حساب های هزینه با " + SummaryAccount.MoeinName;
                a.KolId = x.KolId;
                a.MoeinCode = x.MoeinCode;
                a.MoeinId = x.MoeinId;
                a.MoeinName = x.MoeinName;
                if (x.Nature != x.MandehNature)
                {
                    a.IsDeleted = true;
                    a.Comment = "مانده خلاف ماهیت است";
                }


                rownumber++;
                costArticles.Add(a);
            }
            //بستن حساب درآمد با خلاصه سود و زیان

            long costBed = costArticles.Sum(x => x.Bed);
            long costBes = costArticles.Sum(x => x.Bes);
            int costNature = costBed > costBes ? 1 : (costBed < costBes ? 2 : 3);
            long costMande = costNature == 1 ? costBed - costBes : costBes - costBed;
            DocArticleDto closeCostArt = new DocArticleDto();
            closeCostArt.Id = Guid.NewGuid();
            closeCostArt.RowNumber = rownumber;
            closeCostArt.SellerId = dto.SellerId;
            closeCostArt.PeriodId = dto.PeriodId.Value;
            closeCostArt.KolId = SummaryAccount.KolId;
            closeCostArt.MoeinId = SummaryAccount.Id;
            closeCostArt.MoeinCode = SummaryAccount.MoeinCode;
            closeCostArt.MoeinName = SummaryAccount.MoeinName;
            closeCostArt.Bed = costNature == 1 ? 0 : costMande;
            closeCostArt.Bes = costNature == 1 ? costMande : 0;
            closeCostArt.IsDeleted = false;
            closeCostArt.Comment = "بابت بستن حساب های هزینه ";

            rownumber++;
            articles.Add(closeCostArt);
            articles.AddRange(costArticles);

            //---------------------------------------------------------------------

            // بستن حساب خلاصه سود و زیان با سود و زیان انباشته
            if (dto.RetainedEarningsAccountId != null)
            {
                var RetainedEarningsAccount = await _db.Acc_Coding_Moeins.FindAsync(dto.RetainedEarningsAccountId.Value);

                long SummaryBed = articles.Where(n => n.MoeinId == SummaryAccount.Id).Sum(x => x.Bed);
                long SummaryBes = articles.Where(n => n.MoeinId == SummaryAccount.Id).Sum(x => x.Bes);
                long netIncome = SummaryBed > SummaryBes ? SummaryBed - SummaryBes : SummaryBes - SummaryBed;
                int SummaryNature = SummaryBed > SummaryBes ? 1 : (SummaryBed < SummaryBes ? 2 : 3);
                if (netIncome == 0)
                    return articles;

                DocArticleDto summaryArt = new DocArticleDto();
                summaryArt.Id = Guid.NewGuid();
                summaryArt.RowNumber = rownumber;
                summaryArt.SellerId = dto.SellerId;
                summaryArt.PeriodId = dto.PeriodId.Value;
                summaryArt.KolId = SummaryAccount.KolId;
                summaryArt.MoeinId = SummaryAccount.Id;
                summaryArt.MoeinCode = SummaryAccount.MoeinCode;
                summaryArt.MoeinName = SummaryAccount.MoeinName;
                if (SummaryNature == 2)
                {
                    summaryArt.Bed = netIncome;
                    summaryArt.strBed = netIncome.ToPrice();
                    summaryArt.Bes = 0;
                }
                else
                {
                    summaryArt.Bes = netIncome;
                    summaryArt.strBes = netIncome.ToPrice();
                    summaryArt.Bed = 0;
                }
                summaryArt.IsDeleted = false;
                summaryArt.Comment = "بابت بستن حساب خلاصه سود و زیان با  " + RetainedEarningsAccount.MoeinName;

                //
                DocArticleDto RetainedArt = new DocArticleDto();
                RetainedArt.Id = Guid.NewGuid();
                RetainedArt.RowNumber = rownumber;
                RetainedArt.SellerId = dto.SellerId;
                RetainedArt.PeriodId = dto.PeriodId.Value;
                RetainedArt.KolId = RetainedEarningsAccount.KolId;
                RetainedArt.MoeinId = RetainedEarningsAccount.Id;
                RetainedArt.MoeinCode = RetainedEarningsAccount.MoeinCode;
                RetainedArt.MoeinName = RetainedEarningsAccount.MoeinName;
                if (SummaryNature == 2)
                {
                    RetainedArt.Bes = netIncome;
                    RetainedArt.strBes = netIncome.ToPrice();
                    RetainedArt.Bed = 0;
                }
                else
                {
                    RetainedArt.Bed = netIncome;
                    RetainedArt.strBed = netIncome.ToPrice();
                    RetainedArt.Bes = 0;
                }
                RetainedArt.IsDeleted = false;
                RetainedArt.Comment = "بابت بستن حساب خلاصه سود و زیان   ";


                // Add to articles
                if (SummaryNature == 2)
                {
                    articles.Add(summaryArt);
                    articles.Add(RetainedArt);
                }
                else
                {
                    articles.Add(RetainedArt);
                    articles.Add(summaryArt);
                }

            }

            return articles;
        }
        //Closing Pemanent accounts
        public async Task<List<DocArticleDto>> ClosePermanentAccountsPreviewAsync(EndOfPeriodSettings dto)
        {
            List<DocArticleDto> articles = new List<DocArticleDto>();
            var ekhtetamiyeAccount = await _db.Acc_Coding_Moeins.FindAsync(dto.EkhtetamiyeAccountId.Value);
            List<int> permanentAccouts = await _db.Acc_Coding_Moeins.Where(n => n.SellerId == dto.SellerId && n.MoeinKol.KolGroup.TypeId == 1).Select(n => n.Id).ToListAsync();
            var balancedAccounts = await AccountsBanaceAsync(dto.SellerId, dto.PeriodId.Value, permanentAccouts);
            int rownumber = 1;
            Guid docId = Guid.NewGuid();
            foreach (var x in balancedAccounts)
            {
                if (x.Mandeh <= 0)
                    continue;
                DocArticleDto a = new DocArticleDto();
                a.Id = Guid.NewGuid();
                a.DocId = docId;
                a.RowNumber = rownumber;
                a.SellerId = x.SellerId;
                a.PeriodId = x.PeriodId.Value;
                if (x.MandehNature == 1)
                {
                    a.Bes = x.Mandeh;
                    a.strBes = x.Mandeh.ToPrice();
                    a.Bed = 0;
                }
                else
                {
                    a.Bed = x.Mandeh;
                    a.strBed = x.Mandeh.ToPrice();
                    a.Bes = 0;
                }
                a.Amount = x.Mandeh;
                a.Comment = "بابت بستن حساب با  " + ekhtetamiyeAccount.MoeinName;
                a.KolId = x.KolId;
                a.MoeinCode = x.MoeinCode;
                a.MoeinId = x.MoeinId;
                a.MoeinName = x.MoeinName;
                a.IsMatch = true;
                if (x.Nature != x.MandehNature && x.Nature != 3)
                {
                    a.Comment += "( مانده خلاف ماهیت است )";
                    a.IsMatch = false;
                }
                a.CreatorUserName = dto.CurrentUser;
                a.IsDeleted = false;
                articles.Add(a);
                rownumber++;

                // آرتیکل اختتامیه
                DocArticleDto art = new DocArticleDto();
                art.Id = Guid.NewGuid();
                art.DocId = docId;
                art.RowNumber = rownumber;
                art.SellerId = x.SellerId;
                art.PeriodId = x.PeriodId.Value;
                if (x.MandehNature == 2)
                {
                    art.Bes = x.Mandeh;
                    art.strBes = x.Mandeh.ToPrice();
                    art.Bed = 0;
                }
                else
                {
                    art.Bed = x.Mandeh;
                    art.strBed = x.Mandeh.ToPrice();
                    art.Bes = 0;
                }
                art.Amount = x.Mandeh;
                art.Comment = "  بابت بستن حساب " + x.MoeinName;
                art.KolId = ekhtetamiyeAccount.KolId;
                art.MoeinCode = ekhtetamiyeAccount.MoeinCode;
                art.MoeinId = ekhtetamiyeAccount.Id;
                art.MoeinName = ekhtetamiyeAccount.MoeinName;
                art.IsDeleted = false;
                art.CreatorUserName = dto.CurrentUser;
                art.IsMatch = true;
                articles.Add(art);
                rownumber++;
            }
            return articles;
        }
        public async Task<clsResult> ClosePermanentAccountsAsync(EndOfPeriodSettings dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            long sellerId = dto.SellerId;
            int periodId = dto.PeriodId.Value;

            // Check if all documents are balanced
            if (!await AreDocumentsBalancedAsync(sellerId, periodId))
            {
                result.Message = "همه اسناد تراز نیستند";
                return result;
            }

            // Check if document dates are in correct order
            if (!await AreDocumentDatesInOrderAsync(sellerId, periodId))
            {
                result.Message = "ترتیب تاریخ اسناد صحیح نیست";
                return result;
            }

            // Check if all documents are approved
            var approvalResult = await AreAllDocumentsApprovedAsync(sellerId, periodId);
            if (!approvalResult.Success)
            {
                result.Message = approvalResult.Message;
                return result;
            }

            var articles = await ClosePermanentAccountsPreviewAsync(dto);
            result = await _op.InsertSystemicDocAsync(articles, " بستن حساب های دائم (سند اختتامیه)", 3);

            return result;
        }

    }
}
