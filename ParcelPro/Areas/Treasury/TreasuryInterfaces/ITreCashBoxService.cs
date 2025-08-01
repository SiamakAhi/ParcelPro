using ParcelPro.Areas.Treasury.Dto;

namespace ParcelPro.Areas.Treasury.TreasuryInterfaces
{
    public interface ITreCashBoxService
    {
        Task<clsResult> AddCashBoxAsync(TreCashBoxDto dto);
        Task<clsResult> UpdateCashBoxAsync(TreCashBoxDto dto);
        Task<List<TreCashBoxDto>> GetCashBoxesAsync(long sellerId);
        Task<TreCashBoxDto> GetCashBoxByIdAsync(Guid id);

        // ========================= POS Device Methods =========================
        Task<List<TreBankPosUcDto>> GetPosDevicesAsync(long sellerId);
        Task<TreBankPosUcDto> GetPosDeviceByIdAsync(int id);
        Task<clsResult> AddPosDeviceAsync(TreBankPosUcDto dto);
        Task<clsResult> UpdatePosDeviceAsync(TreBankPosUcDto dto);
        Task<clsResult> DeletePosDeviceAsync(int id);
    }
}
