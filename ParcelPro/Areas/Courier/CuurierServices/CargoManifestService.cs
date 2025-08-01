using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Models;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.CuurierServices
{
    public class CargoManifestService : ICargoManifestService
    {
        private readonly AppDbContext _db;
        private readonly UserContextService _userContext;
        private readonly ITrachkingService _tracking;

        public CargoManifestService(AppDbContext databaseContext,
            UserContextService userContextService,
            ITrachkingService tracking)
        {
            _db = databaseContext;
            _userContext = userContextService;
            _tracking = tracking;
        }

        // ===================== Vehicle Methods =========================================================

        public async Task<SelectList> SelectList_VehiclesAsync(long sellerId, bool onlyActive = true)
        {
            var vehicles = await _db.Cu_Vehicles
                .Where(v => v.SellerId == sellerId && (!onlyActive || v.IsActive))
                .OrderBy(v => v.PlateNumber)
                .Select(v => new
                {
                    v.Id,
                    Text = $"{v.PlateNumber} - {v.VehicleType}"
                })
                .ToListAsync();

            return new SelectList(vehicles, "Id", "Text");
        }
        public async Task<List<VehicleDto>> GetVehiclesAsync(long sellerId, bool onlyActive = false)
        {
            return await _db.Cu_Vehicles
                .AsNoTracking()
                .Where(v => v.SellerId == sellerId && (!onlyActive || v.IsActive))
                .OrderBy(v => v.PlateNumber)
                .Select(v => new VehicleDto
                {
                    Id = v.Id,
                    PlateNumber = v.PlateNumber,
                    CreatedAt = v.CreatedAt,
                    CreatedBy = v.CreatedBy,
                    Description = v.Description,
                    TafsilId = v.TafsilId,
                    MoeinId = v.MoeinId,
                    Model = v.Model,
                    SellerId = v.SellerId,
                    VehicleType = v.VehicleType,
                    LoadCapacityKg = v.LoadCapacityKg,
                    IsActive = v.IsActive
                })
                .ToListAsync();
        }
        public async Task<VehicleDto?> FindVehicleByIdAsync(int id)
        {
            var v = await _db.Cu_Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
            if (v == null) return null;

            return new VehicleDto
            {
                Id = v.Id,
                PlateNumber = v.PlateNumber,
                CreatedAt = v.CreatedAt,
                CreatedBy = v.CreatedBy,
                Description = v.Description,
                TafsilId = v.TafsilId,
                MoeinId = v.MoeinId,
                Model = v.Model,
                SellerId = v.SellerId,
                VehicleType = v.VehicleType,
                LoadCapacityKg = v.LoadCapacityKg,
                IsActive = v.IsActive
            };
        }
        public async Task<clsResult> AddVehicleAsync(VehicleDto dto, long sellerId, string createdBy)
        {
            var result = new clsResult { Success = false, ShowMessage = true };

            if (dto == null || string.IsNullOrWhiteSpace(dto.PlateNumber))
            {
                result.Message = "اطلاعات به درستی وارد نشده است.";
                return result;
            }

            bool isDuplicate = await _db.Cu_Vehicles.AnyAsync(v =>
                v.SellerId == sellerId &&
                v.PlateNumber == dto.PlateNumber
            );
            if (isDuplicate)
            {
                result.Message = "پلاک خودرو تکراری است.";
                return result;
            }

            var vehicle = new Cu_Vehicle
            {
                SellerId = sellerId,
                CreatedBy = createdBy,
                PlateNumber = dto.PlateNumber,
                VehicleType = dto.VehicleType,
                LoadCapacityKg = (int)dto.LoadCapacityKg,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                Model = dto.Model,
                MoeinId = dto.MoeinId,
                TafsilId = dto.TafsilId,
                Description = dto.Description,
            };

            _db.Cu_Vehicles.Add(vehicle);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "خودرو با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطا در زمان ثبت اطلاعات:</br> {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> UpdateVehicleAsync(int id, VehicleDto dto)
        {
            var result = new clsResult { Success = false, ShowMessage = true };

            if (dto == null || string.IsNullOrWhiteSpace(dto.PlateNumber))
            {
                result.Message = "اطلاعات به درستی وارد نشده است.";
                return result;
            }

            var vehicle = await _db.Cu_Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                result.Message = "خودرویی با این شناسه یافت نشد.";
                return result;
            }

            bool isDuplicate = await _db.Cu_Vehicles.AnyAsync(v =>
                v.Id != id &&
                v.SellerId == vehicle.SellerId &&
                v.PlateNumber == dto.PlateNumber
            );
            if (isDuplicate)
            {
                result.Message = "پلاک خودرو تکراری است.";
                return result;
            }

            vehicle.PlateNumber = dto.PlateNumber;
            vehicle.VehicleType = dto.VehicleType;
            vehicle.LoadCapacityKg = (int)dto.LoadCapacityKg;
            vehicle.IsActive = dto.IsActive;
            vehicle.CreatedAt = dto.CreatedAt;
            vehicle.Model = dto.Model;
            vehicle.MoeinId = dto.MoeinId;
            vehicle.TafsilId = dto.TafsilId;
            vehicle.Description = dto.Description;

            _db.Cu_Vehicles.Update(vehicle);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "خودرو با موفقیت به‌روزرسانی شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطا در زمان به‌روزرسانی:</br> {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> DeleteVehicleAsync(int id)
        {
            var result = new clsResult { Success = false, ShowMessage = true };

            var vehicle = await _db.Cu_Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                result.Message = "خودرویی با این شناسه یافت نشد.";
                return result;
            }

            _db.Cu_Vehicles.Remove(vehicle);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "خودرو با موفقیت حذف شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطا در زمان حذف:</br> {ex.Message}";
            }

            return result;
        }


        // ===================== Driver Methods ========================================================

        public async Task<SelectList> SelectList_DriversAsync(long sellerId)
        {
            var Drivers = await _db.Cu_Drivers
               .Where(v => v.SellerId == sellerId && v.IsActive)
               .OrderBy(v => v.FullName)
               .Select(v => new
               {
                   Id = v.Id,
                   Text = $"{v.FullName}"
               })
               .ToListAsync();

            return new SelectList(Drivers, "Id", "Text");
        }

        public async Task<List<DriverDto>> GetDriversAsync(long sellerId, bool onlyActive = false)
        {
            return await _db.Cu_Drivers
                .AsNoTracking()
                .Where(d => d.SellerId == sellerId && (!onlyActive || d.IsActive))
                .OrderBy(d => d.FullName)
                .Select(d => new DriverDto
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    PhoneNumber = d.PhoneNumber,
                    NationalCode = d.NationalCode,
                    LicenseExpiryDate = d.LicenseExpiryDate,
                    LicenseNumber = d.LicenseNumber,
                    DriverPhoto = d.DriverPhoto,
                    CreatedAt = d.CreatedAt,
                    CreatedBy = d.CreatedBy,
                    SellerId = sellerId,
                    IsActive = d.IsActive
                })
                .ToListAsync();
        }
        public async Task<DriverDto> FindDriverByIdAsync(int id)
        {
            var d = await _db.Cu_Drivers.AsNoTracking().SingleOrDefaultAsync(n => n.Id == id);

            if (d == null)
                return null;

            DriverDto dto = new DriverDto();
            dto.Id = d.Id;
            dto.FullName = d.FullName;
            dto.PhoneNumber = d.PhoneNumber;
            dto.NationalCode = d.NationalCode;
            dto.LicenseExpiryDate = d.LicenseExpiryDate;
            dto.LicenseNumber = d.LicenseNumber;
            dto.DriverPhoto = d.DriverPhoto;
            dto.CreatedAt = d.CreatedAt;
            dto.CreatedBy = d.CreatedBy;
            dto.SellerId = d.SellerId;
            dto.IsActive = d.IsActive;

            return dto;

        }
        public async Task<clsResult> AddDriverAsync(DriverDto dto)
        {
            var result = new clsResult { Success = false, ShowMessage = true };

            if (dto == null || string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                result.Message = "اطلاعات راننده ناقص است.";
                return result;
            }

            bool isDuplicate = await _db.Cu_Drivers.AnyAsync(d =>
                d.SellerId == dto.SellerId &&
                d.PhoneNumber == dto.PhoneNumber
            );
            if (isDuplicate)
            {
                result.Message = "شماره موبایل تکراری است.";
                return result;
            }

            var driver = new Cu_Driver
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                NationalCode = dto.NationalCode,
                LicenseExpiryDate = dto.LicenseExpiryDate,
                LicenseNumber = dto.LicenseNumber,
                DriverPhoto = dto.DriverPhoto,
                CreatedAt = dto.CreatedAt,
                CreatedBy = dto.CreatedBy,
                SellerId = dto.SellerId,
                IsActive = dto.IsActive
            };

            _db.Cu_Drivers.Add(driver);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "راننده با موفقیت ثبت شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطا در ثبت راننده:</br> {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> UpdateDriverAsync(DriverDto dto)
        {
            var result = new clsResult { Success = false, ShowMessage = true };

            if (dto == null || string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                result.Message = "اطلاعات راننده ناقص است.";
                return result;
            }

            var driver = await _db.Cu_Drivers.FindAsync(dto.Id);
            if (driver == null)
            {
                result.Message = "راننده‌ای با این شناسه یافت نشد.";
                return result;
            }

            bool isDuplicate = await _db.Cu_Drivers.AnyAsync(d =>
                d.Id != dto.Id &&
                d.SellerId == driver.SellerId &&
                d.PhoneNumber == dto.PhoneNumber
            );
            if (isDuplicate)
            {
                result.Message = "شماره موبایل تکراری است.";
                return result;
            }

            driver.FullName = dto.FullName;
            driver.PhoneNumber = dto.PhoneNumber;
            driver.NationalCode = dto.NationalCode;
            driver.LicenseExpiryDate = dto.LicenseExpiryDate;
            driver.LicenseNumber = dto.LicenseNumber;
            driver.DriverPhoto = dto.DriverPhoto;
            driver.CreatedAt = dto.CreatedAt;
            driver.CreatedBy = dto.CreatedBy;
            driver.SellerId = dto.SellerId;
            driver.IsActive = dto.IsActive;

            _db.Cu_Drivers.Update(driver);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "اطلاعات راننده با موفقیت به‌روزرسانی شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطا در به‌روزرسانی:</br> {ex.Message}";
            }

            return result;
        }
        public async Task<clsResult> DeleteDriverAsync(int id)
        {
            var result = new clsResult { Success = false, ShowMessage = true };

            var driver = await _db.Cu_Drivers.FindAsync(id);
            if (driver == null)
            {
                result.Message = "راننده‌ای با این شناسه یافت نشد.";
                return result;
            }

            _db.Cu_Drivers.Remove(driver);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "راننده با موفقیت حذف شد.";
            }
            catch (Exception ex)
            {
                result.Message = $"خطا در حذف:</br> {ex.Message}";
            }

            return result;
        }


        //====================== Cargo Manifest Methods =======================================



    }
}
