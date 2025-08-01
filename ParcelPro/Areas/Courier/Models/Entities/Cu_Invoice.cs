namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_Invoice
    {
        public long Id { get; set; }
        public long SellerId { get; set; }
        public long PartyId { get; set; }
        public long InvoiceDate { get; set; }
        public string? Description { get; set; }

        public long Amount { get; set; } = 0;
        public long Payed { get; set; } = 0;
        public bool IsSetteled { get; set; } = false;

        public virtual ICollection<Cu_BillOfLading> Items { get; set; }
        public virtual ICollection<Cu_DistributionService>? PrtnerServices { get; set; }
    }
}
