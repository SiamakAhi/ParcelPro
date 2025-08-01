namespace ParcelPro.Areas.Courier.Dto.FinancialDtos
{
    public class ChangeSettelmentDto
    {
        public Guid BillId { get; set; }
        public short settelmentTypeId { get; set; }
        public long PartyId { get; set; } = 0;
    }
}
