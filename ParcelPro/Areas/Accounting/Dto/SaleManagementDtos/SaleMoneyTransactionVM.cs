namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class SaleMoneyTransactionVM
    {
        public SaleMoneyTransactionFilterDto filter { get; set; } = new SaleMoneyTransactionFilterDto();
        public Pagination<SaleMoneyTransaction> Transactions { get; set; }
    }
}
