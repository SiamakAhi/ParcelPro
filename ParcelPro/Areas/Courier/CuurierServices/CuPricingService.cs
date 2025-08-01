using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Areas.Warehouse.WarehouseInterfaces;
using ParcelPro.Models;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.CuurierServices
{
    public class CuPricingService : ICuPricingService
    {
        private readonly AppDbContext _db;
        private readonly UserContextService _userContextService;
        private readonly IWhProductService _productService;

        public CuPricingService(AppDbContext context, UserContextService userContextService, IWhProductService whProductService)
        {
            _db = context;
            _userContextService = userContextService;
            _productService = whProductService;
        }
        public async Task<SelectList> SelectList_RateImpactTypeAsync()
        {
            var lst = await _db.Cu_RateImpactTypes.Select(n => new { id = n.Id, name = n.Name }).ToListAsync();
            return new SelectList(lst, "id", "name");
        }
        public async Task<List<RateImpactTypeDto>> GetRateImpactTypesAsync()
        {
            if (_userContextService.SellerId == null)
                return new List<RateImpactTypeDto>();

            var data = await _db.Cu_RateImpactTypes
                .Select(n => new RateImpactTypeDto
                {
                    Id = n.Id,
                    RateImpactTypeCode = n.RateImpactTypeCode,
                    Name = n.Name,
                    Description = n.Description
                })
                .ToListAsync();

            return data;
        }

        //================================================================================== Zone
        public async Task<SelectList> SelectList_ZonesAsync(long sellerId)
        {
            var data = await _db.Cu_RateZones.Where(n => n.SellerId == sellerId)
                .Select(n => new
                {
                    id = n.ZoneId,
                    name = n.Name,
                })
                .ToListAsync();

            return new SelectList(data, "id", "name");
        }
        public async Task<List<RateZoneDto>> GetRateZonesAsync()
        {
            if (_userContextService.SellerId == null)
                return new List<RateZoneDto>();

            var data = await _db.Cu_RateZones
                .Select(n => new RateZoneDto
                {
                    ZoneId = n.ZoneId,
                    SellerId = n.SellerId,
                    Name = n.Name,
                    IsSatellite = n.IsSatellite,
                    PriceBaseFactor = n.PriceBaseFactor
                })
                .ToListAsync();

            return data;
        }
        public async Task<RateZoneDto> GetRateZoneDtoAsync(int id)
        {
            if (_userContextService.SellerId == null)
                return new RateZoneDto();

            var data = await _db.Cu_RateZones.Where(n => n.ZoneId == id)
                .Select(n => new RateZoneDto
                {
                    ZoneId = n.ZoneId,
                    SellerId = n.SellerId,
                    Name = n.Name,
                    IsSatellite = n.IsSatellite,
                    PriceBaseFactor = n.PriceBaseFactor
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<clsResult> AddRateZonesAsync(RateZoneDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (_userContextService.SellerId == null)
            {
                result.Message = "شرکت فعالی یافت نشد";
                return result;
            }

            if (dto == null)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }

            var zone = new Cu_RateZone
            {
                Name = dto.Name,
                IsSatellite = dto.IsSatellite,
                PriceBaseFactor = dto.PriceBaseFactor,
                SellerId = dto.SellerId,
            };

            _db.Cu_RateZones.Add(zone);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "زون جدید با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان ثبت اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> UpdateRateZonesAsync(RateZoneDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (_userContextService.SellerId == null)
            {
                result.Message = "شرکت فعالی یافت نشد";
                return result;
            }
            var zone = await _db.Cu_RateZones.FindAsync(dto.ZoneId);
            if (dto == null)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }
            zone.Name = dto.Name;
            zone.IsSatellite = dto.IsSatellite;
            zone.PriceBaseFactor = dto.PriceBaseFactor;
            zone.SellerId = dto.SellerId;

            _db.Cu_RateZones.Update(zone);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "تغییرات زون با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان ثبت اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> SeleteRateZonesAsync(int id)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            var zone = await _db.Cu_RateZones.FindAsync(id);
            if (zone == null)
            {
                result.Message = "ماهیت یافت نشد";
                return result;
            }

            _db.Cu_RateZones.Remove(zone);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "زون با موفقیت حذف شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان حذف اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }

        //================================================================================== Nature
        public async Task<SelectList> SelectList_ConsigmentNatureAsync()
        {
            if (_userContextService.SellerId == null)
                return null;

            var data = await _db.Cu_ConsignmentNatures.Where(n => n.SellerId == _userContextService.SellerId.Value)
               .Select(n => new
               {
                   id = n.Id,
                   name = n.Name,
               }).ToListAsync();

            return new SelectList(data, "id", "name");
        }
        // دریافت لیست ماهیت‌های محموله
        public async Task<List<ConsignmentNatureDto>> GetConsignmentNaturesAsync()
        {
            if (_userContextService.SellerId == null)
                return new List<ConsignmentNatureDto>();

            var data = await _db.Cu_ConsignmentNatures
                .Select(n => new ConsignmentNatureDto
                {
                    Id = n.Id,
                    SellerId = n.SellerId,
                    Name = n.Name,
                    Code = n.Code,
                    IsAirTransportable = n.IsAirTransportable,
                    IsGroundTransportable = n.IsGroundTransportable,
                    RateImpactTypeId = n.RateImpactTypeId,
                    RateImpactValue = n.RateImpactValue
                })
                .ToListAsync();

            return data;
        }
        public async Task<clsResult> AddConsigmentNatureAsync(ConsignmentNatureDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (_userContextService.SellerId == null)
            {
                result.Message = "شرکت فعالی یافت نشد";
                return result;
            }

            if (dto == null)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }

            var nature = new Cu_ConsignmentNature
            {
                Code = dto.Code,
                Name = dto.Name,
                IsAirTransportable = dto.IsAirTransportable,
                IsGroundTransportable = dto.IsGroundTransportable,
                RateImpactTypeId = dto.RateImpactTypeId,
                RateImpactValue = dto.RateImpactValue,
                SellerId = dto.SellerId,
            };

            _db.Cu_ConsignmentNatures.Add(nature);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "ماهیت جدید با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان ثبت اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> UpdateConsigmentNatureAsync(ConsignmentNatureDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (dto == null || dto.Id == 0)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }

            var nature = await _db.Cu_ConsignmentNatures.FindAsync(dto.Id);
            if (nature == null)
            {
                result.Message = "رنج وزنی یافت نشد";
                return result;
            }

            nature.Name = dto.Name;
            nature.IsAirTransportable = dto.IsAirTransportable;
            nature.IsGroundTransportable = dto.IsGroundTransportable;
            nature.RateImpactTypeId = dto.RateImpactTypeId;
            nature.RateImpactValue = dto.RateImpactValue;
            nature.SellerId = dto.SellerId;

            _db.Cu_ConsignmentNatures.Update(nature);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "اطلاعات ماهیت با موفقیت بروزسانی شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان بروزرسانی اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> DeleteConsigmentNatureAsync(int id)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            var nature = await _db.Cu_ConsignmentNatures.FindAsync(id);
            if (nature == null)
            {
                result.Message = "ماهیت یافت نشد";
                return result;
            }

            _db.Cu_ConsignmentNatures.Remove(nature);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "ماهیت با موفقیت حذف شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان حذف اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }


        //================================================================================== WeightRange
        // دریافت لیست رنج‌های وزنی
        public async Task<List<RateWeightRangeDto>> GetRateWeightRangesAsync()
        {
            if (_userContextService.SellerId == null)
                return new List<RateWeightRangeDto>();

            var data = await _db.Cu_RateWeightRanges
                .Select(n => new RateWeightRangeDto
                {
                    Id = n.Id,
                    StartWeight = n.StartWeight,
                    EndWeight = n.EndWeight,
                    WeightFactorPercent = n.Courier_WeightFactorPercent
                })
                .ToListAsync();

            return data;
        }
        // دریافت یک رنج وزنی بر اساس شناسه
        public async Task<RateWeightRangeDto> GetRateWeightRangeByIdAsync(int id)
        {
            if (_userContextService.SellerId == null)
                return null;

            var rateWeightRange = await _db.Cu_RateWeightRanges
                .FirstOrDefaultAsync(n => n.Id == id);

            if (rateWeightRange == null)
                return null;

            return new RateWeightRangeDto
            {
                Id = rateWeightRange.Id,
                StartWeight = rateWeightRange.StartWeight,
                EndWeight = rateWeightRange.EndWeight,
                WeightFactorPercent = rateWeightRange.Courier_WeightFactorPercent
            };
        }
        // افزودن یک رنج وزنی جدید
        public async Task<clsResult> AddRateWeightRangeAsync(RateWeightRangeDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (_userContextService.SellerId == null)
            {
                result.Message = "شرکت فعالی یافت نشد";
                return result;
            }

            if (dto == null)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }

            var rateWeightRange = new Cu_RateWeightRange
            {
                StartWeight = dto.StartWeight,
                EndWeight = dto.EndWeight,
                Courier_WeightFactorPercent = dto.WeightFactorPercent
            };

            _db.Cu_RateWeightRanges.Add(rateWeightRange);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "رنج وزنی جدید با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان ثبت اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> AddWeightFactorIncrementAsync(decimal startWeight, decimal increaseValue)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            // انتخاب رنج‌های وزنی با مقدار StartWeight بزرگتر یا مساوی نقطه شروع ورودی
            var records = _db.Cu_RateWeightRanges
                .Where(r => (decimal)r.StartWeight >= startWeight)
                .OrderBy(r => r.StartWeight)
                .ToList();

            if (!records.Any())
            {
                result.Message = "رنج وزنی‌ای با مقدار شروع بزرگتر یا مساوی ورودی یافت نشد.";
                return result;
            }

            decimal newRate = records.FirstOrDefault().Courier_WeightFactorPercent;
            foreach (var record in records)
            {
                record.Courier_WeightFactorPercent = newRate;
                record.IATA_WeightFactorPercent = newRate;
                newRate += increaseValue;
            }

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "رنج‌های وزنی با موفقیت به‌روزرسانی شدند.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطا در به‌روزرسانی اطلاعات: {ex.Message}";
            }

            return result;
        }

        // بروزرسانی یک رنج وزنی
        public async Task<clsResult> UpdateRateWeightRangeAsync(RateWeightRangeDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (dto == null || dto.Id == 0)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }

            var rateWeightRange = await _db.Cu_RateWeightRanges.FindAsync(dto.Id);
            if (rateWeightRange == null)
            {
                result.Message = "رنج وزنی یافت نشد";
                return result;
            }

            rateWeightRange.StartWeight = dto.StartWeight;
            rateWeightRange.EndWeight = dto.EndWeight;
            rateWeightRange.Courier_WeightFactorPercent = dto.WeightFactorPercent;

            _db.Cu_RateWeightRanges.Update(rateWeightRange);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "اطلاعات رنج وزنی با موفقیت بروزسانی شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان بروزرسانی اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        // حذف یک رنج وزنی
        public async Task<clsResult> DeleteRateWeightRangeAsync(int id)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            var rateWeightRange = await _db.Cu_RateWeightRanges.FindAsync(id);
            if (rateWeightRange == null)
            {
                result.Message = "رنج وزنی یافت نشد";
                return result;
            }

            _db.Cu_RateWeightRanges.Remove(rateWeightRange);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "رنج وزنی با موفقیت حذف شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان حذف اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }

        //================================================================================== Cost Item
        // دریافت لیست آیتم‌های هزینه
        public async Task<List<CostItemDto>> GetCostItemsAsync()
        {
            if (_userContextService.SellerId == null)
                return new List<CostItemDto>();

            var data = await _db.Cu_BillOfLadingCostItems
                .Select(n => new CostItemDto
                {
                    Id = n.Id,
                    SellerId = n.SellerId,
                    CostCode = n.CostCode,
                    Description = n.Description,
                    RateImpactTypeCode = n.RateImpactTypeCode,
                    Amount = n.Amount,
                    ForBillOfLading = n.ForBillOfLading,
                    ForConsignment = n.ForConsignment,
                    IsAutoAdded = n.IsAutoAdded,
                    AccountMoeinId = n.AccountMoeinId,
                    AccountTafsilId = n.AccountTafsilId,
                    RateImpactTypeName = n.RateImpactTypeCode.ToRateImpactType(),
                })
                .ToListAsync();

            return data;
        }
        public async Task<CostItemDto> GetCostItemByIdAsync(int id)
        {
            if (_userContextService.SellerId == null)
                return null;

            var costItem = await _db.Cu_BillOfLadingCostItems
                .FirstOrDefaultAsync(n => n.Id == id);

            if (costItem == null)
                return null;

            var costItemDto = new CostItemDto();
            costItemDto.Id = costItem.Id;
            costItemDto.SellerId = costItem.SellerId;
            costItemDto.CostCode = costItem.CostCode;
            costItemDto.Description = costItem.Description;
            costItemDto.RateImpactTypeCode = costItem.RateImpactTypeCode;
            costItemDto.Amount = costItem.Amount;
            costItemDto.ForBillOfLading = costItem.ForBillOfLading;
            costItemDto.ForConsignment = costItem.ForConsignment;
            costItemDto.IsAutoAdded = costItem.IsAutoAdded;
            costItemDto.AccountMoeinId = costItem.AccountMoeinId;
            costItemDto.AccountTafsilId = costItem.AccountTafsilId;

            return costItemDto;
        }
        // افزودن یک آیتم هزینه جدید
        public async Task<clsResult> AddCostItemAsync(CostItemDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (_userContextService.SellerId == null)
            {
                result.Message = "شرکت فعالی یافت نشد";
                return result;
            }

            if (dto == null)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }

            var costItem = new Cu_BillOfLadingCostItem
            {
                SellerId = _userContextService.SellerId.Value,
                CostCode = dto.CostCode,
                Description = dto.Description,
                RateImpactTypeCode = dto.RateImpactTypeCode,
                Amount = dto.Amount,
                ForBillOfLading = dto.ForBillOfLading,
                ForConsignment = dto.ForConsignment,
                IsAutoAdded = dto.IsAutoAdded,
                AccountMoeinId = dto.AccountMoeinId,
                AccountTafsilId = dto.AccountTafsilId
            };

            _db.Cu_BillOfLadingCostItems.Add(costItem);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "آیتم هزینه جدید با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان ثبت اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        // بروزرسانی یک آیتم هزینه
        public async Task<clsResult> UpdateCostItemAsync(CostItemDto dto)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            if (dto == null || dto.Id == 0)
            {
                result.Message = "اطلاعات بدرستی وارد نشده است";
                return result;
            }

            var costItem = await _db.Cu_BillOfLadingCostItems.FindAsync(dto.Id);
            if (costItem == null)
            {
                result.Message = "آیتم هزینه یافت نشد";
                return result;
            }

            costItem.CostCode = dto.CostCode;
            costItem.Description = dto.Description;
            costItem.RateImpactTypeCode = dto.RateImpactTypeCode;
            costItem.Amount = dto.Amount;
            costItem.ForBillOfLading = dto.ForBillOfLading;
            costItem.ForConsignment = dto.ForConsignment;
            costItem.IsAutoAdded = dto.IsAutoAdded;
            costItem.AccountMoeinId = dto.AccountMoeinId;
            costItem.AccountTafsilId = dto.AccountTafsilId;

            _db.Cu_BillOfLadingCostItems.Update(costItem);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "اطلاعات آیتم هزینه با موفقیت بروزسانی شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان بروزرسانی اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        // حذف یک آیتم هزینه
        public async Task<clsResult> DeleteCostItemAsync(int id)
        {
            clsResult result = new clsResult { Success = false, ShowMessage = true };

            var costItem = await _db.Cu_BillOfLadingCostItems.FindAsync(id);
            if (costItem == null)
            {
                result.Message = "آیتم هزینه یافت نشد";
                return result;
            }

            _db.Cu_BillOfLadingCostItems.Remove(costItem);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "آیتم هزینه با موفقیت حذف شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطایی در زمان حذف اطلاعات رخ داده است.\n {ex.Message}";
            }

            return result;
        }
        public async Task<RateImpactTypeDto> GetRateImpactTypeByIdAsync(short id)
        {
            if (_userContextService.SellerId == null)
                return null;

            var rateImpactType = await _db.Cu_RateImpactTypes
                .FirstOrDefaultAsync(n => n.Id == id);

            if (rateImpactType == null)
                return null;

            var rateImpactTypeDto = new RateImpactTypeDto();
            rateImpactTypeDto.Id = rateImpactType.Id;
            rateImpactTypeDto.RateImpactTypeCode = rateImpactType.RateImpactTypeCode;
            rateImpactTypeDto.Name = rateImpactType.Name;
            rateImpactTypeDto.Description = rateImpactType.Description;

            return rateImpactTypeDto;
        }
        public async Task<RateZoneDto> GetRateZoneByIdAsync(int id)
        {
            if (_userContextService.SellerId == null)
                return null;

            var rateZone = await _db.Cu_RateZones
                .FirstOrDefaultAsync(n => n.ZoneId == id);

            if (rateZone == null)
                return null;

            var rateZoneDto = new RateZoneDto();
            rateZoneDto.ZoneId = rateZone.ZoneId;
            rateZoneDto.SellerId = rateZone.SellerId;
            rateZoneDto.Name = rateZone.Name;
            rateZoneDto.IsSatellite = rateZone.IsSatellite;

            return rateZoneDto;
        }
        public async Task<ConsignmentNatureDto> GetConsignmentNatureByIdAsync(short id)
        {
            if (_userContextService.SellerId == null)
                return null;

            var consignmentNature = await _db.Cu_ConsignmentNatures
                .FirstOrDefaultAsync(n => n.Id == id);

            if (consignmentNature == null)
                return null;

            var consignmentNatureDto = new ConsignmentNatureDto();
            consignmentNatureDto.Id = consignmentNature.Id;
            consignmentNatureDto.SellerId = consignmentNature.SellerId;
            consignmentNatureDto.Name = consignmentNature.Name;
            consignmentNatureDto.Code = consignmentNature.Code;
            consignmentNatureDto.IsAirTransportable = consignmentNature.IsAirTransportable;
            consignmentNatureDto.IsGroundTransportable = consignmentNature.IsGroundTransportable;
            consignmentNatureDto.RateImpactTypeId = consignmentNature.RateImpactTypeId;
            consignmentNatureDto.RateImpactValue = consignmentNature.RateImpactValue;

            return consignmentNatureDto;
        }
        public async Task<List<AddParcelCostDto>> GetDefultCostsAsync(long sellerId)
        {
            var costs = await _db.Cu_BillOfLadingCostItems
                .Where(n => n.SellerId == sellerId && n.IsAutoAdded && n.ForConsignment)
                .Select(n => new AddParcelCostDto
                {
                    CostId = n.Id,
                    Code = n.CostCode,
                    CostName = n.Description,
                    Amount = n.Amount,
                    ImpactType = n.RateImpactTypeCode

                }).OrderBy(n => n.Code).ToListAsync();
            return costs;
        }

        //======================================================================================= Insurance Setting

        public async Task<InsuranceSettingsDto> GetInsuranceSettingAsync(long sellerId)
        {
            InsuranceSettingsDto dto = new InsuranceSettingsDto();
            var setting = await _db.Cu_InsuranceSettings.FirstOrDefaultAsync(n => n.SellerId == sellerId);
            if (setting == null)
            {
                Cu_InsuranceSettings DefultSetting = new Cu_InsuranceSettings
                {
                    SellerId = sellerId,
                    ThresholdAmount = 10000000,
                    BaseCost = 5000,
                    IncrementPerUnit = 30000,
                };
                _db.Cu_InsuranceSettings.Add(DefultSetting);
                try
                {
                    await _db.SaveChangesAsync();
                    setting = DefultSetting;
                }
                catch
                {
                    return null;
                }
            }

            dto.SellerId = sellerId;
            dto.Id = setting.Id;
            dto.IncrementPerUnit = setting.IncrementPerUnit;
            dto.BaseCost = setting.BaseCost;
            dto.ThresholdAmount = setting.ThresholdAmount;
            return dto;

        }
        public async Task<InsuranceSettingsDto> SetInsuranceSettingAsync(InsuranceSettingsDto dto)
        {
            var setting = await _db.Cu_InsuranceSettings.FindAsync(dto.Id);
            setting.IncrementPerUnit = dto.IncrementPerUnit;
            setting.ThresholdAmount = dto.ThresholdAmount;
            setting.BaseCost = dto.BaseCost;

            _db.Cu_InsuranceSettings.Update(setting);
            await _db.SaveChangesAsync();

            dto.Id = dto.Id;
            dto.SellerId = dto.SellerId;
            dto.IncrementPerUnit = setting.IncrementPerUnit;
            dto.BaseCost = setting.BaseCost;
            dto.ThresholdAmount = setting.ThresholdAmount;
            return dto;

        }
        public async Task<long> CalculateInsuranceCostAsync(long sellerId, long amount)
        {
            // گرفتن تنظیمات بیمه بر اساس SellerId
            var insuranceSettings = await GetInsuranceSettingAsync(sellerId);

            if (insuranceSettings == null)
            {
                throw new InvalidOperationException("تنظیمات بیمه برای فروشنده مشخص شده وجود ندارد.");
            }

            if (amount <= 0) return (long)insuranceSettings.BaseCost;
            // محاسبه تعداد آستانه‌ها (با گرد کردن بالا)
            int thresholdCount = (int)Math.Ceiling((double)(amount / insuranceSettings.ThresholdAmount)) - 1;
            // محاسبه حق بیمه نهایی
            long insuranceCost = (long)(insuranceSettings.BaseCost + (thresholdCount * insuranceSettings.IncrementPerUnit));

            return insuranceCost;
        }

        //----------------------------------------------- Get Prices
        public async Task<ComputedPriceDto> GetParcellPriceAsync(ParcelPricingItemDto dto)
        {
            ComputedPriceDto price = new ComputedPriceDto();

            try
            {
                // Base Value
                var baseValue = await _db.Cu_RateBaseKValues.FirstOrDefaultAsync(n => n.SellerId == dto.SellerId);
                if (baseValue == null) return price;

                // Route and Zone
                var route = await _db.Cu_Routes.FindAsync(dto.RouteId);
                if (route == null || route.ZoneId == null) return price;

                var zone = await _db.Cu_RateZones.FindAsync(route.ZoneId.Value);
                if (zone == null) return price;

                // Service
                var service = await _db.Cu_Services.FindAsync(dto.ServiceId);
                if (service == null) return price;

                // Weight Range
                var weightRange = await _db.Cu_RateWeightRanges
                    .FirstOrDefaultAsync(n => n.StartWeight <= dto.Weight && n.EndWeight >= dto.Weight);

                // Nature
                var nature = await _db.Cu_ConsignmentNatures.FindAsync(dto.NatureId);

                // Variables
                decimal BaseValue = baseValue.KValue;
                decimal basePrice = zone.PriceBaseFactor * BaseValue;
                decimal ServiceRate = service.ServicePercentage;
                decimal PriceBaseFactor = zone.PriceBaseFactor;
                decimal natureRate = nature?.RateImpactValue ?? 0;
                decimal weightRate = weightRange?.Courier_WeightFactorPercent ?? 0;

                // Calculations
                long afterService = (long)(((ServiceRate * basePrice) / 100) + basePrice);
                long afterNature = (long)(((natureRate * afterService) / 100) + afterService);
                long afterWeight = (long)(((weightRate * afterNature) / 100) + afterNature);

                price.Price = afterWeight;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetParcellPriceAsync: {ex.Message}");
            }

            return price;
        }
        public async Task<long> GetPackagePriceAsync(long id)
        {
            var p = await _db.Wh_Products.FindAsync(id);
            if (p == null) return 0;
            long price = p.UnitPrice.HasValue ? (long)(p.UnitPrice.Value) : 0;
            return price;
        }

    }
}

