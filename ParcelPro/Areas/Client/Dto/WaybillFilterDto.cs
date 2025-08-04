namespace ParcelPro.Areas.Client.Dto
{
    public class WaybillFilterDto
    {
        public long SellerId { get; set; }
        public long? SenderId { get; set; }
        public long? ReciverId { get; set; }
        public string BiilOdLadingNumber { get; set; }
        public int? RoutId { get; set; }
        public int? OriginCityId { get; set; }
        public int? BranchCityId { get; set; }
        public int? DestinationCityId { get; set; }
        public int? DestinationProvinceId { get; set; }
        public string? IssuerUserName { get; set; }
        public string? strFromDate { get; set; }
        public string? strUntilDate { get; set; }
        public string? strWayBillDate { get; set; }
        public short[]? BillStatus { get; set; }
        public short? SettelmentType { get; set; }
        public short? PaymentStatus { get; set; }
        public bool IsFromBody { get; set; } = false;
        public short Sectiontype { get; set; } = 1;
        public short personSearchtype { get; set; } = 1;

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool ShowCancelation { get; set; } = false;
        public string Issuer { get; set; }

        public short? SimpleStatus { get; set; } = null;
        public string? CustomerKeyword { get; set; } = null;
    }
}
