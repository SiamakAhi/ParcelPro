using ParcelPro.Areas.Courier.Dto.BranchUserDto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface IBranchUserService
    {
        Task<List<VmBranchUser>> GetBranchesUserAsync(BranchUserFilterDto filter);
        Task<VmBranchUser> GetBUserAsync(string userId);
        Task<VmBranchUser?> GetBUserByUsernameAsync(string userName);
    }
}
