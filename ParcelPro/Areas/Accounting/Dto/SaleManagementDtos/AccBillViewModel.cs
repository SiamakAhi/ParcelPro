namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class AccBillViewModel
    {
        public Guid Id { get; set; }
        public Guid BillId { get; set; }
        public DateTime Date { get; set; }
        public string PerianDate { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; } = "";
        public long SenderId { get; set; }
        public string SenderName { get; set; } = "";

        public long ReciverId { get; set; }
        public string ReciverName { get; set; } = "";

        public string Number { get; set; } = "";
        public string? IssuerUserName { get; set; }
        public string? IssuerBranch { get; set; }
        public Guid? IssuerBranchId { get; set; }
        public string? DestributerBranch { get; set; }
        public Guid? DestributerBranchId { get; set; }
        public long BasePrice { get; set; } = 0;
        public long TotalCost { get; set; }
        public long TotalDiscount { get; set; } = 0;
        public long VatPrice { get; set; } = 0;

        public int SettelmentTypeId { get; set; }
        public string SettelmentTypeName { get; set; } = "";
        public long Payed { get; set; } = 0;
        public long TotalPriceBeforDiscount => BasePrice + TotalCost;
        public long TotalPriceAfterDiscount => statusId <= 11 ? BasePrice + TotalCost - TotalDiscount : 0;
        public long FinalPrice => TotalPriceAfterDiscount + VatPrice;
        public long Balance => statusId <= 11 ? FinalPrice - Payed : 0;
        public short? PartPartyType { get; set; }
        public short statusId { get; set; } = 1;

        public int DestinationCityId { get; set; }
        public string? DestinationCityName { get; set; }
        public int? OriginCityId { get; set; }
        public string? OriginCityName { get; set; }

    }
}
