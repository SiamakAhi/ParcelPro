namespace ParcelPro.Areas.Accounting.Dto
{
    public class EndOfPeriodDto
    {
        public EndOfPeriodSettings? AccountsSetting { get; set; }
        public List<DocArticleDto>? Articles { get; set; }
    }
}
