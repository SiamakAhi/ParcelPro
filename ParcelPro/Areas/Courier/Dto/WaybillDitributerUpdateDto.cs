namespace ParcelPro.Areas.Courier.Dto
{
    public class WaybillDitributerUpdateDto
    {
        public Guid Id { get; set; }
        public Guid? DistributerId { get; set; }
        public string? WaybillNumber { get; set; }
    }
}
