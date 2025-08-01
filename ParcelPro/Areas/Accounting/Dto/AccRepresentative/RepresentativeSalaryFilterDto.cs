namespace ParcelPro.Areas.Accounting.Dto.AccRepresentative
{
    public class RepresentativeSalaryFilterDto
    {
        public Guid? BranchId { get; set; }
        public long SellerId { get; set; }
        public string StartDate { get; set; }
        public string UntilDate { get; set; }
        public bool JustDelivered { get; set; } = false;

        public bool FromBody { get; set; } = false;

    }
}
