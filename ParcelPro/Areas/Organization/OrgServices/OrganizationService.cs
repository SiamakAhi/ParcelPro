using ParcelPro.Areas.Organization.OrgInterfaces;
using ParcelPro.Models;

namespace ParcelPro.Areas.Organization.OrgServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext _db;
        public OrganizationService(AppDbContext db)
        {
            _db = db;
        }
    }
}
