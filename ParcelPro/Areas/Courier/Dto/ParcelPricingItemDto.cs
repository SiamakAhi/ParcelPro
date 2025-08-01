namespace ParcelPro.Areas.Courier.Dto
{
    public class ParcelPricingItemDto
    {
        // -- Header Data
        public long SellerId { get; set; }
        public int ServiceId { get; set; }
        public int RouteId { get; set; }

        // -- Parcel Data
        public short? NatureId { get; set; }
        public double? Weight { get; set; }

    }
}
