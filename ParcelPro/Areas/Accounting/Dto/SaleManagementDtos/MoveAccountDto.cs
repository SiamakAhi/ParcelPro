namespace ParcelPro.Areas.Accounting.Dto.SaleManagementDtos
{
    public class MoveAccountDto
    {
        public long TargetAccount { get; set; }
        public List<long> OriginAccounts { get; set; }
    }
}
