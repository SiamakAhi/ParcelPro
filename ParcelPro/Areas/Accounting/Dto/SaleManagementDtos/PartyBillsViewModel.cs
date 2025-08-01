using ParcelPro.ViewModels;

namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class PartyBillsViewModel
    {
        public PartyBillsFilterDto filter { get; set; } = new PartyBillsFilterDto();
        public PersonDto? PartyInfo { get; set; }
        public List<AccBillViewModel> Bills { get; set; } = new List<AccBillViewModel>();

    }
}
