using ParcelPro.Areas.Courier.Dto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface ITrachkingService
    {
        Task<bool> SetParcelTrackingAsync(ConsigmentDto parcel, int statusId, string description, bool showCustomer, bool showOp);
        Task<bool> SetBillOfLadingTrackingAsync(Guid Id, string number, string userId, int statusId, string description, bool showCustomer, bool showOp);
        Task<List<TrackingDto>> TrackingAsync(Guid Id, bool showOperation = true, bool showcustomer = true);
        Task<List<TrackingDto>> TrackingAsync(string BillOfLadingNumber);
        Task<bool> SetStatusAsync(Guid billofladingId, short statusId);


    }
}
