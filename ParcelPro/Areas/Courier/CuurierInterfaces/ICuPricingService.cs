using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Courier.Dto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface ICuPricingService
    {
        //انواع تٔاثیر بر نرخ
        Task<SelectList> SelectList_RateImpactTypeAsync();

        // دریافت لیست انواع تأثیر بر نرخ پایه
        Task<List<RateImpactTypeDto>> GetRateImpactTypesAsync();

        // دریافت لیست رنج‌های وزنی
        Task<List<RateWeightRangeDto>> GetRateWeightRangesAsync();

        // دریافت یک رنج وزنی بر اساس شناسه
        Task<RateWeightRangeDto> GetRateWeightRangeByIdAsync(int id);
        //=============================================================================
        // دریافت لیست مناطق قیمت‌گذاری
        Task<SelectList> SelectList_ZonesAsync(long sellerId);
        Task<List<RateZoneDto>> GetRateZonesAsync();
        Task<RateZoneDto> GetRateZoneDtoAsync(int id);
        Task<clsResult> AddRateZonesAsync(RateZoneDto dto);
        Task<clsResult> UpdateRateZonesAsync(RateZoneDto dto);


        //============================================================================
        Task<SelectList> SelectList_ConsigmentNatureAsync();
        // دریافت لیست ماهیت‌های محموله
        Task<List<ConsignmentNatureDto>> GetConsignmentNaturesAsync();
        Task<clsResult> AddConsigmentNatureAsync(ConsignmentNatureDto dto);
        Task<clsResult> UpdateConsigmentNatureAsync(ConsignmentNatureDto dto);
        Task<clsResult> DeleteConsigmentNatureAsync(int id);

        // دریافت لیست آیتم‌های هزینه
        Task<List<CostItemDto>> GetCostItemsAsync();

        // افزودن یک رنج وزنی جدید
        Task<clsResult> AddRateWeightRangeAsync(RateWeightRangeDto dto);
        Task<clsResult> AddWeightFactorIncrementAsync(decimal startWeight, decimal increaseValue);

        // بروزرسانی یک رنج وزنی
        Task<clsResult> UpdateRateWeightRangeAsync(RateWeightRangeDto dto);

        // حذف یک رنج وزنی
        Task<clsResult> DeleteRateWeightRangeAsync(int id);

        Task<CostItemDto> GetCostItemByIdAsync(int id);

        // افزودن یک آیتم هزینه جدید
        Task<clsResult> AddCostItemAsync(CostItemDto dto);

        // بروزرسانی یک آیتم هزینه
        Task<clsResult> UpdateCostItemAsync(CostItemDto dto);

        // حذف یک آیتم هزینه
        Task<clsResult> DeleteCostItemAsync(int id);

        Task<RateImpactTypeDto> GetRateImpactTypeByIdAsync(short id);
        Task<RateZoneDto> GetRateZoneByIdAsync(int id);
        Task<ConsignmentNatureDto> GetConsignmentNatureByIdAsync(short id);
        Task<List<AddParcelCostDto>> GetDefultCostsAsync(long sellerId);

        // ============================================================================== Insurance Setting
        Task<InsuranceSettingsDto> GetInsuranceSettingAsync(long sellerId);
        Task<InsuranceSettingsDto> SetInsuranceSettingAsync(InsuranceSettingsDto dto);
        Task<long> CalculateInsuranceCostAsync(long sellerId, long amount);

        // ============================================================================== Get Prices
        Task<ComputedPriceDto> GetParcellPriceAsync(ParcelPricingItemDto dto);
        Task<long> GetPackagePriceAsync(long id);
    }
}