namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class SaleMoneyTransaction
    {
        public Guid? BillId { get; set; }
        public Guid? BranchId { get; set; }
        public string? WaybillNumber { get; set; }
        public long PartyId { get; set; }
        public string? PartyName { get; set; }
        public DateTime TransactionDate { get; set; }
        public TimeSpan TransactionTime { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public long Amount { get; set; } = 0;
        public string? TrackingNumber { get; set; }
        public int SettelmentTypeId { get; set; }
        public string? BillIssuerName { get; set; }
        public string? Cashier { get; set; }
        public int? BankAccountId { get; set; }
        public string? BankAccountName { get; set; }
        public string? POSName { get; set; }
        public int? POSId { get; set; }
        public string? Desciption { get; set; }

    }
}
