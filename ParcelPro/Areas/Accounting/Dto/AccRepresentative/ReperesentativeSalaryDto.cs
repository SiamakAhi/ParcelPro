namespace ParcelPro.Areas.Accounting.Dto.AccRepresentative
{
    public class ReperesentativeSalaryDto
    {
        public Guid BillId { get; set; }
        public Guid? BranchId { get; set; }
        public string BranchName { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public long BillAmount { get; set; } = 0;
        public decimal SaleRate { get; set; } = 0;
        public decimal DestributionRate { get; set; } = 0;
        public long SaleShareAmount => (long)(BillAmount * SaleRate / 100);
        public long DestributionShareAmount => (long)(BillAmount * DestributionRate / 100);

    }
}
