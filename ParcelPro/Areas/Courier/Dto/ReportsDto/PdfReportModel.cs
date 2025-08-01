namespace ParcelPro.Areas.Courier.Dto.ReportsDto
{
    // Models/PdfReportModel.cs
    public class PdfReportModel
    {
        public string Title { get; set; }
        public List<BillItem> Bills { get; set; }
        public string ExportDate { get; set; }

        public class BillItem
        {
            public string WaybillNumber { get; set; }
            public string IssuanceDate { get; set; }
            public string RouteName { get; set; }
            public string SenderName { get; set; }
            public string ReceiverName { get; set; }
            public int ConsigmentCount { get; set; }
            public decimal TotalWeight { get; set; }
            public decimal BillPrice { get; set; }
            public string PaymentStatus { get; set; }
            public string ReceiverAddress { get; set; }
        }
    }
}
