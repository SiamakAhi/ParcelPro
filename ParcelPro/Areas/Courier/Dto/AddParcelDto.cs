namespace ParcelPro.Areas.Courier.Dto
{
    public class AddParcelDto
    {
        public AddParcelHeaderInfo? BillOfLading { get; set; }
        public ConsigmentDto Consigmen { get; set; }
    }
}
