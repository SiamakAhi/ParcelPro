using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.Accounting.Models.Entities;
using ParcelPro.Models;

namespace ParcelPro.Areas.Accounting.AccountingServices
{
    public class AccAsistantsService : IAccAsistantsService
    {
        private readonly AppDbContext _db;
        public readonly IAccOperationService _accService;
        private readonly IAccCodingService _coding;

        public AccAsistantsService(AppDbContext appDbContext, IAccOperationService accService, IAccCodingService codingService)
        {
            _db = appDbContext;
            _accService = accService;
            _coding = codingService;
        }

        public async Task<clsResult> InsertMoadianReportAsync(long sellerId, List<Acc_MoadianReport> report)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.Message = "اطلاعاتی برای درج در دیتابیس یافت نشد";
            result.ShowMessage = true;

            if (report.Count == 0) return result;

            foreach (var x in report)
            {

                var Item = new Acc_MoadianReport();
                var existItem = await _db.Acc_ModianReports.Where(n => n.TaxNumber == x.TaxNumber).FirstOrDefaultAsync();
                if (existItem != null)
                    Item = existItem;

                Item.SellerId = sellerId;
                Item.InvoiceType = x.InvoiceType;
                Item.InvoicePattern = x.InvoicePattern;
                Item.InvoiceSubject = x.InvoiceSubject;
                Item.TaxNumber = x.TaxNumber;
                Item.TotalInvoiceAmount = x.TotalInvoiceAmount;
                Item.VAT = x.VAT;
                Item.InvoiceStatus = x.InvoiceStatus;
                Item.IssueDate = x.IssueDate;
                Item.FolderInsertDate = x.FolderInsertDate;
                Item.BuyerIdentity = x.BuyerIdentity;
                Item.BuyerEconomicNumber = x.BuyerEconomicNumber;
                Item.SellerBranch = x.SellerBranch;
                Item.BuyerName = x.BuyerName;
                Item.BuyerTradeName = x.BuyerTradeName;
                Item.BuyerPersonType = x.BuyerPersonType;
                Item.SellerContractNumber = x.SellerContractNumber;
                Item.SubscriptionNumber = x.SubscriptionNumber;
                Item.FlightType = x.FlightType;
                Item.ContractorContractNumber = x.ContractorContractNumber;
                Item.SettlementMethod = x.SettlementMethod;
                Item.YearAndPeriod = x.YearAndPeriod;
                Item.LimitStatus = x.LimitStatus;
                Item.AccountingStatus = x.AccountingStatus;
                Item.InvoiceAmountWithoutVAT = x.InvoiceAmountWithoutVAT;
                Item.ReferringInvoiceIssueDate = x.ReferringInvoiceIssueDate;
                Item.NonAccountingStatusDate = x.NonAccountingStatusDate;
                Item.ReferenceInvoiceTaxNumber = x.ReferenceInvoiceTaxNumber;
                Item.InvoiceSettlementBalance = x.InvoiceSettlementBalance;
                if (existItem != null)
                    _db.Acc_ModianReports.Update(Item);
                else
                    _db.Acc_ModianReports.Add(Item);
            }

            await _db.SaveChangesAsync();
            result.Success = true;
            result.Message = "اطلاعات با موفقیت ذخیره شد";
            return result;
        }

        public async Task<List<Acc_MoadianReport>> ReadMoadianReportFromExcelAsync(IFormFile file)
        {
            var reportList = new List<Acc_MoadianReport>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheets.First();
                    var rows = worksheet.RowsUsed().Skip(1); // Skip header row

                    foreach (var row in rows)
                    {
                        var report = new Acc_MoadianReport();

                        report.InvoiceType = row.Cell(1).GetString();
                        report.InvoicePattern = row.Cell(2).GetString();
                        report.InvoiceSubject = row.Cell(3).GetString();
                        report.TaxNumber = row.Cell(5).GetString();
                        report.TotalInvoiceAmount = row.Cell(6).GetValue<long?>() ?? 0;
                        report.VAT = row.Cell(7).GetValue<long?>() ?? 0;
                        report.InvoiceStatus = row.Cell(8).GetString();
                        string strDate = row.Cell(9).GetString().Substring(0, 10);
                        report.IssueDate = strDate.mdToMiladiDate();
                        string strFolderInsertDate = row.Cell(10).GetString().Substring(0, 10);
                        report.FolderInsertDate = strFolderInsertDate.mdToMiladiDate();
                        report.BuyerIdentity = row.Cell(11).GetString();
                        report.BuyerEconomicNumber = row.Cell(12).GetString();
                        report.SellerBranch = row.Cell(13).GetString();
                        report.BuyerName = row.Cell(14).GetString();
                        report.BuyerTradeName = row.Cell(15).GetString();
                        report.BuyerPersonType = row.Cell(16).GetString();
                        report.SellerContractNumber = row.Cell(17).GetString();
                        report.SubscriptionNumber = row.Cell(18).GetString();
                        report.FlightType = row.Cell(19).GetString();
                        report.ContractorContractNumber = row.Cell(20).GetString();
                        report.SettlementMethod = row.Cell(21).GetString();
                        report.YearAndPeriod = row.Cell(22).GetString();
                        report.LimitStatus = row.Cell(25).GetString();
                        report.AccountingStatus = row.Cell(26).GetString();
                        report.InvoiceAmountWithoutVAT = row.Cell(27).GetValue<long?>() ?? 0;

                        //string? strReferringInvoiceIssueDate = row.Cell(28).GetString()?.Substring(0, 10);
                        report.ReferringInvoiceIssueDate = null;// strReferringInvoiceIssueDate?.mdToMiladiDate();

                        //string? strNonAccountingStatusDate = row.Cell(29).GetString()?.Substring(0, 10);
                        report.NonAccountingStatusDate = null;// strNonAccountingStatusDate?.mdToMiladiDate();
                        report.ReferenceInvoiceTaxNumber = row.Cell(30).GetString();
                        report.InvoiceSettlementBalance = row.Cell(32).GetValue<long?>() ?? 0;

                        reportList.Add(report);
                    }
                }
            }

            return reportList;
        }



        public async Task<BulkDocDto> PreparingToCreateMoadianDocAsync(List<Acc_MoadianReport> report, bool isSale, long sellerId, int periodId, string currentUser)
        {
            BulkDocDto docs = new BulkDocDto();

            //بدهکاران تجاری  2060001
            //فروش 6010001
            // بدهی تحقق یافته ( ارزش افزوده فروش )5030006
            // بدهی تحقق نیافته ( ارزش افزوده فروش ) 5030007


            var saleMoein = await _db.Acc_Coding_Moeins.AsNoTracking().Where(n => n.SellerId == sellerId && n.MoeinCode == "6010001").FirstOrDefaultAsync();
            var DebtorMoein = await _db.Acc_Coding_Moeins.AsNoTracking().Where(n => n.SellerId == sellerId && n.MoeinCode == "2060001").FirstOrDefaultAsync();
            var VatNaghdiMoein = await _db.Acc_Coding_Moeins.AsNoTracking().Where(n => n.SellerId == sellerId && n.MoeinCode == "5030006").FirstOrDefaultAsync();
            var VatNesiyehiMoein = await _db.Acc_Coding_Moeins.AsNoTracking().Where(n => n.SellerId == sellerId && n.MoeinCode == "5030007").FirstOrDefaultAsync();

            int atf = await _accService.DocAutoNumberGeneratorAsync(sellerId, periodId);
            int docNo = await _accService.DocNumberGeneratorAsync(sellerId, periodId);
            foreach (var x in report.OrderBy(d => d.IssueDate).ToList())
            {

                long? tafsilId = null;
                string? tafsilName = null;
                if (!string.IsNullOrEmpty(x.BuyerName))
                {
                    tafsilId = await _coding.CheckAddTafsilAsync(x.BuyerName, sellerId);
                    tafsilName = x.BuyerName;
                }

                Acc_Document header = new Acc_Document();
                header.Id = Guid.NewGuid();
                header.SellerId = sellerId;
                header.PeriodId = periodId;
                header.AtfNumber = atf;
                header.AutoDocNumber = atf;
                header.DocNumber = docNo;
                header.DocDate = x.IssueDate;
                header.Description = "بابت منظور نمودن مدارک مثبته ضمیمه سند حسابداری";
                header.IsDeleted = false;
                header.CreatorUserName = currentUser;
                header.CreateDate = DateTime.Now;

                atf++;
                docNo++;
                docs.Headers.Add(header);

                //Articles
                int row = 1;
                //بدهکاران تجاری
                Acc_Article debtor = new Acc_Article();
                debtor.Id = Guid.NewGuid();
                debtor.DocId = header.Id;
                debtor.KolId = DebtorMoein.KolId;
                debtor.MoeinId = DebtorMoein.Id;
                debtor.Amount = x.TotalInvoiceAmount;
                debtor.Bed = x.TotalInvoiceAmount;
                debtor.Bes = 0;
                debtor.Tafsil4Id = tafsilId;
                debtor.Tafsil4Name = tafsilName;
                debtor.ArchiveCode = x.TaxNumber;
                debtor.RowNumber = row;
                debtor.IsDeleted = false;
                debtor.CreatorUserName = currentUser;
                debtor.CreateDate = DateTime.Now;
                row++;
                docs.Articles.Add(debtor);

                // فروش
                Acc_Article sale = new Acc_Article();
                sale.Id = Guid.NewGuid();
                sale.DocId = header.Id;
                sale.KolId = saleMoein.KolId;
                sale.MoeinId = saleMoein.Id;
                sale.Amount = x.InvoiceAmountWithoutVAT;
                sale.Bed = 0;
                sale.Bes = x.InvoiceAmountWithoutVAT;
                sale.Tafsil4Id = tafsilId;
                sale.Tafsil4Name = tafsilName;
                sale.ArchiveCode = x.TaxNumber;
                sale.RowNumber = row;
                sale.IsDeleted = false;
                sale.CreatorUserName = currentUser;
                sale.CreateDate = DateTime.Now;
                row++;
                docs.Articles.Add(sale);

                if (x.SettlementMethod == "نقدی")
                {
                    // اعتبار تحقق یافته 
                    Acc_Article Vat = new Acc_Article();
                    Vat.Id = Guid.NewGuid();
                    Vat.DocId = header.Id;
                    Vat.KolId = VatNaghdiMoein.KolId;
                    Vat.MoeinId = VatNaghdiMoein.Id;
                    Vat.Amount = x.VAT;
                    Vat.Bed = 0;
                    Vat.Bes = x.VAT;
                    Vat.ArchiveCode = x.TaxNumber;
                    Vat.RowNumber = row;
                    Vat.IsDeleted = false;
                    Vat.CreatorUserName = currentUser;
                    Vat.CreateDate = DateTime.Now;
                    row++;
                    docs.Articles.Add(Vat);
                }
                else
                {
                    // اعتبار تحقق نیافته 
                    Acc_Article Vat = new Acc_Article();
                    Vat.Id = Guid.NewGuid();
                    Vat.DocId = header.Id;
                    Vat.KolId = VatNesiyehiMoein.KolId;
                    Vat.MoeinId = VatNesiyehiMoein.Id;
                    Vat.Tafsil4Id = tafsilId;
                    Vat.Tafsil4Name = tafsilName;
                    Vat.Amount = x.VAT;
                    Vat.Bed = 0;
                    Vat.Bes = x.VAT;
                    Vat.ArchiveCode = x.TaxNumber;
                    Vat.RowNumber = row;
                    Vat.IsDeleted = false;
                    Vat.CreatorUserName = currentUser;
                    Vat.CreateDate = DateTime.Now;
                    row++;
                    docs.Articles.Add(Vat);
                }
            }

            return docs;

        }

        public async Task<clsResult> InsertBulkDocsAsync(BulkDocDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.Message = "اطلاعاتی برای درج در دیتابیس یافت نشد";
            result.ShowMessage = true;

            if (dto.Headers.Count == 0 || dto.Articles.Count == 0) return result;

            _db.Acc_Documents.AddRange(dto.Headers);
            _db.Acc_Articles.AddRange(dto.Articles);
            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "ثبت اسناد با موفقیت انجام شد";
            }
            catch (Exception x)
            {
                result.Success = false;
                result.Message = $"خطایی در عملیات ثبت اسناد بوجود آمده است . \n {x.Message}";
            }

            return result;
        }

        public async Task<clsResult> BankTransactionSaveAsCheckedAsync(List<long> items)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.Message = "اطلاعاتی یافت نشد";
            result.ShowMessage = true;

            if (items?.Count == 0)
                return result;

            var transaction = await _db.TreBankTransactions.Where(n => items.Contains(n.Id)).ToListAsync();
            if (transaction.Count == 0)
                return result;
            foreach (var x in transaction)
            {
                x.IsChecked = true;
                x.HasDoc = true;

            }
            _db.TreBankTransactions.UpdateRange(transaction);
            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "ثبت اطلاعات با موفقیت انجام شد";
            }
            catch (Exception x)
            {
                result.Success = false;
                result.Message = $"خطایی در عملیات ثبت اطلاعات بوجود آمده است . \n {x.Message}";
            }

            return result;
        }
    }
}
