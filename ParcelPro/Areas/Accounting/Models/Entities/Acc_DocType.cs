using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Accounting.Models.Entities
{
    public class Acc_DocType
    {
        [Key]
        public short Id { get; set; }
        public string DocTypeName { get; set; }
    }
}
