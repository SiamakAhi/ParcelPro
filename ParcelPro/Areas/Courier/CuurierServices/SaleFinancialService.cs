using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Models;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.CuurierServices
{
    public class SaleFinancialService : ISaleFinancialService
    {
        private readonly AppDbContext _db;
        private readonly UserContextService _userContextService;

        public SaleFinancialService(AppDbContext db, UserContextService userContextService)
        {
            _db = db;
            _userContextService = userContextService;
        }
    }
}
