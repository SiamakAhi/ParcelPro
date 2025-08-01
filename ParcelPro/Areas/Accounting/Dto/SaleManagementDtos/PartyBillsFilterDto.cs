namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class PartyBillsFilterDto
    {

        public long SellerId { get; set; }
        public long? PartyId { get; set; }
        public string strStartDate { get; set; }
        public string strEndDate { get; set; }
        public bool? IsPayed { get; set; }
    }
}
