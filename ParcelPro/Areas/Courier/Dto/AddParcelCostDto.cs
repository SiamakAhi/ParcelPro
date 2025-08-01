namespace ParcelPro.Areas.Courier.Dto
{
    public class AddParcelCostDto
    {
        public int CostId { get; set; }
        public string? Code { get; set; }
        public string CostName { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
        public short? ImpactType { get; set; }

    }
}
