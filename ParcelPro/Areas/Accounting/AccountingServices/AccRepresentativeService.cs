using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Accounting.AccountingInterfaces;
using ParcelPro.Areas.Accounting.Dto.AccRepresentative;
using ParcelPro.Models;

namespace ParcelPro.Areas.Accounting.AccountingServices
{


    public class AccRepresentativeService : IAccRepresentativeService
    {
        private readonly AppDbContext _db;
        public AccRepresentativeService(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<List<AccRepresentativeInfoDto>> GetRepresentativesInfo(AccRepresentativeFilterDto filter)
        {
            var query = _db.Cu_Branch.AsNoTracking().Where(n => n.SellerId == filter.SellerId)
                .Include(n => n.BranchCity)
                .Include(n => n.BranchPerson).AsQueryable();

            if (filter.Id != null)
                query = query.Where(n => n.Id == filter.Id);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(n => n.BranchName.Contains(filter.Name));

            if (filter.CityId.HasValue)
                query = query.Where(n => n.CityId.Value == filter.CityId.Value);

            var result = await query.Select(n => new AccRepresentativeInfoDto
            {
                Id = n.Id,
                BranchName = n.BranchName,
                CityId = n.CityId.Value,
                CityName = n.BranchCity.PersianName,
                PartyId = n.PartyId,
                PartyName = n.BranchPerson.Name ?? "",
                IsActive = n.IsActive,
                IsOwnership = n.IsOwnership,
                IsBillOfLadingIssuer = n.IsBillOfLadingIssuer,
                IsDistShareFixed = n.IsDistShareFixed,
                IsExternalBLIssuer = n.IsExternalBLIssuer,
                IsIntercityFleet = n.IsIntercityFleet,
                DistShare = n.DistShare,
                BranchCode = n.BranchCode,
                BranchManagerPhoneNumber = n.BranchManagerPhoneNumber,
                Address = n.Address,
                AllowdDiscountRate = n.AllowdDiscountRate,
                BranchManager = n.BranchManager,
                OldBranchName = n.OldBranchName,
                IssueShare = n.IssueShare,

            }).ToListAsync();

            return result;
        }

        public async Task<List<ReperesentativeTotalSalary>> ReperesentativeTotalSalariesAsync(RepresentativeSalaryFilterDto filter)
        {
            var baseQuery = _db.Cu_BillOfLadings.AsNoTracking()
             .Include(n => n.DistributerBranch)
             .Where(n => n.SellerId == filter.SellerId && n.BillOfLadingStatusId > 1);

            if (filter.BranchId != null)
                baseQuery = baseQuery.Where(n => n.DistributerBranchId == filter.BranchId);

            if (!string.IsNullOrEmpty(filter.StartDate))
            {
                DateTime startDate = filter.StartDate.PersianToLatin();
                baseQuery = baseQuery.Where(n => n.IssuanceDate >= startDate);
            }

            if (!string.IsNullOrEmpty(filter.UntilDate))
            {
                DateTime untilDate = filter.UntilDate.PersianToLatin();
                baseQuery = baseQuery.Where(n => n.IssuanceDate <= untilDate);
            }

            if (filter.JustDelivered)
                baseQuery = baseQuery.Where(n => n.BillOfLadingStatusId == 11);

            // کوئری فلت از BillCosts مرتبط فقط با CostTypeId == 1
            var query = baseQuery
                .SelectMany(bill => bill.BillCosts
                    .Where(cost => cost.CostTypeId == 1)
                    .Select(cost => new
                    {
                        bill.DistributerBranchId,
                        bill.DistributerBranch.BranchName,
                        bill.IssuanceDate,
                        bill.BillOfLadingStatusId,
                        Amount = cost.Amount,
                        DistShare = bill.DistributerBranch.DistShare
                    })
                );

            // حالا گروپ‌بندی رو روی این داده فلت انجام میدیم
            var groupedData = await query
                .GroupBy(x => x.DistributerBranchId)
                .Select(g => new ReperesentativeTotalSalary
                {
                    Id = g.Key ?? Guid.NewGuid(),
                    BranchName = g.Select(x => x.BranchName).FirstOrDefault() ?? "",
                    StartDate = g.Min(x => x.IssuanceDate),
                    UntilDate = g.Max(x => x.IssuanceDate),
                    BillQty = g.Count(),
                    BillTotalAmount = g.Sum(x => x.Amount),
                    Percentage = g.Select(x => x.DistShare).FirstOrDefault() ?? 0
                })
                .OrderByDescending(x => x.BillQty)
                .ToListAsync();

            return groupedData;

        }

        public async Task<List<ReperesentativeSalaryDto>> ReperesentativeSalaryAsync(RepresentativeSalaryFilterDto filter)
        {
            var query = _db.Cu_BillOfLadings.AsNoTracking().Include(n => n.BillCosts).Include(n => n.DistributerBranch)
                .Where(n => n.SellerId == filter.SellerId && n.BillOfLadingStatusId > 1).AsQueryable();

            if (filter.BranchId != null)
                query = query.Where(n => n.DistributerBranchId == filter.BranchId);

            if (!string.IsNullOrEmpty(filter.StartDate))
            {
                DateTime date = filter.StartDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate >= date);
            }
            if (!string.IsNullOrEmpty(filter.UntilDate))
            {
                DateTime date = filter.UntilDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate <= date);
            }
            if (filter.JustDelivered)
                query = query.Where(n => n.BillOfLadingStatusId == 11);

            var data = await query.Select(n => new ReperesentativeSalaryDto
            {
                BillId = n.Id,
                BranchId = n.DistributerBranchId ?? null,
                BranchName = n.DistributerBranch.BranchName ?? "مشخص نشده",
                BillNumber = n.WaybillNumber,
                BillDate = n.IssuanceDate,
                BillAmount = n.BillOfLadingStatusId <= 11 ? n.BillCosts.Where(x => x.CostTypeId == 1).Sum(x => x.Amount) : 0,
                DestributionRate = n.DistributerBranch.DistShare ?? 0,

            }).ToListAsync();

            return data;
        }

    }
}
