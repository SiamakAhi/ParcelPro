namespace ParcelPro.Areas.Courier.Dto.FinancialDtos
{
    public class ViewModelMomeyTransaction
    {
        public TransactionFilterDto filter { get; set; } = new TransactionFilterDto();
        public List<Sale_MoneyTransactionDto> Transactions { get; set; }

    }
}
