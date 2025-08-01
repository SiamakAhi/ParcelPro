using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Treasury.Dto;
using ParcelPro.Areas.Treasury.Models.Entities;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Models;

namespace ParcelPro.Areas.Treasury.TreasuryServices
{
    public class TreCashBoxService : ITreCashBoxService
    {
        private readonly AppDbContext _db;

        public TreCashBoxService(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<clsResult> AddCashBoxAsync(TreCashBoxDto dto)
        {
            clsResult result = new clsResult();
            try
            {
                TreCashBox cashBox = new TreCashBox
                {
                    Id = Guid.NewGuid(),
                    SellerId = dto.SellerId,
                    BranchId = dto.BranchId,
                    RegisterName = dto.RegisterName,
                    PhysicalLocation = dto.PhysicalLocation,
                    AccountId = dto.AccountId,
                    DetailedAccountId = dto.DetailedAccountId,
                    OpeningDate = dto.OpeningDate,
                    CurrencyId = dto.CurrencyId,
                    CurrencyRate = dto.CurrencyRate,
                    IsActive = dto.IsActive,
                    CreateAt = DateTime.Now,
                    CreatorUserName = dto.CreatorUserName
                };

                await _db.TreCashBoxes.AddAsync(cashBox);
                await _db.SaveChangesAsync();

                result.Success = true;
                result.Message = "صندوق نقدی با موفقیت اضافه شد";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطا در افزودن صندوق نقدی: " + ex.Message;
            }
            return result;
        }
        public async Task<clsResult> UpdateCashBoxAsync(TreCashBoxDto dto)
        {
            clsResult result = new clsResult();
            try
            {
                var cashBox = await _db.TreCashBoxes.FindAsync(dto.Id);
                if (cashBox == null)
                {
                    result.Success = false;
                    result.Message = "صندوق نقدی یافت نشد";
                    return result;
                }

                // به‌روزرسانی خصوصیات
                cashBox.RegisterName = dto.RegisterName;
                cashBox.PhysicalLocation = dto.PhysicalLocation;
                cashBox.BranchId = dto.BranchId;
                cashBox.AccountId = dto.AccountId;
                cashBox.DetailedAccountId = dto.DetailedAccountId;
                cashBox.OpeningDate = dto.OpeningDate;
                cashBox.CurrencyId = dto.CurrencyId;
                cashBox.CurrencyRate = dto.CurrencyRate;
                cashBox.IsActive = dto.IsActive;
                cashBox.CreatorUserName = dto.CreatorUserName;

                _db.TreCashBoxes.Update(cashBox);
                await _db.SaveChangesAsync();

                result.Success = true;
                result.Message = "صندوق نقدی با موفقیت بروزرسانی شد";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطا در بروزرسانی صندوق نقدی: " + ex.Message;
            }
            return result;
        }
        public async Task<List<TreCashBoxDto>> GetCashBoxesAsync(long sellerId)
        {
            return await _db.TreCashBoxes
                .Include(n => n.Currency)
                .Where(x => x.SellerId == sellerId)
                .Select(x => new TreCashBoxDto
                {
                    Id = x.Id,
                    SellerId = x.SellerId,
                    BranchId = x.BranchId,
                    BranchName = x.Branch != null ? x.Branch.BranchName : " نامشخص",
                    RegisterName = x.RegisterName,
                    PhysicalLocation = x.PhysicalLocation,
                    AccountId = x.AccountId,
                    DetailedAccountId = x.DetailedAccountId,
                    TafsilName = x.Tafsil != null ? x.Tafsil.Name : " نامشخص",
                    TafsilCode = x.Tafsil != null ? x.Tafsil.Code : "-",
                    OpeningDate = x.OpeningDate,
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.Currency != null ? x.Currency.FullName : " نامشخص",
                    CurrencyRate = x.CurrencyRate,
                    IsActive = x.IsActive,
                    CreatorUserName = x.CreatorUserName
                }).ToListAsync();
        }
        public async Task<TreCashBoxDto> GetCashBoxByIdAsync(Guid id)
        {
            var cashBox = await _db.TreCashBoxes.FindAsync(id);
            if (cashBox == null)
                return null;
            return new TreCashBoxDto
            {
                Id = cashBox.Id,
                SellerId = cashBox.SellerId,
                BranchId = cashBox.BranchId,
                RegisterName = cashBox.RegisterName,
                PhysicalLocation = cashBox.PhysicalLocation,
                AccountId = cashBox.AccountId,
                DetailedAccountId = cashBox.DetailedAccountId,
                OpeningDate = cashBox.OpeningDate,
                CurrencyId = cashBox.CurrencyId,
                CurrencyRate = cashBox.CurrencyRate,
                IsActive = cashBox.IsActive,
                CreatorUserName = cashBox.CreatorUserName
            };
        }

        //==== POS ========================================================
        // لیست دستگاه‌های پوز
        public async Task<List<TreBankPosUcDto>> GetPosDevicesAsync(long sellerId)
        {
            return await _db.BankPosUcs
                 .Include(n => n.BankAccount)
                 .ThenInclude(n => n.Bank)
                 .Include(n => n.Currency)
                 .Where(x => x.SellerId == sellerId)
                 .Select(x => new TreBankPosUcDto
                 {
                     Id = x.Id,
                     SellerId = x.SellerId,
                     BranchId = x.BranchId,
                     Name = x.Name,
                     BankAccountId = x.BankAccountId,
                     TerminalNumber = x.TerminalNumber,
                     TafsilId = x.TafsilId,
                     CurrencyId = x.CurrencyId,
                     IsActive = x.IsActive,
                     CreateAt = x.CreateAt,
                     BankAccountName = x.BankAccount != null ? x.BankAccount.Bank.Name + " - " + x.BankAccount.AccountName + "-" + x.BankAccount.AccountNumber : "",
                     CurrencyName = x.Currency != null ? x.Currency.FullName : ""
                 }).ToListAsync();
        }

        // افزودن دستگاه پوز
        public async Task<clsResult> AddPosDeviceAsync(TreBankPosUcDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;

            var pos = new TreBankPosUc();
            pos.SellerId = dto.SellerId;
            pos.BranchId = dto.BranchId;
            pos.Name = dto.Name;
            pos.BankAccountId = dto.BankAccountId;
            pos.TerminalNumber = dto.TerminalNumber;
            pos.TafsilId = dto.TafsilId;
            pos.CurrencyId = dto.CurrencyId;
            pos.IsActive = dto.IsActive;
            pos.CreateAt = DateTime.Now;
            try
            {
                _db.BankPosUcs.Add(pos);
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "دستگاه پوز با موفقیت اضافه شد";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطا در افزودن دستگاه پوز: " + ex.Message;
            }
            return result;
        }

        // دریافت یک دستگاه پوز بر اساس شناسه
        public async Task<TreBankPosUcDto> GetPosDeviceByIdAsync(int id)
        {
            var pos = await _db.BankPosUcs
                .Include(x => x.BankAccount)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (pos == null) return null;
            return new TreBankPosUcDto
            {
                Id = pos.Id,
                SellerId = pos.SellerId,
                BranchId = pos.BranchId,
                Name = pos.Name,
                BankAccountId = pos.BankAccountId,
                TerminalNumber = pos.TerminalNumber,
                TafsilId = pos.TafsilId,
                CurrencyId = pos.CurrencyId,
                IsActive = pos.IsActive,
                CreateAt = pos.CreateAt,
                BankAccountName = pos.BankAccount != null ? pos.BankAccount.AccountName : "",
                CurrencyName = pos.Currency != null ? pos.Currency.FullName : ""
            };
        }

        // ویرایش دستگاه پوز
        public async Task<clsResult> UpdatePosDeviceAsync(TreBankPosUcDto dto)
        {
            clsResult result = new clsResult();
            try
            {
                var pos = await _db.BankPosUcs.FindAsync(dto.Id);
                if (pos == null)
                {
                    result.Success = false;
                    result.Message = "دستگاه پوز یافت نشد";
                    return result;
                }
                pos.Name = dto.Name;
                pos.BranchId = dto.BranchId;
                pos.BankAccountId = dto.BankAccountId;
                pos.TerminalNumber = dto.TerminalNumber;
                pos.TafsilId = dto.TafsilId;
                pos.CurrencyId = dto.CurrencyId;
                pos.IsActive = dto.IsActive;
                _db.BankPosUcs.Update(pos);
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "دستگاه پوز با موفقیت به‌روزرسانی شد";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطا در به‌روزرسانی دستگاه پوز: " + ex.Message;
            }
            return result;
        }

        // حذف دستگاه پوز
        public async Task<clsResult> DeletePosDeviceAsync(int id)
        {
            clsResult result = new clsResult();
            try
            {
                var pos = await _db.BankPosUcs.FindAsync(id);
                if (pos == null)
                {
                    result.Success = false;
                    result.Message = "دستگاه پوز یافت نشد";
                    return result;
                }
                _db.BankPosUcs.Remove(pos);
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "دستگاه پوز با موفقیت حذف شد";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطا در حذف دستگاه پوز: " + ex.Message;
            }
            return result;
        }

        //== End POS ===============================================================


    }
}
