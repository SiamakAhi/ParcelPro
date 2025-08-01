using ParcelPro.Areas.AvaRasta.Dto;
using ParcelPro.Areas.AvaRasta.Models.Entities;
using ParcelPro.Areas.CustomerArea.Dto;

namespace ParcelPro.Areas.AvaRasta.ViewModels
{
    public class CustomerManagementViewModel
    {
        public VmCustomer Customer { get; set; }
        public List<VmCustomerUsers>? CustomerUsers { get; set; }
        public List<License> Licenses { get; set; }
        public AddLicenseDto? NewLicense { get; set; }
    }

}

