using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.Accounting.Models.Entities;
using ParcelPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ParcelPro.Areas.Accounting.AccountingServices
{
    public class AccSettingService : IAccSettingService
    {
        private readonly AppDbContext _db;
        private readonly IMemoryCache _cache;

        public AccSettingService(AppDbContext context, IMemoryCache cache)
        {
            _db = context;
            _cache = cache;
        }

        public async Task<AccSettingDto> GetSettingAsync(long sellerId)
        {
            if (!_cache.TryGetValue($"SellerSettings_{sellerId}", out AccSettingDto settings))
            {
                var setting = await _db.Acc_Settings.FirstOrDefaultAsync(s => s.SellerId == sellerId);
                if (setting == null)
                {
                    setting = await CreateDefaultSettingsAsync(sellerId);
                }

                settings = new AccSettingDto
                {
                    Id = setting.Id,
                    SellerId = setting.SellerId,
                    AccLevel = setting.AccLevel,
                    DocPrintDefault = setting.DocPrintDefault,
                    ShowAllTafsil = setting.ShowAllTafsil,
                    MandatoryTafsil = setting.MandatoryTafsil,
                    PrintCreator = setting.PrintCreator,
                    Approver1Title = setting.Approver1Title,
                    Approver1Name = setting.Approver1Name,
                    Approver2Title = setting.Approver2Title,
                    Approver2Name = setting.Approver2Name,
                    saleMoeinId = setting.saleMoeinId,
                    salePartyMoeinId = setting.salePartyMoeinId,
                    SaleIsAutoGenerate = setting.SaleIsAutoGenerate,
                    saleDiscountMoeinId = setting.saleDiscountMoeinId,
                    ReturnToSaleMoeinId = setting.ReturnToSaleMoeinId,
                    SaleVatMoeinId = setting.SaleVatMoeinId,
                    BuyIsAutoGenerate = setting.BuyIsAutoGenerate,
                    BuyMoeinId = setting.BuyMoeinId,
                    BuyPartyMoeinId = setting.BuyPartyMoeinId,
                    ReturnToBuyMoeinId = setting.ReturnToBuyMoeinId,
                    BuyDiscountMoeinId = setting.BuyDiscountMoeinId,
                    BuyVatMoeinId = setting.BuyVatMoeinId,
                    WarehouseIsAutoGenerate = setting.WarehouseIsAutoGenerate,
                    WarehouseMoeinId = setting.WarehouseMoeinId,
                };

                // ذخیره در حافظه برای درخواست‌های بعدی
                _cache.Set($"SellerSettings_{sellerId}", settings);
            }

            return settings;
        }

        public async Task<clsResult> UpdateSettingAsync(AccSettingDto settingDto)
        {
            var existingSetting = await _db.Acc_Settings.FirstOrDefaultAsync(s => s.SellerId == settingDto.SellerId);
            if (existingSetting == null)
                return new clsResult { Success = false, Message = "اطلاعات یافت نشد" };

            existingSetting.AccLevel = settingDto.AccLevel;
            existingSetting.DocPrintDefault = settingDto.DocPrintDefault;
            existingSetting.ShowAllTafsil = settingDto.ShowAllTafsil;
            existingSetting.MandatoryTafsil = settingDto.MandatoryTafsil;
            existingSetting.PrintCreator = settingDto.PrintCreator;
            existingSetting.Approver1Title = settingDto.Approver1Title;
            existingSetting.Approver1Name = settingDto.Approver1Name;
            existingSetting.Approver2Title = settingDto.Approver2Title;
            existingSetting.Approver2Name = settingDto.Approver2Name;

            existingSetting.saleMoeinId = settingDto.saleMoeinId;
            existingSetting.salePartyMoeinId = settingDto.salePartyMoeinId;
            existingSetting.SaleIsAutoGenerate = settingDto.SaleIsAutoGenerate;
            existingSetting.saleDiscountMoeinId = settingDto.saleDiscountMoeinId;
            existingSetting.ReturnToSaleMoeinId = settingDto.ReturnToSaleMoeinId;
            existingSetting.SaleVatMoeinId = settingDto.SaleVatMoeinId;

            existingSetting.BuyMoeinId = settingDto.BuyMoeinId;
            existingSetting.BuyPartyMoeinId = settingDto.BuyPartyMoeinId;
            existingSetting.BuyIsAutoGenerate = settingDto.BuyIsAutoGenerate;
            existingSetting.BuyDiscountMoeinId = settingDto.BuyDiscountMoeinId;
            existingSetting.ReturnToBuyMoeinId = settingDto.ReturnToBuyMoeinId;
            existingSetting.BuyVatMoeinId = settingDto.BuyVatMoeinId;

            existingSetting.WarehouseMoeinId = settingDto.WarehouseMoeinId;
            existingSetting.WarehouseIsAutoGenerate = settingDto.WarehouseIsAutoGenerate;

            await _db.SaveChangesAsync();

            // به‌روزرسانی حافظه کش
            _cache.Remove($"SellerSettings_{settingDto.SellerId}");

            return new clsResult { Success = true, Message = "تنظیمات حسابداری با موفقیت انجام شد" };
        }

        public async Task<clsResult> AddSettingAsync(AccSettingDto settingDto)
        {
            var setting = new Acc_Setting
            {
                SellerId = settingDto.SellerId,
                AccLevel = settingDto.AccLevel,
                DocPrintDefault = settingDto.DocPrintDefault,
                ShowAllTafsil = settingDto.ShowAllTafsil,
                MandatoryTafsil = settingDto.MandatoryTafsil,
                PrintCreator = settingDto.PrintCreator,
                Approver1Title = settingDto.Approver1Title,
                Approver1Name = settingDto.Approver1Name,
                Approver2Title = settingDto.Approver2Title,
                Approver2Name = settingDto.Approver2Name,

                saleMoeinId = settingDto.saleMoeinId,
                salePartyMoeinId = settingDto.salePartyMoeinId,
                SaleIsAutoGenerate = settingDto.SaleIsAutoGenerate,
                saleDiscountMoeinId = settingDto.saleDiscountMoeinId,
                ReturnToSaleMoeinId = settingDto.ReturnToSaleMoeinId,
                SaleVatMoeinId = settingDto.SaleVatMoeinId,

                BuyMoeinId = settingDto.BuyMoeinId,
                BuyIsAutoGenerate = settingDto.BuyIsAutoGenerate,
                BuyPartyMoeinId = settingDto.BuyPartyMoeinId,
                BuyDiscountMoeinId = settingDto.BuyDiscountMoeinId,
                ReturnToBuyMoeinId = settingDto.ReturnToBuyMoeinId,
                BuyVatMoeinId = settingDto.BuyVatMoeinId,


                WarehouseIsAutoGenerate = settingDto.WarehouseIsAutoGenerate,
                WarehouseMoeinId = settingDto.WarehouseMoeinId,
            };

            await _db.Acc_Settings.AddAsync(setting);
            await _db.SaveChangesAsync();

            // ذخیره در حافظه کش
            _cache.Set($"SellerSettings_{settingDto.SellerId}", settingDto, TimeSpan.FromHours(1));

            return new clsResult { Success = true, Message = "Setting added successfully" };
        }

        public async Task<clsResult> DeleteSettingAsync(long id)
        {
            var setting = await _db.Acc_Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null)
                return new clsResult { Success = false, Message = "Setting not found" };

            _db.Acc_Settings.Remove(setting);
            await _db.SaveChangesAsync();

            // حذف از حافظه کش
            _cache.Remove($"SellerSettings_{setting.SellerId}");

            return new clsResult { Success = true, Message = "Setting deleted successfully" };
        }

        private async Task<Acc_Setting> CreateDefaultSettingsAsync(long sellerId)
        {
            var defaultSetting = new Acc_Setting
            {
                SellerId = sellerId,
                AccLevel = null,
                DocPrintDefault = 1,
                ShowAllTafsil = false,
                MandatoryTafsil = false,
                PrintCreator = true,
                Approver1Title = "مدیر امورمالی",
                Approver1Name = "....",
                Approver2Title = "مدیرعامل",
                Approver2Name = "..."
            };

            await _db.Acc_Settings.AddAsync(defaultSetting);
            await _db.SaveChangesAsync();

            return defaultSetting;
        }
    }
}
