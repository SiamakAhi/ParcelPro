using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.RepresentativeDtos;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface ICuBranchService
    {
        Task<SelectList> SelectList_BranchesAsync(long sellerId);
        Task<SelectList> SelectList_BranchesAsync(BranchFilterDto filter);

        // شعبه های ثادرکننده غیرشرکتی
        Task<SelectList> SelectList_IssuerBranchesAsync(long sellerId);
        Task<SelectList> SelectList_DestributerAsync(long sellerId);
        Task<SelectList> SelectList_RepresentativeBranchesAsync(Guid id);
        Task<List<BranchDto>> GetBranchesAsync(long sellerId);
        Task<BranchDto> FindBranchByIdAsync(Guid id);
        Task<clsResult> AddBranchAsync(BranchDto dto);
        Task<clsResult> UpdateBranchAsync(BranchDto dto);
        Task<clsResult> DeleteBranchAsync(Guid id);
        Task<int?> FindRoutByCityAsync(int OriginCity, int DestinationCity, long SellerId);
        Task<List<BranchDto>> GetDistributersByDestinationSityAsync(int RouteId, long sellerId);
    }
}
