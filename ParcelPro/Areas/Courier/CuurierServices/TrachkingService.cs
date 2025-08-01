using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Models;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.CuurierServices
{

    public class TrachkingService : ITrachkingService
    {
        private readonly AppDbContext _db;
        private readonly UserContextService _userContext;
        public TrachkingService(AppDbContext databaseContext, UserContextService UserContextService)
        {
            _db = databaseContext;
            _userContext = UserContextService;
        }
        public async Task<bool> SetParcelTrackingAsync(ConsigmentDto parcel, int statusId, string description, bool showCustomer, bool showOp)
        {
            Cu_ParcelTracking n = new Cu_ParcelTracking();
            n.ParcelId = parcel.Id;
            n.BillOfLadingId = parcel.BillOfLadingId;
            n.Description = description;
            n.UserId = parcel.UserId;
            n.BillOfLadingNumber = parcel.BillOfLadingNumber;
            n.StatusId = statusId;

            _db.Cu_ParcelTrackings.Add(n);
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SetBillOfLadingTrackingAsync(Guid Id, string number, string userId, int statusId, string description, bool showCustomer, bool showOp)
        {
            Cu_ParcelTracking n = new Cu_ParcelTracking();
            n.BillOfLadingId = Id;
            n.Description = description;
            n.UserId = userId;
            n.BillOfLadingNumber = number;
            n.StatusId = statusId;

            _db.Cu_ParcelTrackings.Add(n);
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<TrackingDto>> TrackingAsync(Guid Id, bool showOperation = true, bool showcustomer = true)
        {
            var trackingList = await _db.Cu_ParcelTrackings.AsNoTracking()
                  .Include(n => n.Status)
                  .Include(n => n.User)
                .Where(n => n.BillOfLadingId == Id)
                .Select(n => new TrackingDto
                {
                    Id = n.Id,
                    BillOfLadingId = n.BillOfLadingId,
                    ParcelId = n.ParcelId,
                    Description = n.Description,
                    Time = n.Time,
                    Date = n.Date,
                    ShowInCustomerTracking = n.ShowInCustomerTracking,
                    ShowInOperationsTracking = n.ShowInOperationsTracking,
                    UserFullName = n.User.FName + " " + n.User.Family,
                    UserId = n.UserId,
                    StatusId = n.StatusId,
                    StatusName = n.Status.Message,
                }).OrderBy(n => n.Date).ThenBy(n => n.Time).ToListAsync();

            return trackingList;

        }
        public async Task<List<TrackingDto>> TrackingAsync(Guid Id)
        {
            var trackingList = await _db.Cu_ParcelTrackings.AsNoTracking()
                  .Include(n => n.Status)
                  .Include(n => n.User)
                .Where(n => n.BillOfLadingId == Id)
                .Select(n => new TrackingDto
                {
                    Id = n.Id,
                    BillOfLadingId = n.BillOfLadingId,
                    ParcelId = n.ParcelId,
                    Description = n.Description,
                    Time = n.Time,
                    Date = n.Date,
                    ShowInCustomerTracking = n.ShowInCustomerTracking,
                    ShowInOperationsTracking = n.ShowInOperationsTracking,
                    UserFullName = n.User.FName + " " + n.User.Family,
                    UserId = n.UserId,
                    StatusId = n.StatusId,
                    StatusName = n.Status.Message,
                }).OrderBy(n => n.Date).ThenBy(n => n.Time).ToListAsync();

            return trackingList;

        }
        public async Task<List<TrackingDto>> TrackingAsync(string BillOfLadingNumber)
        {
            var trackingList = await _db.Cu_ParcelTrackings.AsNoTracking()
                .Include(n => n.Status)
                .Include(n => n.User)
                .Where(n => n.BillOfLadingNumber == BillOfLadingNumber)
                .Select(n => new TrackingDto
                {
                    Id = n.Id,
                    BillOfLadingId = n.BillOfLadingId,
                    ParcelId = n.ParcelId,
                    Description = n.Description,
                    Time = n.Time,
                    Date = n.Date,
                    ShowInCustomerTracking = n.ShowInCustomerTracking,
                    ShowInOperationsTracking = n.ShowInOperationsTracking,
                    UserFullName = n.User.FName + " " + n.User.Family,
                    UserId = n.UserId,
                    StatusId = n.StatusId,
                    StatusName = n.Status.Message,
                }).OrderBy(n => n.Date).ThenBy(n => n.Time).ToListAsync();

            return trackingList;

        }

        //========================
        public async Task<bool> SetStatusAsync(Guid billofladingId, short statusId)
        {
            var bill = await _db.Cu_BillOfLadings.SingleOrDefaultAsync(n => n.Id == billofladingId);
            if (bill == null)
                return false;
            bill.BillOfLadingStatusId = statusId;
            _db.Cu_BillOfLadings.Update(bill);
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
