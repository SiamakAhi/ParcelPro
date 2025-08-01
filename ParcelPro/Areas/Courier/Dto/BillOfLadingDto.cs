using ParcelPro.Areas.Courier.Dto.FinancialDtos;

namespace ParcelPro.Areas.Courier.Dto
{
    public class BillOfLadingDto
    {
        public BillOfLadingDto_Header bill { get; set; }
        public List<ConsigmentDto>? Consigments { get; set; }
        public List<TrackingDto>? Trackings { get; set; }
        public FinancialTransactionDto? Financials { get; set; }
    }
}

