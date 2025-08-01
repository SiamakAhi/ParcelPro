using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Client.ClientInterfacses;
using ParcelPro.Areas.Client.Dto;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.ContractDto;
using ParcelPro.Models;

namespace ParcelPro.Areas.Client.ClientServices
{
    public class ClientPanelService : IClientPanelService
    {
        private protected AppDbContext _db;
        private protected IBillofladingService _bil;

        public ClientPanelService(AppDbContext appDbContext, IBillofladingService billofladingService)
        {
            _db = appDbContext;
            _bil = billofladingService;
        }

        public async Task<SaleContractUserDto> GetClientUserInfoAsync(string userId)
        {
            var userifo = await _db.Cu_SaleContractUsers.AsNoTracking()
                .Include(n => n.Contract).ThenInclude(n => n.ContractParty)
                .Where(n => n.userId == userId)
                .Select(n => new SaleContractUserDto
                {
                    Id = n.Id,
                    ContractId = n.ContractId,
                    ContractTitle = n.Contract.Title,
                    PartyName = n.Contract.ContractParty.Name,
                    PartyId = n.Contract.PartyId,
                    Name = n.Name,
                    SellerId = n.SellerId,
                    userId = userId,

                }).FirstOrDefaultAsync();

            return userifo;
        }

        public async Task<SelectList> SelectList_ClientRoute(long partyId)
        {
            List<int> routeId = await _db.Cu_BillOfLadings.Where(n => n.SenderId == partyId).Select(n => n.RouteId).Distinct()
                .ToListAsync();
            var routs = await _db.Cu_Routes.Where(n => routeId.Contains(n.RouteId)).Select(n => new { id = n.RouteId, name = n.RouteName }).ToListAsync();

            return new SelectList(routs, "id", "name");

        }

        public async Task<SelectList> SelectList_ClientCustomers(long partyId)
        {
            List<long> cids = await _db.Cu_BillOfLadings.Where(n => n.SenderId == partyId).Select(n => n.ReceiverId).Distinct()
                .ToListAsync();

            var ClientCustomers = await _db.parties.Where(n => cids.Contains(n.Id
                )).Select(n => new { id = n.Id, name = n.Name }).ToListAsync();

            return new SelectList(ClientCustomers, "id", "name");

        }
        public async Task<SelectList> SelectList_ClientUsedProvincesAsync(long partyId)
        {
            var routs = await _db.Cu_FinancialTransactions.AsNoTracking().Include(n => n.BillOfLading)
                 .Where(n => n.AccountPartyId == partyId).Select(n => n.BillOfLading.RouteId).ToListAsync<int>();

            var originProvinces = await _db.Cu_Routes.AsNoTracking().Include(n => n.OriginCity)
                 .Where(n => routs.Contains(n.RouteId)).Select(n => n.OriginCity.ProvinceId).ToListAsync<int>();

            var DestinationProvinces = await _db.Cu_Routes.AsNoTracking().Include(n => n.DestinationCity)
                 .Where(n => routs.Contains(n.RouteId)).Select(n => n.DestinationCity.ProvinceId).ToListAsync<int>();

            var AllPerovince = _db.Geo_Provinces.AsNoTracking().Where(n => originProvinces.Contains(n.Id) || DestinationProvinces.Contains(n.Id)).Select(n => n.Id).Distinct().ToList<int>();

            var Provinces = await _db.Geo_Provinces.AsNoTracking().Where(n => AllPerovince.Contains(n.Id))
                .Select(n => new { Id = n.Id, Name = n.PersianName }).OrderBy(n => n.Name).ToListAsync();

            return new SelectList(Provinces, "Id", "Name");
        }

        public async Task<SelectList> SelectList_ClientUsedCitiesAsync(long partyId, int? provinceId = null)
        {
            var routsQuery = _db.Cu_FinancialTransactions.AsNoTracking().Include(n => n.BillOfLading)
                .ThenInclude(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.BillOfLading).ThenInclude(n => n.Route).ThenInclude(n => n.DestinationCity)
                 .Where(n => n.AccountPartyId == partyId).AsQueryable();
            if (provinceId != null)
                routsQuery = routsQuery.Where(n => n.BillOfLading.Route.OriginCity.ProvinceId == provinceId || n.BillOfLading.Route.DestinationCity.ProvinceId == provinceId);

            var routs = await routsQuery.Select(n => n.BillOfLading.RouteId).ToListAsync<int>();


            var originCities = await _db.Cu_Routes.AsNoTracking()
                 .Where(n => routs.Contains(n.RouteId)).Select(n => n.OriginCityId).ToListAsync<int>();

            var DestinationCities = await _db.Cu_Routes.AsNoTracking()
                 .Where(n => routs.Contains(n.RouteId)).Select(n => n.DestinationCityId).ToListAsync<int>();

            var AllCities = _db.Geo_Cities.AsNoTracking().Where(n => originCities.Contains(n.Id) || DestinationCities.Contains(n.Id)).Select(n => n.Id).Distinct().ToList<int>();

            var Cities = await _db.Geo_Cities.AsNoTracking().Where(n => AllCities.Contains(n.Id))
                .Select(n => new { Id = n.Id, Name = n.PersianName }).OrderBy(n => n.Name).ToListAsync();

            return new SelectList(Cities, "Id", "Name");

        }

        public async Task<List<AddressCityDto>> GetCitiesAsync(long partyId, int? provinceId = null)
        {
            var routsQuery = _db.Cu_FinancialTransactions.AsNoTracking().Include(n => n.BillOfLading)
                            .ThenInclude(n => n.Route).ThenInclude(n => n.OriginCity)
                            .Include(n => n.BillOfLading).ThenInclude(n => n.Route).ThenInclude(n => n.DestinationCity)
                             .Where(n => n.AccountPartyId == partyId).AsQueryable();
            if (provinceId != null)
                routsQuery = routsQuery.Where(n => n.BillOfLading.Route.OriginCity.ProvinceId == provinceId || n.BillOfLading.Route.DestinationCity.ProvinceId == provinceId);

            var routs = await routsQuery.Select(n => n.BillOfLading.RouteId).ToListAsync<int>();

            //var originCities = await _db.Cu_Routes.AsNoTracking()
            //     .Where(n => routs.Contains(n.RouteId)).Select(n => n.OriginCityId).ToListAsync<int>();

            var DestinationCities = await _db.Cu_Routes.AsNoTracking()
                 .Where(n => routs.Contains(n.RouteId)).Select(n => n.DestinationCityId).ToListAsync<int>();

            var AllCities = _db.Geo_Cities.AsNoTracking().Where(n => DestinationCities.Contains(n.Id)).Select(n => n.Id).Distinct().ToList<int>();

            var Cities = await _db.Geo_Cities.AsNoTracking().Where(n => AllCities.Contains(n.Id))
                .Select(n => new AddressCityDto
                {
                    CityId = n.Id,
                    NameFa = n.PersianName,
                }).ToListAsync();

            return Cities;

        }


        public IQueryable<ViewBillOfLadings> GetClientWaybillsAsQuery(WaybillFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.DistributerBranch)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .Where(n => !n.IsDeleted && n.SenderId == filter.SenderId && n.BillOfLadingStatusId > 2)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber))
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));

            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                DateTime date = filter.strFromDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date >= date.Date);
            }

            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                DateTime date = filter.strUntilDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date <= date.Date);
            }

            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.RouteId == filter.RoutId);

            if (filter.ReciverId.HasValue)
                query = query.Where(n => n.ReceiverId == filter.ReciverId.Value);

            if (!filter.ShowCancelation)
            {
                query = query.Where(n => n.BillOfLadingStatusId != 15 && n.BillOfLadingStatusId != 16);
            }
            if (filter.DestinationProvinceId.HasValue)
                query = query.Where(n => n.Route.DestinationCity.ProvinceId == filter.DestinationProvinceId.Value);

            if (filter.DestinationCityId.HasValue)
                query = query.Where(n => n.Route.DestinationCityId == filter.DestinationCityId.Value);

            if (filter.SimpleStatus.HasValue)
            {
                if (filter.SimpleStatus == 1)
                    query = query.Where(n => n.BillOfLadingStatusId == 11);
                else if (filter.SimpleStatus == 2)
                    query = query.Where(n => n.BillOfLadingStatusId < 11 && n.BillOfLadingStatusId > 2);
                else if (filter.SimpleStatus == 3)
                    query = query.Where(n => n.BillOfLadingStatusId == 15);
            }

            var result = query.Select(n => new ViewBillOfLadings
            {
                Id = n.Id,
                SellerId = n.SellerId,

                WaybillNumber = n.WaybillNumber,
                IssuanceDate = n.IssuanceDate,
                IssuanceTime = n.IssuanceTime,
                SenderId = n.SenderId,
                ReceiverId = n.ReceiverId,
                ReceiverName = n.Receiver.Name,
                ReceiverAddress = n.ReceiverAddress,
                ReceiverPhone = string.IsNullOrEmpty(n.ReceiverPhone) ? n.Receiver.MobilePhone : n.ReceiverPhone,
                Description = n.Description,
                RouteName = n.Route.RouteName,
                ConsigmentCount = n.Consignments.Count,
                TotalWeight = n.Consignments.Sum(x => x.Weight),
                BillOfLadingStatusId = n.BillOfLadingStatusId,
                LastStatusDescription = n.BillOfLadingStatus.Name,
                TotalCost = n.BillCosts.Sum(s => s.Amount),
                TotalDiscount = n.Consignments.Sum(s => s.Discount),
                CreatedBy = n.CreatedBy,
                UpdatedBy = n.UpdatedBy,
                UpdatedDate = n.UpdatedDate,
                IsDeleted = n.IsDeleted,

                tg_DeliveryDate = n.tg_DeliveryDate,
                tg_Description = n.tg_Description,
                tg_CourierManUserName = !string.IsNullOrEmpty(n.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).Family : "",
                DistributerBranchId = n.DistributerBranchId,

            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }
    }
}
