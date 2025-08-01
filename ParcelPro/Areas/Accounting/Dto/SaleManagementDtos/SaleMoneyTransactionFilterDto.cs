namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class SaleMoneyTransactionFilterDto
    {
        public long SellerId { get; set; }
        public long? PartyId { get; set; }
        public string? BiilOdLadingNumber { get; set; }
        public Guid? OriginBranchId { get; set; }
        public Guid? BillId { get; set; }
        public string? IssuerUserName { get; set; }
        public string? CourierMan { get; set; }
        public string? strFromDate { get; set; } = DateTime.Now.AddDays(-2).LatinToPersian();
        public string? strUntilDate { get; set; } = DateTime.Now.LatinToPersian();
        public int? SettelmentType { get; set; }
        public int? PaymentMethod { get; set; }
        public string? Description { get; set; }
        public int? BankAccountId { get; set; }
        public int? PosId { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
