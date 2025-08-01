namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class SaleManagementViewModel
    {
        public AccSalesFilterDto filter { get; set; } = new AccSalesFilterDto();
        public List<AccBillViewModel> Bills { get; set; }
        public Pagination<AccBillViewModel> BillsPagin { get; set; }
        public List<CreditAccountBill>? CreditSaleCurrentDebt { get; set; }

    }
}
