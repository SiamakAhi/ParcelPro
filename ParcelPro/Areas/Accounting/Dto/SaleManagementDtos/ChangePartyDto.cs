namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class ChangePartyDto
    {
        public Guid BillId { get; set; }
        public long PartyId { get; set; }

        //For View
        public string? Billnumber { get; set; }
        public string? Sender { get; set; }
        public string? Reciver { get; set; }
        public string? CurrentPartyName { get; set; }
        public long? Price { get; set; }
    }
}
