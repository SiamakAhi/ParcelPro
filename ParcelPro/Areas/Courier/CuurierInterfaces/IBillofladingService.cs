using Microsoft.AspNetCore.Mvc.Rendering;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.DitributionDto;
using ParcelPro.Areas.Courier.Dto.FinancialDtos;
using ParcelPro.Areas.Courier.Dto.RepresentativeDtos;
using ParcelPro.Areas.Treasury.Dto;

namespace ParcelPro.Areas.Courier.CuurierInterfaces
{
    public interface IBillofladingService
    {
        Task<SelectList> SelectList_IssuersUsersAsync(long sellerId);
        Task<SelectList> SelectList_BranchPersenAsync(Guid branchId);
        Task<string> GenerateBillNumberAsync(long sellerId, string branchCode);
        Task<clsResult> CreateNewBillOfLadingAsync(BillOfLadingDto_Header dto);
        Task<BillOfLadingDto> GetBillOfLadingDtoAsync(Guid id);
        Task<BillOfLadingDto> FindBillOfLadingDtoByNumberAsync(string wayBillnumber);
        Task<BillDataViewModel> GetBillDataAsync(Guid id);

        IQueryable<ViewBillOfLadings> GetBillsAsQuery(BillOfLadingFilterDto filter);
        Task<Guid> getBillIdByNumberAsync(string BillNumber);
        Task<BillCashPayDto> GetBillCashPayDtoAsync(Guid id);
        IQueryable<ViewBillOfLadings> GetInputBillsAsQuery(BillOfLadingFilterDto filter);
        IQueryable<ViewBillOfLadings> GetSimpleBillsAsQuery(BillOfLadingFilterDto filter);
        Task<clsResult> BillCancelationAsync(BillDataViewModel bill);
        //----------------------------------------------------------------------------------
        IQueryable<ViewBillOfLadings> GetIssuindBillsAsQuery(BillOfLadingFilterDto filter);
        IQueryable<ViewBillOfLadings> GetPendingDistributionAsQuery(BillOfLadingFilterDto filter);
        IQueryable<ViewBillOfLadings> GetNoSetteledBranchBillsAsQuery(BillOfLadingFilterDto filter, int destinationCityId);
        IQueryable<ViewBillOfLadings> GetWaybillsAsQuery(BillOfLadingFilterDto filter);
        Task<List<WaybillLabelDto>> WaybillLabelsAsync(Guid id);
        //========================================================
        Task<AddParcelHeaderInfo> GetParcelHeaderInfoAsync(Guid id);
        Task<clsResult> AddNewParcelAsync(ConsigmentDto dto);
        Task<clsResult> SaveParcelDeliveryAsync(ParcelDeliveryDto dto);
        Task<clsResult> DeleteBillAsync(BillDataViewModel bill);
        Task<clsResult> DeleteParcelAsync(Guid ParcelId, short? StatusId = null);
        Task<clsResult> SendPaymentLinkAsync(Guid id, string trackingNumber, string reciver);
        Task<clsResult> ChangeSenderOrReciverAsync(ChangeSenderOrReciverDto dto);
        Task<ChangeSenderOrReciverDto> GetSenderOrResiverData(Guid billId, bool isSender);
        Task<ConsigmentDto> GetParcelByIdAsync(Guid ParcelId);
        Task<clsResult> UpdateParcelAsync(ConsigmentDto dto);
        Task<WaybillDitributerUpdateDto> GetWaybillDistributerByIdAsync(Guid billId);
        Task<clsResult> SetWaybillDistributerAsync(WaybillDitributerUpdateDto dto);
        Task<WayBillsStatusCheckDto> getDashboardDataAsync(BillOfLadingFilterDto filter, int destinationCityId);
        Task<clsResult> ChangeSettelmentAsync(ChangeSettelmentDto dto);
        Task<clsResult> ChangeDestributerBulkAsync(ChangeDestribiuterDto dto);

        //=============================================================
        //=============================================================
        //=============================================================
        Task<clsResult> CreateDistributionBillAsync(AddDistriutionDto dto);

    }
}
