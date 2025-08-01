namespace ParcelPro.Areas.Courier.Dto.FinancialDtos
{
    public class FinancialTransactionDto
    {

        public Guid Id { get; set; }
        public short OperationId { get; set; }
        public long SellerId { get; set; }
        public Guid? BillOfLadingId { get; set; }

        public long AccountPartyId { get; set; }
        public string? PartyName { get; set; }
        public string? SenderName { get; set; }
        public string? OriginCity { get; set; }
        public string? Destination { get; set; }
        public string? BillNumber { get; set; }
        public int SettlementTypeId { get; set; }
        public bool IsSetteled { get; set; }

        // تاریخ و زمان می‌تواند به صورت پیش‌فرض مقداردهی شود یا از کاربر دریافت گردد
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public TimeSpan TransactionTime { get; set; } = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        public string? Description { get; set; }
        public Guid? BranchId { get; set; }
        public long Amount { get; set; }
        public long Bed { get; set; } = 0;
        public long Bes { get; set; } = 0;
        public string UserId { get; set; }

        public bool BranchIsHub { get; set; } = false;
    }

}
