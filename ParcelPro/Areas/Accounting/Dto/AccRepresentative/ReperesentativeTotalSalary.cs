namespace ParcelPro.Areas.Accounting.Dto.AccRepresentative
{
    public class ReperesentativeTotalSalary
    {
        public Guid Id { get; set; }
        public string BranchName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? UntilDate { get; set; }
        public decimal Percentage { get; set; }
        public int BillQty { get; set; } = 0;
        public long BillTotalAmount { get; set; } = 0;
        public long BranchShareAmount => (long)(BillTotalAmount * Percentage / 100);
    }
}
