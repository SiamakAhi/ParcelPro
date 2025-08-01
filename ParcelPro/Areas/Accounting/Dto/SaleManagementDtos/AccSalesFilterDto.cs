namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class AccSalesFilterDto
    {
        public long SellerId { get; set; }
        public long? PartyId { get; set; }
        public long? PersonId { get; set; }
        public short personSearchtype { get; set; } = 1;


        public string BiilOdLadingNumber { get; set; }
        public Guid? OriginBranchId { get; set; }
        public Guid? DestinationBranchId { get; set; }
        public int? OriginCityId { get; set; }
        public int? DestinationCityId { get; set; }
        public string? IssuerUserName { get; set; }
        public string? CourierMan { get; set; }
        public string? strFromDate { get; set; }
        public string? strUntilDate { get; set; }
        public short[]? BillStatus { get; set; }
        public short? SettelmentType { get; set; }
        public short? PaymentStatus { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string Issuer { get; set; }



    }
}
