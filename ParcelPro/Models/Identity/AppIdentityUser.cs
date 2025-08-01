using Microsoft.AspNetCore.Identity;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Areas.Treasury.Models.Entities;
using ParcelPro.Models.Commercial;

namespace ParcelPro.Models.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        // public string Id { get; set; }
        public string FName { get; set; }
        public string Family { get; set; }
        public short Gender { get; set; }
        public string Mobile { get; set; }
        public string? Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime LastVisitDate { get; set; }
        public DateTime RegistrDate { get; set; }
        public bool IsActive { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual List<AppRole>? Roles { get; set; }

        public long? PersonId { get; set; }
        public virtual Party? Person { get; set; }

        public short DepartmentCode { get; set; } = 0;
        public virtual Cu_BranchUser? BranchUser { get; set; }

        public virtual ICollection<Cu_ParcelTracking> Parcels { get; set; }
        public virtual ICollection<Cu_FinancialTransaction> CuTransactions { get; set; } = new List<Cu_FinancialTransaction>();
        public virtual ICollection<TreTransaction>? TreTransactions { get; set; }

        public virtual Cu_SaleContractUser? ClientUserData { get; set; }

    }
}
