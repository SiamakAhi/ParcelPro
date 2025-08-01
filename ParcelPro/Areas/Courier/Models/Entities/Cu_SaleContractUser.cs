using ParcelPro.Models.Identity;

namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_SaleContractUser
    {
        public int Id { get; set; }
        public long SellerId { get; set; }
        public int ContractId { get; set; }
        public virtual Cu_SaleContract Contract { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } = "MainUser";
        public DateTime CreateAt { get; set; }
        public string userId { get; set; }
        public AppIdentityUser UserData { get; set; }
    }
}
