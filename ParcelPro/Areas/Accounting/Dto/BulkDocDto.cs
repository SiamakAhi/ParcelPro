using ParcelPro.Areas.Accounting.Models.Entities;

namespace ParcelPro.Areas.Accounting.Dto
{
    public class BulkDocDto
    {
        public List<Acc_Document> Headers { get; set; } = new List<Acc_Document>();
        public List<Acc_Article> Articles { get; set; } = new List<Acc_Article>();
    }
}
