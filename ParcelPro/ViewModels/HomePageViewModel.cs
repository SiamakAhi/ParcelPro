using ParcelPro.Areas.Accounting.Dto;
using ParcelPro.Areas.AvaRasta.ViewModels;
using ParcelPro.Areas.DataTransfer.Dto;
using ParcelPro.ViewModels.CommercialViewModel;

namespace ParcelPro.ViewModels
{
    public class HomePageViewModel
    {
        public VmCustomer? CustomerInfo { get; set; }
        public VmSellerDashboard? SellerDashboardData { get; set; }
        public DocDto_AddNew Doc { get; set; }
        public VmBillOfLandingMonitor? BillsMonitor { get; set; }
        public DocFilterDto filter { get; set; }
        public DocumentsInfo? DocumentsInfo { get; set; }
        public VmTafsilReport? TafsilReport { get; set; }
    }
}
