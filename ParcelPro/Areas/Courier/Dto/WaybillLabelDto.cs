namespace ParcelPro.Areas.Courier.Dto
{
    public class WaybillLabelDto
    {
        public Guid BillId { get; set; }
        public string WaybillNimber { get; set; }
        public string OriginCity { get; set; }
        public string Destination { get; set; }
        public int TotalCountParcel { get; set; }
        public int ParcelNumber { get; set; }
        public string weight { get; set; }
        public string ReciverName { get; set; }
        public string ReciverTel { get; set; }
        public string ReciverAddress { get; set; }
    }
}
