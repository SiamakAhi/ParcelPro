namespace ParcelPro.Areas.Treasury.Dto
{
    public class PaymentListViewModel
    {
        public Guid TransactionId { get; set; }
        public Guid CouierTransactionId { get; set; }
        public Guid BillId { get; set; }
        public string BillNumber { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PayedDate { get; set; }
        public TimeSpan PayedTime { get; set; }
        public long Amount { get; set; }
        public string ReciptNumber { get; set; }
        public string? BankName { get; set; }
        public string? PosName { get; set; }
        public string? Cashier { get; set; }
        public string? GetwayTransactionNumber { get; set; }
    }
}
