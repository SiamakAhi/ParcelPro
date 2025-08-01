using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.Treasury.Models.Entities
{
    public class TreOperation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نوع عملیات")]
        public int OperationType { get; set; }

        [Required(ErrorMessage = "نام عملیات الزامی است")]
        [Display(Name = "نام عملیات")]
        public string OperationName { get; set; }

        [Display(Name = "تراکنش از طریق POS")]
        public bool IsPOSTransaction { get; set; } // برای نشان دادن اینکه تراکنش از طریق POS است
        public bool IsPay { get; set; }
        public bool UserAlowSelect { get; set; } = true;

        public virtual ICollection<TreTransaction> TreTransactions { get; set; } = new List<TreTransaction>();
    }

}

