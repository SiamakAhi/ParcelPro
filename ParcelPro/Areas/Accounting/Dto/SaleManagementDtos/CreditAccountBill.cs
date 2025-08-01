namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class CreditAccountBill
    {
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public int QtyBill { get; set; }
        public long Bed { get; set; } = 0;
        public long Bes { get; set; } = 0;
        public long Balance => Bed - Bes;
    }
}
