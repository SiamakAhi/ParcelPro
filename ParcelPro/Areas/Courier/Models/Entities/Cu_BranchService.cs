namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_BranchService
    {
        public int Id { get; set; }
        public long SellerId { get; set; }
        public Guid BranchId { get; set; }
        public int ServiceId { get; set; }

        public virtual Cu_Service Cu_Service { get; set; }
        public virtual Cu_Branch Branch { get; set; }
    }
}
