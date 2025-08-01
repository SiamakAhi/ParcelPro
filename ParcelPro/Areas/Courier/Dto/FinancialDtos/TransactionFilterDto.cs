namespace ParcelPro.Areas.Courier.Dto.FinancialDtos
{
    public class TransactionFilterDto
    {
        public long SellerId { get; set; }
        public string? BillNumber { get; set; }
        public long? PartyId { get; set; }
        public string? strStartDate { get; set; }
        public string? strEndDate { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? BillId { get; set; }
        public int? BankAccountId { get; set; }
        public int? PosId { get; set; }

    }
}
