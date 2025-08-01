namespace ParcelPro.Areas.Courier.Models.Entities
{
    public class Cu_FinancialTransactionOperation
    {
        public short Id { get; set; }
        public short Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Cu_FinancialTransaction> Transactions { get; set; }

    }
}
