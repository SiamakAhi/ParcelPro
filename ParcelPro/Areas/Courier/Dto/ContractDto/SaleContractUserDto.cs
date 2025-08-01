namespace ParcelPro.Areas.Courier.Dto.ContractDto
{
    public class SaleContractUserDto
    {
        public int Id { get; set; }
        public long SellerId { get; set; }
        public int ContractId { get; set; }
        public string ContractTitle { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string Role { get; set; } = "MainUser";
        public DateTime CreateAt { get; set; }
        public string userId { get; set; }
        public string? userName { get; set; }
        public string? PartyName { get; set; }
        public long PartyId { get; set; }
    }
}
