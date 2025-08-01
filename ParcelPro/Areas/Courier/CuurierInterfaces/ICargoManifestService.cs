using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Courier.Dto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface ICargoManifestService
    {

        //========================================================== Vehicle ======================
        Task<SelectList> SelectList_VehiclesAsync(long sellerId, bool onlyActive = true);
        Task<List<VehicleDto>> GetVehiclesAsync(long sellerId, bool onlyActive = false);
        Task<VehicleDto?> FindVehicleByIdAsync(int id);
        Task<clsResult> AddVehicleAsync(VehicleDto dto, long sellerId, string createdBy);
        Task<clsResult> UpdateVehicleAsync(int id, VehicleDto dto);
        Task<clsResult> DeleteVehicleAsync(int id);


        //========================================================== DRIVER ======================
        Task<SelectList> SelectList_DriversAsync(long sellerId);
        Task<List<DriverDto>> GetDriversAsync(long sellerId, bool onlyActive = false);
        Task<DriverDto> FindDriverByIdAsync(int id);
        Task<clsResult> AddDriverAsync(DriverDto dto);
        Task<clsResult> UpdateDriverAsync(DriverDto dto);
        Task<clsResult> DeleteDriverAsync(int id);

        //========================================================== MANIFEST =====================


    }
}
