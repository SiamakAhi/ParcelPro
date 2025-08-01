namespace ParcelPro.Areas.Courier.Dto
{
    public class BillOfLadingFilterDto
    {
        public long SellerId { get; set; }
        public long? SenderId { get; set; }
        public long? ReciverId { get; set; }
        public string BiilOdLadingNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public Guid? OriginBranchId { get; set; }
        public Guid? DestinationBranchId { get; set; }
        public Guid? Distributer { get; set; }
        public int? RoutId { get; set; }
        public int? OriginCityId { get; set; }
        public int? BranchCityId { get; set; }
        public int? DestinationCityId { get; set; }
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
        public int? Partner { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public bool branchIsOwner { get; set; }
        public bool IsBranchManager { get; set; }
        public bool ShowCancelation { get; set; } = false;

    }
}
