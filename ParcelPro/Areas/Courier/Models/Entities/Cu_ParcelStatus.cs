namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_ParcelStatus
    {
        public int Id { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public bool ForReciver { get; set; }
        public bool ForSender { get; set; }

        public virtual ICollection<Cu_ParcelTracking> Parcels { get; set; }
    }
}
