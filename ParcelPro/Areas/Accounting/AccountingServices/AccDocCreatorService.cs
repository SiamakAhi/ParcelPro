using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.Accounting.Models.Entities;
using ParcelPro.Areas.Commercial.ComercialInterfaces;
using ParcelPro.Models;
using ParcelPro.Services;

namespace ParcelPro.Areas.Accounting.AccountingServices
{
    public class AccDocCreatorService : IAccDocCreatorService
    {
        private readonly AppDbContext _db;
        private readonly IAccOperationService _docServic;
        private readonly UserContextService _userContext;
        private readonly IAccCodingService _codingService;
        private readonly IInvoiceService _invoiceService;
        long? _sellerId = null;
        int? _periodId = null;

        public AccDocCreatorService(AppDbContext dbContext
            , IAccOperationService accOperationService
            , UserContextService userContextService
            , IAccCodingService codingService
            , IInvoiceService invoiceService)
        {
            _db = dbContext;
            _userContext = userContextService;
            _docServic = accOperationService;
            _codingService = codingService;
            _invoiceService = invoiceService;
            _sellerId = _userContext.SellerId;
            _periodId = _userContext.PeriodId;
        }
        private clsResult CheckAccountingSettings(Acc_Setting? settings, bool sale, bool buy)
        {
            clsResult result = new clsResult();
            result.Success = true;
            if (settings == null)
                result.Success = false;

            result.Message = "جهت ثبت اسناد خرید و فروش لازم است ابتدا از طریق منوی تنظیمات حسابداری، تنظیمات مربوط به حساب ها را اعمال نمائید.  \n \n \n";
            if (sale)
            {
                if (settings.saleDiscountMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب تحفیفات فروش مشخص نشده است.";
                    result.Success = false;
                }
                if (settings.saleMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب فروش مشخص نشده است.";
                    result.Success = !false;
                }
                if (settings.ReturnToSaleMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب برگشت از فروش مشخص نشده است.";
                    result.Success = false;
                }
                if (settings.SaleVatMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب ارزش افزوده فروش مشخص نشده است.";
                    result.Success = false;
                }
                if (settings.salePartyMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب دریافتنی تجاری (بدهکاران تجاری) مشخص نشده است .";
                    result.Success = false;
                }
            }
            if (buy)
            {
                if (settings.BuyDiscountMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب تحفیفات خرید مشخص نشده است.";
                    result.Success = false;
                }
                if (settings.BuyMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب خرید مشخص نشده است.";
                    result.Success = !false;
                }
                if (settings.ReturnToBuyMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب برگشت از خرید مشخص نشده است.";
                    result.Success = false;
                }
                if (settings.BuyVatMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب ارزش افزوده خرید مشخص نشده است.";
                    result.Success = false;
                }
                if (settings.BuyPartyMoeinId == null)
                {
                    result.Message += "\n در تنظیمات حسابداری، حساب پرداختنی تجاری (بستانکاران تجاری) مشخص نشده است .";
                    result.Success = false;
                }
            }

            return result;

        }
        public async Task<clsResult> CreateBankDocAsync(BankTransactionsCreateDocDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.Message = "اطاعاتی یافت نشد";

            if (!_sellerId.HasValue)
            {
                result.Message = "شرکت فعال یافت نشد";
                return result;
            }
            if (dto == null) return result;

            if (dto.PeriodId == 0)
            {
                result.Message = "سال مالی فعال یافت نشد";
                return result;
            }
            if (!dto.TransactionsId.Any())
            {
                result.Message = "اطلاعاتی برای ثبت وجود ندارد";
                return result;
            }

            // دریافت اطلاعات و اعتبارسنحی حساب بانکی
            var bankAccount = await _db.BankAccounts.Include(m => m.Moein).SingleOrDefaultAsync(n => n.Id == dto.BankAccountId);
            if (bankAccount == null)
            {
                result.Message = "اطلاعات حساب بانکی یافت نشد";
                return result;
            }
            if (!bankAccount.MoeinId.HasValue)
            {
                result.Message = "حساب معین بانک تعریف نشده است";
                return result;
            }
            if (!bankAccount.TafsilId.HasValue)
            {
                result.Message = "تفصیل حساب بانکی یافت نشد";
                return result;
            }
            if (dto.TransactionsType == 0)
            {
                result.Message = "نوع تراکنش (واریز-برداشت) مشخص نیست";
                return result;
            }
            //=============================================================

            var transactions = await _db.TreBankTransactions
                .Where(n => dto.TransactionsId.Contains(n.Id))
                .OrderBy(n => n.Row).ToListAsync();
            if (transactions == null) return result;

            //--
            List<Acc_Document> Docs = new List<Acc_Document>();
            List<Acc_Article> Articles = new List<Acc_Article>();

            //---- گروه بندی تراکنش ها بر اساس تاریخ تراکنش
            var dateGrouped = transactions.GroupBy(n => n.Date).Select(n => new
            {
                date = n.Key,
                transaction = n

            }).ToList();

            int docAutonumber = await _docServic.DocAutoNumberGeneratorAsync(dto.SellerId, dto.PeriodId);
            int docNumber = await _docServic.DocNumberGeneratorAsync(dto.SellerId, dto.PeriodId);
            int rownumber = 1;
            foreach (var x in dateGrouped)
            {
                //======= Doc Header
                Acc_Document doc = new Acc_Document();
                if (!dto.CreateNewDoc)
                {
                    var query = _db.Acc_Documents.Include(n => n.DocArticles)
                      .Where(n => !n.IsDeleted && n.SellerId == dto.SellerId && n.PeriodId == dto.PeriodId && n.DocDate.Date == x.date.Value.Date).
                    AsQueryable();

                    if (!string.IsNullOrEmpty(dto.DocSelector))
                        query = query.Where(n => n.Description == dto.DocSelector);

                    var docInDate = await query.FirstOrDefaultAsync();

                    if (docInDate != null)
                    {
                        doc = docInDate;
                        rownumber = docInDate.DocArticles.Select(m => m.RowNumber).Max() + 1;
                    }

                    else
                    {
                        doc.Id = Guid.NewGuid();
                        doc.SellerId = dto.SellerId;
                        doc.PeriodId = dto.PeriodId;
                        doc.DocDate = x.date.Value;
                        doc.AtfNumber = docAutonumber;
                        doc.AutoDocNumber = docAutonumber;
                        doc.DocNumber = docNumber;
                        doc.Description = $"بابت ثبت حسابداری اسناد ضمیمه";
                        if (!string.IsNullOrEmpty(dto.DocSelector))
                            doc.Description = dto.DocSelector;
                        doc.StatusId = 1;
                        doc.SubsystemId = 2;
                        doc.CreateDate = DateTime.Now;
                        doc.CreatorUserName = dto.UserName;
                        doc.IsDeleted = false;
                        doc.TypeId = 1;
                        Docs.Add(doc);

                        docAutonumber++;
                        docNumber++;
                    }
                }
                else
                {
                    doc.Id = Guid.NewGuid();
                    doc.SellerId = dto.SellerId;
                    doc.PeriodId = dto.PeriodId;
                    doc.DocDate = x.date.Value;
                    doc.AtfNumber = docAutonumber;
                    doc.AutoDocNumber = docAutonumber;
                    doc.DocNumber = docNumber;
                    doc.Description = $"بابت ثبت حسابداری اسناد ضمیمه";
                    if (!string.IsNullOrEmpty(dto.DocSelector))
                        doc.Description = dto.DocSelector;
                    doc.StatusId = 1;
                    doc.SubsystemId = 2;
                    doc.CreateDate = DateTime.Now;
                    doc.CreatorUserName = dto.UserName;
                    doc.IsDeleted = false;
                    doc.TypeId = 1;
                    Docs.Add(doc);

                    docAutonumber++;
                    docNumber++;
                }

                //== End Create Doc Heade =========================================
                var moein = await _db.Acc_Coding_Moeins.FindAsync(dto.MoeinId);

                Acc_Coding_Tafsil tafsil4 = new Acc_Coding_Tafsil();
                if (dto.Tafsil4Id.HasValue)
                    tafsil4 = await _db.Acc_Coding_Tafsils.FindAsync(dto.Tafsil4Id.Value);

                Acc_Coding_Tafsil tafsil5 = new Acc_Coding_Tafsil();
                if (dto.Tafsil5Id.HasValue)
                    tafsil5 = await _db.Acc_Coding_Tafsils.FindAsync(dto.Tafsil5Id.Value);

                Acc_Coding_Tafsil bankTafsil = new Acc_Coding_Tafsil();
                if (bankAccount.TafsilId.HasValue)
                    bankTafsil = await _db.Acc_Coding_Tafsils.FindAsync(bankAccount.TafsilId.Value);

                // ردیف بانک
                // واریزی
                long totalBed = x.transaction.Sum(n => n.Debtor) ?? 0;
                long totalBes = x.transaction.Sum(n => n.Creditor) ?? 0;

                Acc_Article BankArt = new Acc_Article();
                BankArt.Id = Guid.NewGuid();
                BankArt.SellerId = dto.SellerId;
                BankArt.DocId = doc.Id;
                BankArt.PeriodId = doc.PeriodId;
                BankArt.KolId = bankAccount?.Moein?.KolId;
                BankArt.MoeinId = bankAccount.MoeinId.Value;
                BankArt.Tafsil4Id = bankTafsil?.Id;
                BankArt.Tafsil4Name = bankTafsil?.Name;
                BankArt.Amount = totalBed > 0 ? totalBed : totalBes;
                BankArt.Comment = dto.Descriptions;
                BankArt.CreateDate = DateTime.Now;
                BankArt.CreatorUserName = dto.UserName;
                BankArt.IsDeleted = false;


                if (dto.Grouped)
                {
                    // طرف دم سند
                    Acc_Article Side2Art = new Acc_Article();
                    Side2Art.Id = Guid.NewGuid();
                    Side2Art.SellerId = dto.SellerId;
                    Side2Art.DocId = doc.Id;
                    Side2Art.PeriodId = doc.PeriodId;
                    Side2Art.KolId = moein?.KolId;
                    Side2Art.MoeinId = moein.Id;
                    Side2Art.Tafsil4Id = tafsil4?.Id;
                    Side2Art.Tafsil4Name = tafsil4?.Name;
                    Side2Art.Tafsil5Id = tafsil5?.Id;
                    Side2Art.Tafsil5Name = tafsil5?.Name;
                    Side2Art.Amount = totalBed > 0 ? totalBed : totalBes;
                    Side2Art.Comment = dto.Descriptions;
                    Side2Art.CreateDate = DateTime.Now;
                    Side2Art.CreatorUserName = dto.UserName;
                    Side2Art.IsDeleted = false;


                    if (dto.TransactionsType == 1)
                    {
                        BankArt.Bed = BankArt.Amount;
                        BankArt.Bes = 0;
                        BankArt.RowNumber = rownumber;
                        rownumber++;
                        Articles.Add(BankArt);

                        // -------
                        Side2Art.Bed = 0;
                        Side2Art.Bes = Side2Art.Amount;
                        Side2Art.RowNumber = rownumber;
                        rownumber++;
                        Articles.Add(Side2Art);

                    }
                    else if (dto.TransactionsType == 2)
                    {
                        // -------
                        Side2Art.Bed = Side2Art.Amount;
                        Side2Art.Bes = 0;
                        Side2Art.RowNumber = rownumber;
                        rownumber++;
                        Articles.Add(Side2Art);

                        //-------
                        BankArt.Bed = 0;
                        BankArt.Bes = BankArt.Amount;
                        BankArt.RowNumber = rownumber;
                        rownumber++;
                        Articles.Add(BankArt);

                    }
                }
                else
                {
                    if (dto.TransactionsType == 1)
                    {
                        BankArt.Bed = BankArt.Amount;
                        BankArt.Bes = 0;
                        BankArt.RowNumber = rownumber;
                        rownumber++;
                        Articles.Add(BankArt);
                    }

                    foreach (var a in x.transaction)
                    {
                        long bed = a.Debtor ?? 0;
                        long bes = a.Creditor ?? 0;

                        Acc_Article Side2Art = new Acc_Article();
                        Side2Art.Id = Guid.NewGuid();
                        Side2Art.SellerId = dto.SellerId;
                        Side2Art.DocId = doc.Id;
                        Side2Art.PeriodId = doc.PeriodId;
                        Side2Art.KolId = moein.KolId;
                        Side2Art.MoeinId = moein.Id;
                        Side2Art.Tafsil4Id = tafsil4?.Id;
                        Side2Art.Tafsil4Name = tafsil4?.Name;
                        Side2Art.Tafsil5Id = tafsil5?.Id;
                        Side2Art.Tafsil5Name = tafsil5?.Name;
                        Side2Art.Comment = dto.Descriptions;
                        Side2Art.CreateDate = DateTime.Now;
                        Side2Art.CreatorUserName = dto.UserName;
                        Side2Art.IsDeleted = false;

                        if (dto.AppendBankDescription)
                            Side2Art.Comment += " " + a.Description;
                        if (dto.InsertTrackingNumber)
                            Side2Art.ArchiveCode = a.DocumentNumber;

                        if (dto.TransactionsType == 1)
                        {
                            // -------
                            Side2Art.Amount = a.Debtor ?? 0;
                            Side2Art.Bed = 0;
                            Side2Art.Bes = a.Debtor ?? 0;
                            Side2Art.RowNumber = rownumber;
                            rownumber++;
                            Articles.Add(Side2Art);
                        }
                        else if (dto.TransactionsType == 2)
                        {
                            // -------
                            Side2Art.Amount = a.Creditor ?? 0;
                            Side2Art.Bed = a.Creditor ?? 0;
                            Side2Art.Bes = 0;
                            Side2Art.RowNumber = rownumber;
                            rownumber++;
                            Articles.Add(Side2Art);
                        }
                    }
                    if (dto.TransactionsType == 2)
                    {
                        BankArt.Bed = 0;
                        BankArt.Bes = BankArt.Amount;
                        BankArt.RowNumber = rownumber;
                        rownumber++;
                        Articles.Add(BankArt);
                    }
                }
            }

            try
            {
                _db.Acc_Documents.AddRange(Docs);
                _db.Acc_Articles.AddRange(Articles);
                foreach (var t in transactions)
                {
                    t.IsChecked = true;
                    t.HasDoc = true;
                }
                _db.TreBankTransactions.UpdateRange(transactions);

                await _db.SaveChangesAsync();

                result.Message = "ثبت اسناد حسابداری با موفقیت انجام شد";
                result.Success = true;
            }
            catch (Exception x)
            {
                result.Message = "خطایی در ثبت اطلاعات رخ داده است";
                result.Message += "\n\n" + x.Message;
            }

            return result;
        }



        private class InvoiceDocInfo
        {
            public Guid InvoiceId { get; set; }
            public Guid DocId { get; set; }
            public int DocAutoNumber { get; set; }
        }
    }
}
