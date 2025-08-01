using ParcelPro.ViewModels.CommercialViewModel;

namespace ParcelPro.Areas.CustomerArea.Dto
{
    public class UpdatePermissionDto
    {
        public userSettingDto? UserSetting { get; set; }
        public List<UserSellerDto>? UserSellers { get; set; }
        public UserSellerDto? UserSeller { get; set; }
    }
}
