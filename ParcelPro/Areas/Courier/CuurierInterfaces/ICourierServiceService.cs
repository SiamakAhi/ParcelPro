using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Courier.Dto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface ICourierServiceService
    {
        //========================================================== Service
        Task<SelectList> SelectList_ServicesAsync(long sellerId);
        SelectList ServiceRatingType();
        Task<List<Cu_ServiceDto>> GetServicesAsync(long sellerId);
        Task<Cu_ServiceDto> FindServiceByIdAsync(int id);
        Task<clsResult> AddServiceAsync(Cu_ServiceDto dto);
        Task<clsResult> UpdateServiceAsync(Cu_ServiceDto dto);
        Task<clsResult> DeleteServiceAsync(int id);
        //================================================================================== Branch Servic 
        Task<clsResult> AddServiceToBranchAsync(Guid branchId, int serviceId, long sellerId);
        Task<clsResult> RemoveBranchServiceAsync(long id);
        Task<SelectList> SelectList_BranchServicesAsync(Guid BranchId);
        Task<List<BranchServiceDto>> GetBranchServicesAsync(Guid branchId);



        //============================================================ Route
        Task<List<RouteDto>> GetRoutesAsync(long sellerId);
        Task<RouteDto> FindRouteByIdAsync(int id);
        Task<clsResult> AddRouteAsync(RouteDto dto);
        Task<clsResult> UpdateRouteAsync(RouteDto dto);
        Task<clsResult> DeleteRouteAsync(int id);
        Task<List<RouteDto>> GetRoutesByCityAsync(long sellerId, int cityId);
        Task<List<RouteDto>> GetRoutesByOriginCityAsync(long sellerId, int originCityId);
        Task<SelectList> SelectList_RoutesByOriginCityAsync(long sellerId, int originCityId);
        Task<SelectList> SelectList_RoutesByDestinationCityAsync(long sellerId, int originCityId);
        Task<SelectList> SelectList_RoutesAsync(long sellerId);


        //============================================================= Packaging
        Task<SelectList> SelectList_PackagesAsync(long sellerId, bool forExport = false);
        Task<List<PackagingDto>> GetPackagingsAsync(long sellerId);
        Task<PackagingDto> GetPackagingDtoAsync(int id);
        Task<clsResult> AddPackagingAsync(PackagingDto dto);
        Task<clsResult> UpdatePackagingAsync(PackagingDto dto);
        Task<clsResult> DeletePackagingAsync(int id);
        Task<SelectList> SelectLst_PackagesAsync(long productCategory);
        Task<long> GetPackagePriceAsync(long productId);

    }
}
