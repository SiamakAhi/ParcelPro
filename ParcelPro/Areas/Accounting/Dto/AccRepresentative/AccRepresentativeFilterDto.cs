namespace ParcelPro.Areas.Accounting.Dto.AccRepresentative
{
    public class AccRepresentativeFilterDto
    {
        public long SellerId { get; set; }
        public Guid? Id { get; set; } = null;
        public long? PersonId { get; set; }
        public int? CityId { get; set; }
        public string Name { get; set; }

        public int PageSize { get; set; } = 35;
        public int CurrentPage { get; set; } = 1;

    }
}
