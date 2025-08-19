using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto;
using ParcelPro.Areas.Courier.Dto.DitributionDto;
using ParcelPro.Areas.Courier.Dto.FinancialDtos;
using ParcelPro.Areas.Courier.Dto.RepresentativeDtos;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Areas.Treasury.Dto;
using ParcelPro.Interfaces;
using ParcelPro.Interfaces.Identity;
using ParcelPro.Models;
using ParcelPro.Services;
using System.Text.RegularExpressions;

namespace ParcelPro.Areas.Courier.CuurierServices
{
    public class BillofladingService : IBillofladingService
    {
        private readonly AppDbContext _db;
        private readonly UserContextService _userContext;
        private readonly ITrachkingService _tracking;
        private readonly ISMSService _smsService;
        private readonly SmsSenderPersiaFava _smsSender;
        private readonly IAppIdentityUserManager _userManager;

        public BillofladingService(AppDbContext databaseContext
            , UserContextService UserContextService,
              ITrachkingService tracking
            , ISMSService SmsService
            , SmsSenderPersiaFava smsSender
            , IAppIdentityUserManager appIdentityUserManager)
        {
            _db = databaseContext;
            _userContext = UserContextService;
            _tracking = tracking;
            _smsService = SmsService;
            _smsSender = smsSender;
            _userManager = appIdentityUserManager;
        }

        public async Task<SelectList> SelectList_IssuersUsersAsync(long sellerId)
        {
            var usersName = await _db.Cu_BillOfLadings.Where(n => n.SellerId == sellerId).Select(n => n.CreatedBy).Distinct().ToListAsync();
            var usersData = await _db.Users.Where(n => usersName.Contains(n.UserName))
                .Select(n => new { Id = n.UserName, Name = n.FName + " " + n.Family }).OrderBy(n => n.Name).ToListAsync();

            return new SelectList(usersData, "Id", "Name");
        }
        public async Task<SelectList> SelectList_BranchPersenAsync(Guid branchId)
        {
            var persens = await _db.parties.AsNoTracking().Where(n => n.BranchId == branchId)
                .Select(n => new { id = n.Id, name = n.Name }).OrderBy(n => n.name).ToListAsync();

            return new SelectList(persens, "id", "name");

        }
        public async Task<SelectList> SelectList_BranchUsedPersenAsync(Guid branchId)
        {
            var persensId = await _db.Cu_BillOfLadings.AsNoTracking().Where(n => n.OriginBranchId == branchId)
               .Select(n => new { senders = n.SenderId, reciversId = n.ReceiverId }).ToListAsync();
            List<long> all = new List<long>();
            all.AddRange(persensId.Select(n => n.senders).ToList());
            all.AddRange(persensId.Select(n => n.reciversId).ToList());
            all = all.Distinct().ToList();
            var persens = await _db.parties.Where(n => all.Contains(n.Id))
                .Select(n => new { id = n.Id, name = n.Name }).OrderBy(n => n.name).ToListAsync();

            return new SelectList(persens, "id", "name");

        }
        public async Task<string> GenerateBillNumberAsync(long sellerId, string branchCode)
        {
            // استفاده از تراکنش با سطح ایزولاسیون Serializable برای جلوگیری از مشکلات همزمانی
            using (var transaction = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
            {
                // دریافت ردیف شمارنده سراسری
                var globalCounter = await _db.GlobalBillCounters.Where(n => n.SellerId == sellerId).FirstOrDefaultAsync();

                if (globalCounter == null)
                {
                    // در صورت عدم وجود ردیف، ایجاد آن با مقدار اولیه 1
                    globalCounter = new GlobalBillCounter { LastNumber = 1, SellerId = sellerId };
                    _db.GlobalBillCounters.Add(globalCounter);
                }
                else
                {
                    // افزایش مقدار شمارنده
                    globalCounter.LastNumber++;
                }

                // ذخیره تغییرات در دیتابیس
                await _db.SaveChangesAsync();
                // تایید تراکنش
                await transaction.CommitAsync();

                // تولید شماره بارنامه به فرمت: کد شعبه + شمارنده 6 رقمی
                string sequenceNumber = globalCounter.LastNumber.ToString("D6");
                return branchCode + sequenceNumber;
            }
        }
        public async Task<clsResult> CreateNewBillOfLadingAsync(BillOfLadingDto_Header dto)
        {
            if (dto == null)
            {
                return new clsResult { Message = "اطلاعاتی جهت صدور دریافت نشد", ShowMessage = true, Success = false };
            }
            string billNumber = await GenerateBillNumberAsync(dto.SellerId, dto.OriginBranchCode);
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            Cu_BillOfLading bill = new Cu_BillOfLading();
            bill.Id = dto.Id;
            bill.SellerId = dto.SellerId;
            bill.WayBillType = bill.WayBillType;
            bill.WaybillNumber = billNumber;
            bill.IssuanceDate = DateTime.Now;
            bill.CustomerKeyword = dto.CustomerKeyword;

            bill.RouteId = dto.RouteId;
            bill.ServiceId = dto.ServiceId;
            bill.OriginBranchId = dto.OriginBranchId;
            bill.OriginHubId = dto.OriginHubId;

            bill.SenderId = dto.SenderId;
            bill.SenderAddress = dto.SenderAddress;
            bill.SenderPhone = dto.SenderPhone;
            bill.ReceiverId = dto.ReceiverId;
            bill.ReceiverAddress = dto.ReceiverAddress;
            bill.ReceiverPhone = dto.ReciverPhone;
            bill.Description = dto.Description;

            bill.BillOfLadingStatusId = dto.BillOfLadingStatusId;
            bill.LastStatusDescription = dto.LastStatusDescription;
            bill.CreatedBy = dto.CreatedBy;

            bill.DistributerBranchId = dto.DistributerRepresentativeId;
            if (dto.DistributerRepresentativeId == Guid.Empty)
                bill.DistributerBranchId = null;

            try
            {
                _db.Cu_BillOfLadings.Add(bill);
                await _db.SaveChangesAsync();

                // Set Tracking
                await _tracking.SetBillOfLadingTrackingAsync(dto.Id, billNumber, dto.userId, 1, "ایجاد هدر بارنامه توسط کاربر صدور", false, true);

                result.Success = true;
                result.Message = "بارنامه با موفقیت ایجاد شد";
            }
            catch (Exception x)
            {
                result.Message = "در عملیات صدور بارنامه خطایی رخ داده است";
            }
            return result;
        }
        public async Task<BillOfLadingDto> GetBillOfLadingDtoAsync_new(Guid id)
        {
            // 1. استفاده از پروجکشن دقیق به جای Include
            var billProjection = await _db.Cu_BillOfLadings
                .AsNoTracking()
                .AsSplitQuery() // جلوگیری از Cartesian explosion
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    // اطلاعات اصلی
                    Bill = b,

                    // اطلاعات مرتبط با Route
                    OriginCityName = b.Route.OriginCity.PersianName,
                    DestinationCityName = b.Route.DestinationCity.PersianName,

                    // اطلاعات Service
                    ServiceName = b.Service.ServiceName,
                    VatRate = b.Service.VatRate,

                    // اطلاعات Sender
                    SenderName = b.Sender.Name,
                    SenderNationalId = b.Sender.NationalId,
                    SenderMobile = b.Sender.MobilePhone,

                    // اطلاعات Receiver
                    ReceiverName = b.Receiver.Name,
                    ReceiverMobile = b.Receiver.MobilePhone,

                    // اطلاعات IssuingBranch
                    OriginBranchName = b.IssuingBranch.BranchName,

                    // اطلاعات DistributerBranch
                    DistributerBranchName = b.DistributerBranch != null ? b.DistributerBranch.BranchName : null,

                    // اطلاعات BusinessPartner
                    BusinessPartnerName = b.BusinessPartner != null ? b.BusinessPartner.Name : null,

                    // اطلاعات FinancialTransactions
                    MainFinancialTransaction = b.FinancialTransactions
                        .Where(ft => ft.OperationId == 1)
                        .Select(ft => new
                        {
                            ft.Id,
                            ft.AccountPartyId,
                            PartyName = ft.Party.Name,
                            ft.BillOfLading.WaybillNumber,
                            ft.SettlementTypeId,
                            ft.IsSettled,
                            ft.Amount,
                            ft.Bed,
                            ft.Bes
                        })
                        .FirstOrDefault(),

                    // محاسبات هزینه‌ها
                    TotalBillCosts = b.BillCosts.Sum(bc => bc.Amount),
                    TotalConsignmentDiscounts = b.Consignments.Sum(c => c.Discount),

                    // گروه‌بندی هزینه‌ها
                    GroupedCosts = b.BillCosts
                        .GroupBy(bc => bc.CostType.CostCode)
                        .Select(g => new
                        {
                            CostCode = g.Key,
                            Amount = g.Sum(bc => bc.Amount),
                            CostName = g.FirstOrDefault().CostType.Description
                        })
                        .ToList(),

                    // اطلاعات Consignments
                    Consignments = b.Consignments.Select(c => new
                    {
                        c.Id,
                        c.SellerId,
                        c.Code,
                        c.Weight,
                        c.Height,
                        c.Width,
                        c.Length,
                        c.Volume,
                        c.ContentDescription,
                        c.Value,
                        c.ServiceInformation,
                        c.IsDelivered,
                        c.RecipientName,
                        c.DeliveryDate,
                        c.ReceiverSignature,
                        c.CargoFare,
                        c.TotalCostPrice,
                        c.Discount,
                        c.VatRate,
                        c.VatPrice,
                        c.TotalPrice,
                        c.BillOfLadingId,
                        c.NatureTypeId,
                        NatureName = c.NatureType.Name,
                        c.CreatedAt,
                        c.CreatedAtTime,
                        c.CreatedBy,
                        Costs = c.BillCosts.Select(bc => new
                        {
                            bc.Amount,
                            bc.CostType.CostCode,
                            bc.CostTypeId,
                            CostName = bc.CostType.Description
                        }).ToList()
                    }).ToList(),

                    // اطلاعات کاربر (برای CreatedByFullName)
                    CreatedByUser = _db.Users
                        .Where(u => u.Id == b.CreatedBy)
                        .Select(u => new
                        {
                            u.FName,
                            u.Family
                        })
                        .FirstOrDefault(),

                    // اطلاعات پیک (برای tg_CourierManUserName)
                    CourierManUser = !string.IsNullOrEmpty(b.tg_CourierManUserName)
                        ? _db.Users
                            .Where(u => u.UserName == b.tg_CourierManUserName)
                            .Select(u => u.FName + " " + u.Family)
                            .FirstOrDefault()
                        : null
                })
                .SingleOrDefaultAsync();

            if (billProjection == null)
                throw new InvalidOperationException("بارنامه با شناسه مورد نظر یافت نشد.");

            var bill = billProjection.Bill;

            // 2. تبدیل اطلاعات
            var billHeader = new BillOfLadingDto_Header
            {
                Id = bill.Id,
                // ... سایر پراپرتی‌ها
                tg_CourierManUserName = billProjection.CourierManUser,
                CreatedByFullName = billProjection.CreatedByUser != null
                    ? $"{billProjection.CreatedByUser.FName} {billProjection.CreatedByUser.Family}"
                    : "",
                BillofLadingCosts = billProjection.GroupedCosts.Select(g => new AddParcelCostDto
                {
                    Amount = g.Amount,
                    CostName = g.CostName,
                    Code = g.CostCode
                }).ToList(),
                FinalPrice = billProjection.TotalBillCosts - billProjection.TotalConsignmentDiscounts,
                // ... سایر پراپرتی‌ها
            };

            // 3. تبدیل Consignments
            var consignments = billProjection.Consignments.Select(c => new ConsigmentDto
            {
                // ... مپینگ پراپرتی‌ها
                Costs = c.Costs.Select(cost => new AddParcelCostDto
                {
                    Amount = cost.Amount,
                    Code = cost.CostCode,
                    CostId = cost.CostTypeId,
                    CostName = cost.CostName
                }).ToList()
            }).ToList();

            // 4. تبدیل Financials
            var billSettel = billProjection.MainFinancialTransaction != null
                ? new FinancialTransactionDto
                {
                    Id = billProjection.MainFinancialTransaction.Id,
                    // ... سایر پراپرتی‌ها
                }
                : null;

            return new BillOfLadingDto
            {
                bill = billHeader,
                Consigments = consignments,
                Trackings = await _tracking.TrackingAsync(bill.Id, false, true),
                Financials = billSettel
            };
        }

        public async Task<BillOfLadingDto> GetBillOfLadingDtoAsync(Guid id)
        {
            var bill = await _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.Consignments).ThenInclude(n => n.NatureType)
                .Include(n => n.IssuingBranch)
                .Include(n => n.OriginHub)
                .Include(n => n.DestinationHub)
                .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .Include(n => n.Service)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.FinancialTransactions)
                .Include(n => n.DistributerBranch)
                .Include(n => n.BusinessPartner)
                .SingleOrDefaultAsync(n => n.Id == id);

            if (bill == null)
            {
                throw new InvalidOperationException("بارنامه با شناسه مورد نظر یافت نشد.");
            }

            // 2. تبدیل اطلاعات بارنامه به BillOfLadingDto_Header
            var billHeader = new BillOfLadingDto_Header();
            billHeader.Id = bill.Id;
            billHeader.SellerId = bill.SellerId;
            billHeader.WaybillNumber = bill.WaybillNumber;
            billHeader.ReferenceCode = bill.ReferenceCode;
            billHeader.CustomerKeyword = bill.CustomerKeyword;
            billHeader.IssuanceDate = bill.IssuanceDate;
            billHeader.RouteId = bill.RouteId;
            billHeader.OriginCity = bill.Route.OriginCity.PersianName;
            billHeader.DestinationCity = bill.Route.DestinationCity.PersianName;
            billHeader.ServiceId = bill.ServiceId;
            billHeader.ServiceName = bill.Service?.ServiceName;
            billHeader.OriginBranchId = bill.OriginBranchId;
            billHeader.OriginBranchName = bill.IssuingBranch?.BranchName;
            billHeader.SenderId = bill.SenderId;
            billHeader.SenderAddress = bill.SenderAddress;
            billHeader.SenderName = bill.Sender?.Name;
            billHeader.SenderNationalCode = bill.Sender?.NationalId;
            billHeader.SenderPhone = string.IsNullOrEmpty(bill.SenderPhone) ? bill.Sender.MobilePhone : bill.SenderPhone;
            billHeader.ReceiverId = bill.ReceiverId;
            billHeader.ReceiverAddress = bill.ReceiverAddress;
            billHeader.ReciverName = bill.Receiver?.Name;
            billHeader.ReciverPhone = string.IsNullOrEmpty(bill.ReceiverPhone) ? bill.Receiver.MobilePhone : bill.ReceiverPhone;
            billHeader.Description = bill.Description;
            billHeader.DeliveredCount = bill.DeliveredCount;

            if (bill.WayBillType == 2 && bill.FinancialTransactions.Where(f => f.OperationId == 1) == null)
            {
                billHeader.SettelmentType = 1;
                billHeader.Setteled = true;
            }
            else if (bill.FinancialTransactions != null && bill.FinancialTransactions.Any())
            {
                if (bill.FinancialTransactions != null)
                {
                    var settel = bill.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault();
                    if (settel == null)
                        billHeader.SettelmentType = null;
                    else
                        billHeader.SettelmentType = (short)settel.SettlementTypeId;
                }
                billHeader.Setteled = bill.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any();
            }

            billHeader.LastStatusDescription = bill.LastStatusDescription;
            billHeader.BillOfLadingStatusId = bill.BillOfLadingStatusId;
            billHeader.OriginHubId = bill.OriginHubId;
            billHeader.DestinationHubId = bill.DestinationHubId;
            billHeader.CreatedBy = bill.CreatedBy;
            billHeader.UpdatedDate = bill.UpdatedDate;
            billHeader.UpdatedBy = bill.UpdatedBy;
            billHeader.IsDeleted = bill.IsDeleted;
            billHeader.VatRate = bill.Service.VatRate;
            billHeader.tg_CourierManUserName = !string.IsNullOrEmpty(bill.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == bill.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == bill.tg_CourierManUserName).Family : "";
            billHeader.tg_DeliveryDate = bill.tg_DeliveryDate;
            billHeader.tg_Description = bill.tg_Description;
            billHeader.tg_Name = bill.tg_Name;
            billHeader.tg_NationalityCode = bill.tg_NationalityCode;
            billHeader.tg_Phone = bill.tg_Phone;
            billHeader.tg_SignatureData = bill.tg_SignatureData;
            billHeader.Delivered = bill.Delivered;
            billHeader.CreatedByFullName = _userManager.UserInfo(bill.CreatedBy).FirstName + " " + _userManager.UserInfo(bill.CreatedBy).LastName;
            billHeader.BillofLadingCosts = bill.BillCosts.GroupBy(n => n.CostType.CostCode).Select(n => new AddParcelCostDto
            {
                Amount = n.Sum(z => z.Amount),
                CostName = n.FirstOrDefault().CostType.Description,
                Code = n.FirstOrDefault().CostType.CostCode,
            }).ToList();
            billHeader.FinalPrice = (bill.BillCosts.Sum(a => a.Amount)) - (bill.Consignments.Sum(d => d.Discount));
            billHeader.DistributerRepresentativeId = bill.DistributerBranchId;
            billHeader.DistributerRepresentative = bill.DistributerBranch != null ? bill.DistributerBranch.BranchName : "";

            //Business Partner
            billHeader.BusinessPartnerId = bill.BusinessPartnerId;
            billHeader.BusinessPartnerName = bill.BusinessPartner != null ? bill.BusinessPartner.Name : "";

            // 3.  ConsigmentDto
            var consignments = bill.Consignments?.Select(c => new ConsigmentDto
            {
                Id = c.Id,
                SellerId = c.SellerId,
                Code = c.Code,
                Weight = c.Weight,
                Height = c.Height,
                Width = c.Width,
                Length = c.Length,
                Volume = c.Volume,
                ContentDescription = c.ContentDescription,
                Value = c.Value,
                ServiceInformation = c.ServiceInformation,
                IsDelivered = c.IsDelivered,
                RecipientName = c.RecipientName,
                DeliveryDate = c.DeliveryDate,
                ReceiverSignature = c.ReceiverSignature,
                CargoFare = c.CargoFare,
                TotalCostPrice = c.TotalCostPrice,
                Discount = c.Discount,
                VatRate = c.VatRate,
                VatPrice = c.VatPrice,
                TotalPrice = c.TotalPrice,
                BillOfLadingId = c.BillOfLadingId,
                NatureTypeId = c.NatureTypeId,
                NatureName = c.NatureType.Name,
                CreateAt = c.CreatedAt,
                CreateAtTime = c.CreatedAtTime,
                CreatedBy = c.CreatedBy,
                Costs = c.BillCosts.Select(z => new AddParcelCostDto
                {
                    Amount = z.Amount,
                    Code = z.CostType.CostCode,
                    CostId = z.CostTypeId,
                    CostName = z.CostType.Description,
                }).ToList(),

            }).ToList();

            var n = await _db.Cu_FinancialTransactions.Include(n => n.BillOfLading).Include(n => n.Party)
                .Where(n => n.BillOfLadingId == id && n.OperationId == 1).FirstOrDefaultAsync();

            var billSettel = new FinancialTransactionDto();
            if (n != null)
            {
                billSettel.Id = n.Id;
                billSettel.SellerId = n.SellerId;
                billSettel.AccountPartyId = n.AccountPartyId;
                billSettel.PartyName = n.Party.Name;
                billSettel.BillNumber = n.BillOfLading.WaybillNumber;
                billSettel.BillOfLadingId = n.BillOfLadingId;
                billSettel.BranchId = n.BranchId;
                billSettel.SettlementTypeId = n.SettlementTypeId;
                billSettel.IsSetteled = n.IsSettled;
                billSettel.Amount = n.Amount;
                billSettel.Bed = n.Bed;
                billSettel.Bes = n.Bes;
            }
            if (bill.WayBillType == 2 && n == null)
            {
                billSettel.SettlementTypeId = 1;
                billSettel.IsSetteled = true;
            }

            // 4. ایجاد و بازگشت BillOfLadingDto
            return new BillOfLadingDto
            {
                bill = billHeader,
                Consigments = consignments,
                Trackings = await _tracking.TrackingAsync(bill.Id, false, true),
                Financials = billSettel,

            };
        }
        public async Task<BillOfLadingDto> FindBillOfLadingDtoByNumberAsync(string wayBillnumber)
        {
            var bill = await _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.Consignments).ThenInclude(n => n.NatureType)
                .Include(n => n.IssuingBranch)
                .Include(n => n.OriginHub)
                .Include(n => n.DestinationHub)
                .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .Include(n => n.Service)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.FinancialTransactions)
                .FirstOrDefaultAsync(n => n.WaybillNumber == wayBillnumber);

            if (bill == null)
            {
                throw new InvalidOperationException("بارنامه با شناسه مورد نظر یافت نشد.");
            }

            // 2. تبدیل اطلاعات بارنامه به BillOfLadingDto_Header
            var billHeader = new BillOfLadingDto_Header
            {
                Id = bill.Id,
                SellerId = bill.SellerId,
                WaybillNumber = bill.WaybillNumber,
                IssuanceDate = bill.IssuanceDate,
                CustomerKeyword = bill.CustomerKeyword,
                RouteId = bill.RouteId,
                OriginCity = bill.Route.OriginCity.PersianName,
                DestinationCity = bill.Route.DestinationCity.PersianName,
                ServiceId = bill.ServiceId,
                ServiceName = bill.Service.ServiceName,
                OriginBranchId = bill.OriginBranchId,
                OriginBranchName = bill.IssuingBranch.BranchName,
                SenderId = bill.SenderId,
                SenderAddress = bill.SenderAddress,
                SenderName = bill.Sender.Name,
                SenderNationalCode = bill.Sender.NationalId,
                SenderPhone = string.IsNullOrEmpty(bill.SenderPhone) ? bill.Sender.MobilePhone : bill.SenderPhone,
                ReceiverId = bill.ReceiverId,
                ReceiverAddress = bill.ReceiverAddress,
                ReciverName = bill.Receiver.Name,
                ReciverPhone = string.IsNullOrEmpty(bill.ReceiverPhone) ? bill.Receiver.MobilePhone : bill.ReceiverPhone,
                Description = bill.Description,
                DeliveredCount = bill.DeliveredCount,
                SettelmentType = bill.FinancialTransactions.Any() ? (short)bill.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault()?.SettlementTypeId : null,
                LastStatusDescription = bill.LastStatusDescription,
                BillOfLadingStatusId = bill.BillOfLadingStatusId,
                OriginHubId = bill.OriginHubId,
                DestinationHubId = bill.DestinationHubId,
                CreatedBy = bill.CreatedBy,
                UpdatedDate = bill.UpdatedDate,
                UpdatedBy = bill.UpdatedBy,
                IsDeleted = bill.IsDeleted,
                VatRate = bill.Service.VatRate,
                tg_CourierManUserName = !string.IsNullOrEmpty(bill.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == bill.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == bill.tg_CourierManUserName).Family : "",
                tg_DeliveryDate = bill.tg_DeliveryDate,
                tg_Description = bill.tg_Description,
                tg_Name = bill.tg_Name,
                tg_NationalityCode = bill.tg_NationalityCode,
                tg_Phone = bill.tg_Phone,
                tg_SignatureData = bill.tg_SignatureData,
                Delivered = bill.Delivered,
                Setteled = bill.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any(),

                BillofLadingCosts = bill.BillCosts.GroupBy(n => n.CostType.CostCode).Select(n => new AddParcelCostDto
                {
                    Amount = n.Sum(z => z.Amount),
                    CostName = n.FirstOrDefault().CostType.Description,
                }).ToList(),
                FinalPrice = (bill.BillCosts.Sum(a => a.Amount)) - (bill.Consignments.Sum(d => d.Discount)),
            };

            // 3.  ConsigmentDto
            var consignments = bill.Consignments?.Select(c => new ConsigmentDto
            {
                Id = c.Id,
                SellerId = c.SellerId,
                Code = c.Code,
                Weight = c.Weight,
                Height = c.Height,
                Width = c.Width,
                Length = c.Length,
                Volume = c.Volume,
                ContentDescription = c.ContentDescription,
                Value = c.Value,
                ServiceInformation = c.ServiceInformation,
                IsDelivered = c.IsDelivered,
                RecipientName = c.RecipientName,
                DeliveryDate = c.DeliveryDate,
                ReceiverSignature = c.ReceiverSignature,
                CargoFare = c.CargoFare,
                TotalCostPrice = c.TotalCostPrice,
                Discount = c.Discount,
                VatRate = c.VatRate,
                VatPrice = c.VatPrice,
                TotalPrice = c.TotalPrice,
                BillOfLadingId = c.BillOfLadingId,
                NatureTypeId = c.NatureTypeId,
                NatureName = c.NatureType.Name,
                CreateAt = c.CreatedAt,
                CreateAtTime = c.CreatedAtTime,
                CreatedBy = c.CreatedBy,
                Costs = c.BillCosts.Select(z => new AddParcelCostDto
                {
                    Amount = z.Amount,
                    Code = z.CostType.CostCode,
                    CostId = z.CostTypeId,
                    CostName = z.CostType.Description,
                }).ToList(),

            }).ToList();

            var n = await _db.Cu_FinancialTransactions.Include(n => n.Party)
                .Where(n => n.BillOfLadingId == bill.Id && n.OperationId == 1).FirstOrDefaultAsync();

            var billSettel = new FinancialTransactionDto();
            if (n != null)
            {
                billSettel.Id = n.Id;
                billSettel.SellerId = n.SellerId;
                billSettel.AccountPartyId = n.AccountPartyId;
                billSettel.PartyName = n.Party.Name;
                billSettel.BillNumber = n.BillOfLading.WaybillNumber;
                billSettel.BillOfLadingId = n.BillOfLadingId;
                billSettel.BranchId = n.BranchId;
                billSettel.SettlementTypeId = n.SettlementTypeId;
                billSettel.IsSetteled = n.IsSettled;
                billSettel.Amount = n.Amount;
                billSettel.Bed = n.Bed;
                billSettel.Bes = n.Bes;
            }

            // 4. ایجاد و بازگشت BillOfLadingDto
            return new BillOfLadingDto
            {
                bill = billHeader,
                Consigments = consignments,
                Trackings = await _tracking.TrackingAsync(bill.Id, false, true),
                Financials = billSettel,

            };
        }
        public async Task<BillDataViewModel> GetBillDataAsync(Guid id)
        {
            var bill = await _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.Consignments).ThenInclude(n => n.NatureType)
                .Include(n => n.IssuingBranch)
                .Include(n => n.OriginHub)
                .Include(n => n.DestinationHub)
                .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .Include(n => n.Service)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.TreTransactions)
                .Include(n => n.FinancialTransactions).ThenInclude(n => n.MoneyTransactions)
                .SingleOrDefaultAsync(n => n.Id == id);

            if (bill == null)
            {
                throw new InvalidOperationException("بارنامه با شناسه مورد نظر یافت نشد.");
            }

            // 2. تبدیل اطلاعات بارنامه به BillOfLadingDto_Header
            var billHeader = new BillDataViewModel
            {
                Id = bill.Id,
                SellerId = bill.SellerId,

                ServiceName = bill.Service.ServiceName,
                WaybillNumber = bill.WaybillNumber,
                IssuanceDate = bill.IssuanceDate,
                RouteId = bill.RouteId,
                RouteName = bill.Route.RouteName,
                Description = bill.Description,
                StatusId = bill.BillOfLadingStatusId,

                SenderId = bill.ServiceId,
                SenderName = bill.Sender.Name,
                SenderAddress = bill.Sender.Address,
                SenderNationalId = bill.Sender.NationalId,

                ReceiverId = bill.ReceiverId,
                ReciverName = bill.Receiver.Name,
                ReceiverAddress = bill.ReceiverAddress,

                HasParcel = bill.Consignments.Any(),
                ParcelQty = bill.Consignments.Count,

                HasTreasuryRecord = bill.TreTransactions.Any(),

                TotalPrice = bill.BillCosts.Sum(s => s.Amount) - bill.Consignments.Sum(n => n.Discount),
                HasFinancialRecord = bill.FinancialTransactions.Any(),
                //TotalPrice = bill.FinancialTransactions.Where(n => n.OperationId == 1).Any() ? bill.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault().Amount : 0,
                PayedAmount = bill.TreTransactions.Any() ? bill.TreTransactions.Sum(n => n.CreditAmount) : 0,
                IsSetteled = bill.FinancialTransactions.Where(n => n.OperationId == 1).Any() ? bill.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault().IsSettled : false,
                SettelmentTYpeId = bill.FinancialTransactions.Where(n => n.OperationId == 1).Any() ? bill.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault().SettlementTypeId : null,


            };

            return billHeader;

        }
        public async Task<AddParcelHeaderInfo> GetParcelHeaderInfoAsync(Guid id)
        {
            var bill = await _db.Cu_BillOfLadings.AsNoTracking()
               .Include(n => n.IssuingBranch)
               .Include(n => n.OriginHub)
               .Include(n => n.DestinationHub)
               .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
               .Include(n => n.Service)
               .SingleOrDefaultAsync(n => n.Id == id);

            if (bill == null)
            {
                throw new InvalidOperationException("بارنامه با شناسه مورد نظر یافت نشد.");
            }

            AddParcelHeaderInfo info = new AddParcelHeaderInfo
            {
                BillOdLadingId = bill.Id,
                IssuanceDate = bill.IssuanceDate,
                WaybillNumber = bill.WaybillNumber,
                RouteId = bill.RouteId,
                OriginBranchId = bill.OriginBranchId,
                OriginBranchName = bill.IssuingBranch.BranchName,
                DestinationCity = bill.Route.DestinationCity.PersianName,
                SellerId = bill.SellerId,
                ServiceId = bill.ServiceId,
                ZoneId = bill.Route.ZoneId,
                ParcelQty = bill.Consignments.Count,
                VatRate = bill.Service.VatRate,
                MaxDiscountRate = bill.IssuingBranch.AllowdDiscountRate
            };

            return info;
        }

        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        public IQueryable<ViewBillOfLadings> GetBillsAsQuery_new(BillOfLadingFilterDto filter)
        {
            // 1. شروع کوئری پایه با پروجکشن دقیق
            var query = _db.Cu_BillOfLadings
                .AsNoTracking()
                .AsSplitQuery() // جلوگیری از کارتزین پروداکت
                .Where(b => b.SellerId == filter.SellerId && !b.IsDeleted);

            // 2. اعمال فیلترها با بهینه‌سازی شرط تاریخ
            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber))
                query = query.Where(b => b.WaybillNumber.Contains(filter.BiilOdLadingNumber));

            if (filter.OriginBranchId.HasValue)
                query = query.Where(b => b.OriginBranchId == filter.OriginBranchId);

            // ... سایر فیلترها ...

            // بهینه‌سازی شرط تاریخ
            var hasDateFilter = false;

            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                var fromDate = filter.strFromDate.PersianToLatin();
                query = query.Where(b => b.IssuanceDate >= fromDate);
                hasDateFilter = true;
            }

            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                var untilDate = filter.strUntilDate.PersianToLatin();
                query = query.Where(b => b.IssuanceDate <= untilDate.AddDays(1));
                hasDateFilter = true;
            }

            if (!string.IsNullOrEmpty(filter.CustomerKeyword))
            {
                query = query.Where(b => b.CustomerKeyword.Contains(filter.CustomerKeyword));
            }

            // شرط پیش‌فرض فقط زمانی اعمال شود که هیچ فیلتری وجود ندارد
            if (!hasDateFilter && string.IsNullOrEmpty(filter.BiilOdLadingNumber))
            {
                var fiveDaysAgo = DateTime.Now.AddDays(-5);
                query = query.Where(b =>
                    b.IssuanceDate >= fiveDaysAgo ||
                    (b.BillOfLadingStatusId < 11 ||
                     b.FinancialTransactions
                         .Any(ft => ft.OperationId == 1 &&
                                    !ft.IsSettled &&
                                    ft.SettlementTypeId != 3)));
            }

            // 3. پروجکشن نهایی با محاسبات بهینه‌شده
            var result = query.Select(b => new ViewBillOfLadings
            {
                Id = b.Id,
                SellerId = b.SellerId,
                WaybillNumber = b.WaybillNumber,
                IssuanceDate = b.IssuanceDate,
                IssuanceTime = b.IssuanceTime,
                OriginBranchName = b.IssuingBranch.BranchName,
                OriginCity = b.Route.OriginCity.PersianName,
                DestinationCity = b.Route.DestinationCity.PersianName,
                CustomerKeyword = b.CustomerKeyword,
                ServiceName = b.Service.ServiceName,
                SenderName = b.Sender.Name,
                ReceiverName = b.Receiver.Name,
                ReceiverPhone = !string.IsNullOrEmpty(b.ReceiverPhone)
                    ? b.ReceiverPhone
                    : b.Receiver.MobilePhone,
                OriginHubName = b.OriginHub.HubName,
                Description = b.Description,
                RouteName = b.Route.RouteName,
                ConsigmentCount = b.Consignments.Count,
                TotalWeight = b.Consignments.Sum(c => c.Weight),
                BillOfLadingStatusId = b.BillOfLadingStatusId,
                LastStatusDescription = b.BillOfLadingStatus.Name,
                TotalCost = b.BillCosts.Sum(bc => bc.Amount),
                TotalDiscount = b.Consignments.Sum(c => c.Discount),

                // بهینه‌سازی محاسبات مالی
                SettelmentTypeId = b.FinancialTransactions
                    .Where(ft => ft.OperationId == 1)
                    .Select(ft => (short?)ft.SettlementTypeId)
                    .FirstOrDefault(),

                IsSetteled = b.FinancialTransactions
                    .Any(ft => ft.OperationId == 1 && ft.IsSettled),

                CreatedBy = b.CreatedBy,
                UpdatedBy = b.UpdatedBy,
                UpdatedDate = b.UpdatedDate,
                IsDeleted = b.IsDeleted,

                // محاسبه کارآمد مبالغ پرداخت شده
                PayedAmount = b.FinancialTransactions
                    .Where(ft => ft.OperationId == 1)
                    .SelectMany(ft => ft.MoneyTransactions)
                    .Sum(mt => mt.CreditAmount),

                // حذف N+1 با جوین مستقیم
                tg_CourierManUserName = !string.IsNullOrEmpty(b.tg_CourierManUserName)
                    ? _db.Users
                        .Where(u => u.UserName == b.tg_CourierManUserName)
                        .Select(u => u.FName + " " + u.Family)
                        .FirstOrDefault()
                    : "",

                tg_DeliveryDate = b.tg_DeliveryDate,
                Delivered = b.Delivered,

                // بهینه‌سازی بارگیری محموله‌ها
                Parcels = b.Consignments.Select(c => new ConsigmentDto
                {
                    Id = c.Id,
                    BillOfLadingId = c.BillOfLadingId,
                    Code = c.Code,
                    NatureTypeId = c.NatureTypeId,
                    RecipientName = c.NatureType.Name,
                    ContentDescription = c.ContentDescription,
                    CargoFare = c.CargoFare,
                    Length = c.Length,
                    Width = c.Width,
                    Height = c.Height,
                    Weight = c.Weight,
                    Volume = c.Volume,
                    CreatedBy = c.CreatedBy,
                    Costs = c.BillCosts.Select(bc => new AddParcelCostDto
                    {
                        Amount = bc.Amount,
                        CostName = bc.CostType.Description
                    }).ToList()
                }).ToList()
            })
            .OrderByDescending(b => b.IssuanceDate)
            .ThenByDescending(b => b.IssuanceTime);

            return result;
        }
        public IQueryable<ViewBillOfLadings> GetBillsAsQuery(BillOfLadingFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.Consignments).ThenInclude(n => n.NatureType)
                .Include(n => n.IssuingBranch)
                .Include(n => n.OriginHub)
                .Include(n => n.DestinationHub)
                .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .Include(n => n.Service)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.FinancialTransactions).ThenInclude(n => n.MoneyTransactions)
                .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (filter.OriginBranchId.HasValue)  // بر اساس شعبه صادرکننده
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);
            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.RouteId == filter.RoutId);
            if (filter.OriginCityId.HasValue)  // بر ساس مبدأ
                query = query.Where(n => n.Route.OriginCityId == filter.OriginCityId.Value);
            if (filter.DestinationCityId.HasValue)   // مقصد
                query = query.Where(n => n.Route.DestinationCityId == filter.DestinationCityId.Value);
            if (!string.IsNullOrEmpty(filter.IssuerUserName))  // کاربر صادرکننده
                query = query.Where(n => n.CreatedBy == filter.IssuerUserName);

            if (!string.IsNullOrEmpty(filter.CustomerKeyword))
                query = query.Where(n => n.CustomerKeyword.Contains(filter.CustomerKeyword));

            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                DateTime date = filter.strFromDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date >= date.Date);
            }
            else if (string.IsNullOrEmpty(filter.strFromDate) && string.IsNullOrEmpty(filter.BiilOdLadingNumber))
            {
                query = query.Where(n => n.IssuanceDate >= DateTime.Now.AddDays(-5) && n.BillOfLadingStatusId < 11 || (!n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().IsSettled && n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId != 3));
            }
            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                DateTime date = filter.strUntilDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date <= date.Date);
            }

            if (filter.BillStatus?.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLadingStatusId));
            if (filter.SettelmentType.HasValue)
                query = query
                    .Where(n => n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1)
                    .FirstOrDefault().SettlementTypeId == filter.SettelmentType : false);
            if (filter.PaymentStatus.HasValue)
            {
                bool IsPayed = filter.PaymentStatus.Value == 1 ? true : false;
                query = query
                   .Where(n => n.FinancialTransactions != null ? n.FinancialTransactions.Where(f => f.OperationId == 1)
                   .FirstOrDefault().IsSettled == IsPayed : false);
            }
            if (filter.SenderId.HasValue)
                query = query.Where(n => n.SenderId == filter.SenderId.Value);
            if (filter.ReciverId.HasValue)
                query = query.Where(n => n.ReceiverId == filter.ReciverId.Value);


            var result = query.Select(n => new ViewBillOfLadings
            {
                Id = n.Id,
                SellerId = n.SellerId,

                WaybillNumber = n.WaybillNumber,
                IssuanceDate = n.IssuanceDate,
                IssuanceTime = n.IssuanceTime,
                OriginBranchName = n.IssuingBranch.BranchName,
                OriginCity = n.Route.OriginCity.PersianName,
                DestinationCity = n.Route.DestinationCity.PersianName,
                ServiceName = n.Service.ServiceName,
                SenderId = n.SenderId,
                SenderName = n.Sender.Name,
                SenderAddress = n.SenderAddress,
                ReceiverId = n.ReceiverId,
                ReceiverName = n.Receiver.Name,
                ReceiverAddress = n.ReceiverAddress,
                ReceiverPhone = string.IsNullOrEmpty(n.ReceiverPhone) ? n.Receiver.MobilePhone : n.ReceiverPhone,
                OriginHubId = n.OriginHubId,
                OriginHubName = n.OriginHub.HubName,
                Description = n.Description,
                RouteName = n.Route.RouteName,
                ConsigmentCount = n.Consignments.Count,
                TotalWeight = n.Consignments.Sum(x => x.Weight),

                BillOfLadingStatusId = n.BillOfLadingStatusId,
                LastStatusDescription = n.BillOfLadingStatus.Name,

                TotalCost = n.BillCosts.Sum(s => s.Amount),
                TotalDiscount = n.Consignments.Sum(s => s.Discount),
                SettelmentTypeId = n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId : null,
                IsSetteled = n.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any(),
                CreatedBy = n.CreatedBy,
                UpdatedBy = n.UpdatedBy,
                UpdatedDate = n.UpdatedDate,
                IsDeleted = n.IsDeleted,
                PayedAmount = n.FinancialTransactions != null ? n.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault() != null ? (n.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault().MoneyTransactions.Sum(n => n.CreditAmount)) : 0 : 0,
                tg_CourierManUserName = !string.IsNullOrEmpty(n.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).Family : "",
                tg_DeliveryDate = n.tg_DeliveryDate,
                tg_Description = n.tg_Description,
                tg_Name = n.tg_Name,
                tg_NationalityCode = n.tg_NationalityCode,
                tg_Phone = n.tg_Phone,
                tg_SignatureData = n.tg_SignatureData,
                Delivered = n.Delivered,
                Parcels = n.Consignments.Select(x => new ConsigmentDto
                {
                    Id = x.Id,
                    BillOfLadingId = x.BillOfLadingId,

                    Code = x.Code,
                    NatureTypeId = x.NatureTypeId,
                    RecipientName = x.NatureType.Name,
                    ContentDescription = x.ContentDescription,

                    CargoFare = x.CargoFare,
                    PackageTypeId = x.PackagetypeId,
                    Length = x.Length,
                    Width = x.Width,
                    Height = x.Height,
                    Weight = x.Weight,
                    Volume = x.Volume,
                    CreatedBy = x.CreatedBy,
                    Costs = x.BillCosts.Select(c => new AddParcelCostDto
                    {
                        Amount = c.Amount,
                        CostName = c.CostType.Description
                    }).ToList(),
                }).ToList(),

            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------

        public IQueryable<ViewBillOfLadings> GetInputBillsAsQuery(BillOfLadingFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.Consignments).ThenInclude(n => n.NatureType)
                .Include(n => n.IssuingBranch)
                .Include(n => n.OriginHub)
                .Include(n => n.DestinationHub)
                .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .Include(n => n.Service)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.FinancialTransactions).ThenInclude(n => n.MoneyTransactions)
                .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted && n.BillOfLadingStatusId >= 2)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (!string.IsNullOrEmpty(filter.ReferenceNumber)) // فیلتر بر اساس بحشی از شماره بارنامه مرجع
                query = query.Where(n => n.ReferenceCode.Contains(filter.ReferenceNumber));
            if (filter.OriginBranchId.HasValue)  // بر اساس شعبه صادرکننده
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);
            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.RouteId == filter.RoutId);
            if (filter.OriginCityId.HasValue)  // بر ساس مبدأ
                query = query.Where(n => n.Route.OriginCityId == filter.OriginCityId.Value);
            if (filter.DestinationCityId.HasValue)   // مقصد
                query = query.Where(n => n.Route.DestinationCityId == filter.DestinationCityId.Value);
            if (!string.IsNullOrEmpty(filter.IssuerUserName))  // کاربر صادرکننده
                query = query.Where(n => n.CreatedBy == filter.IssuerUserName);

            if (!string.IsNullOrEmpty(filter.CustomerKeyword))
                query = query.Where(n => n.CustomerKeyword.Contains(filter.CustomerKeyword));


            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                DateTime date = filter.strFromDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date >= date.Date);
            }
            else if (string.IsNullOrEmpty(filter.strFromDate) && string.IsNullOrEmpty(filter.BiilOdLadingNumber))
            {
                query = query.Where(n => n.IssuanceDate >= DateTime.Now.AddDays(-5) && (n.BillOfLadingStatusId < 11 && n.BillOfLadingStatusId >= 2) || (n.BillOfLadingStatusId >= 2 && !n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().IsSettled && n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId != 3));
            }
            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                DateTime date = filter.strUntilDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date <= date.Date);
            }

            if (filter.BillStatus?.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLadingStatusId));

            if (filter.SettelmentType.HasValue)
                query = query
                    .Where(n => n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1)
                    .FirstOrDefault().SettlementTypeId == filter.SettelmentType : false);

            if (filter.PaymentStatus.HasValue)
            {
                bool IsPayed = filter.PaymentStatus.Value == 1 ? true : false;
                query = query
                   .Where(n => n.FinancialTransactions != null ? n.FinancialTransactions.Where(f => f.OperationId == 1)
                   .FirstOrDefault().IsSettled == IsPayed : false);
            }

            if (filter.SenderId.HasValue)
                query = query.Where(n => n.SenderId == filter.SenderId.Value);

            if (filter.ReciverId.HasValue)
                query = query.Where(n => n.ReceiverId == filter.ReciverId.Value);

            var result = query.Select(n => new ViewBillOfLadings
            {
                Id = n.Id,
                SellerId = n.SellerId,

                WaybillNumber = n.WaybillNumber,
                ReferenceNumber = n.ReferenceCode,
                IssuanceDate = n.IssuanceDate,
                IssuanceTime = n.IssuanceTime,
                OriginBranchName = n.IssuingBranch.BranchName,
                OriginCity = n.Route.OriginCity.PersianName,
                DestinationCity = n.Route.DestinationCity.PersianName,
                ServiceName = n.Service.ServiceName,
                SenderId = n.SenderId,
                SenderName = n.Sender.Name,
                SenderAddress = n.SenderAddress,
                ReceiverId = n.ReceiverId,
                ReceiverName = n.Receiver.Name,
                ReceiverAddress = n.ReceiverAddress,
                ReceiverPhone = string.IsNullOrEmpty(n.ReceiverPhone) ? n.Receiver.MobilePhone : n.ReceiverPhone,
                OriginHubId = n.OriginHubId,
                OriginHubName = n.OriginHub.HubName,
                Description = n.Description,
                RouteName = n.Route.RouteName,
                ConsigmentCount = n.Consignments.Count,

                BillOfLadingStatusId = n.BillOfLadingStatusId,
                LastStatusDescription = n.BillOfLadingStatus.Name,

                TotalCost = n.BillCosts.Sum(s => s.Amount),
                TotalDiscount = n.Consignments.Sum(s => s.Discount),
                SettelmentTypeId = n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId : null,
                IsSetteled = n.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any(),
                CreatedBy = n.CreatedBy,
                UpdatedBy = n.UpdatedBy,
                UpdatedDate = n.UpdatedDate,
                IsDeleted = n.IsDeleted,
                PayedAmount = n.FinancialTransactions != null ? n.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault() != null ? (n.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault().MoneyTransactions.Sum(n => n.CreditAmount)) : 0 : 0,
                tg_CourierManUserName = !string.IsNullOrEmpty(n.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).Family : "",
                tg_DeliveryDate = n.tg_DeliveryDate,
                tg_Description = n.tg_Description,
                tg_Name = n.tg_Name,
                tg_NationalityCode = n.tg_NationalityCode,
                tg_Phone = n.tg_Phone,
                tg_SignatureData = n.tg_SignatureData,
                Delivered = n.Delivered,
                TotalWeight = n.Consignments.Sum(x => x.Weight),
                Parcels = n.Consignments.Select(x => new ConsigmentDto
                {
                    Id = x.Id,
                    BillOfLadingId = x.BillOfLadingId,

                    Code = x.Code,
                    NatureTypeId = x.NatureTypeId,
                    RecipientName = x.NatureType.Name,
                    ContentDescription = x.ContentDescription,

                    CargoFare = x.CargoFare,
                    PackageTypeId = x.PackagetypeId,
                    Length = x.Length,
                    Width = x.Width,
                    Height = x.Height,
                    Weight = x.Weight,
                    Volume = x.Volume,
                    CreatedBy = x.CreatedBy,
                    Costs = x.BillCosts.Select(c => new AddParcelCostDto
                    {
                        Amount = c.Amount,
                        CostName = c.CostType.Description
                    }).ToList(),
                }).ToList(),

            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }
        public IQueryable<ViewBillOfLadings> GetSimpleBillsAsQuery(BillOfLadingFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.IssuingBranch)
                .Include(n => n.Route)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.FinancialTransactions)
                .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (filter.OriginBranchId.HasValue)  // بر اساس شعبه صادرکننده
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);
            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.RouteId == filter.RoutId);
            if (filter.OriginCityId.HasValue)  // بر ساس مبدأ
                query = query.Where(n => n.Route.OriginCityId == filter.OriginCityId.Value);
            if (filter.DestinationCityId.HasValue)   // مقصد
                query = query.Where(n => n.Route.DestinationCityId == filter.DestinationCityId.Value);
            if (!string.IsNullOrEmpty(filter.IssuerUserName))  // کاربر صادرکننده
                query = query.Where(n => n.CreatedBy == filter.IssuerUserName);

            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                DateTime date = filter.strFromDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date >= date.Date);
            }
            else
            {
                query = query.Where(n => n.IssuanceDate.Date >= DateTime.Now.AddDays(-3).Date || (n.BillOfLadingStatusId < 11));
            }
            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                DateTime date = filter.strUntilDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date <= date.Date);
            }

            if (filter.BillStatus?.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLadingStatusId));
            if (filter.SettelmentType.HasValue)
                query = query
                    .Where(n => n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1)
                    .FirstOrDefault().SettlementTypeId == filter.SettelmentType : false);

            if (!string.IsNullOrEmpty(filter.CustomerKeyword))
                query = query.Where(n => n.CustomerKeyword.Contains(filter.CustomerKeyword));

            if (filter.PaymentStatus.HasValue)
            {
                bool IsPayed = filter.PaymentStatus.Value == 1 ? true : false;
                query = query
                   .Where(n => n.FinancialTransactions != null ? n.FinancialTransactions.Where(f => f.OperationId == 1)
                   .FirstOrDefault().IsSettled == IsPayed : false);
            }
            if (filter.SenderId.HasValue)
                query = query.Where(n => n.SenderId == filter.SenderId.Value);
            if (filter.ReciverId.HasValue)
                query = query.Where(n => n.ReceiverId == filter.ReciverId.Value);


            var result = query.Select(n => new ViewBillOfLadings
            {
                Id = n.Id,
                SellerId = n.SellerId,

                WaybillNumber = n.WaybillNumber,
                IssuanceDate = n.IssuanceDate,
                IssuanceTime = n.IssuanceTime,
                OriginBranchName = n.IssuingBranch.BranchName,
                SenderId = n.SenderId,
                ReceiverAddress = n.ReceiverAddress,
                ReceiverPhone = string.IsNullOrEmpty(n.ReceiverPhone) ? n.Receiver.MobilePhone : n.ReceiverPhone,
                Description = n.Description,
                RouteName = n.Route.RouteName,
                ConsigmentCount = n.Consignments.Count,

                BillOfLadingStatusId = n.BillOfLadingStatusId,
                LastStatusDescription = n.BillOfLadingStatus.Name,

                SettelmentTypeId = n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId : null,
                IsSetteled = n.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any(),
                CreatedBy = n.CreatedBy,
                UpdatedBy = n.UpdatedBy,
                UpdatedDate = n.UpdatedDate,
                IsDeleted = n.IsDeleted,
                tg_CourierManUserName = n.tg_CourierManUserName,
                tg_DeliveryDate = n.tg_DeliveryDate,
                tg_Description = n.tg_Description,
                tg_Name = n.tg_Name,
                tg_NationalityCode = n.tg_NationalityCode,
                tg_Phone = n.tg_Phone,
                Delivered = n.Delivered,
                TotalWeight = n.Consignments.Sum(x => x.Weight),
            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }


        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------
        public async Task<Guid> getBillIdByNumberAsync(string BillNumber)
        {
            Guid id = await _db.Cu_BillOfLadings.Where(n => n.WaybillNumber == BillNumber).Select(n => n.Id).FirstOrDefaultAsync();
            return id;
        }
        public async Task<BillCashPayDto> GetBillCashPayDtoAsync(Guid id)
        {
            BillCashPayDto model = new BillCashPayDto();

            var bill = await _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.Service)
                .Include(n => n.BillCosts)
                .Include(n => n.Sender)
                .SingleOrDefaultAsync(n => n.Id == id);

            if (bill == null)
            {
                return null;
                throw new InvalidOperationException("بارنامه با شناسه مورد نظر یافت نشد.");
            }


            long billPrice = bill.BillCosts.Sum(n => n.Amount) - bill.Consignments.Sum(n => n.Discount);

            model.BillCreateDate = bill.IssuanceDate;
            model.BillOfLadingId = bill.Id;
            model.BillNumber = bill.WaybillNumber;
            model.Amount = billPrice;
            model.DebitAmount = billPrice;
            model.AccountPartyId = bill.SenderId;
            model.BranchId = bill.OriginBranchId;
            model.SellerId = bill.SellerId;
            model.PartyMobile = bill.Sender.MobilePhone;
            model.PartyName = bill.Sender.Name;
            model.SenderName = bill.Sender.Name;
            model.Description = $"بابت بارنامه شماره {bill.WaybillNumber} - {bill.Sender.Name}";
            model.SettelmentType = bill.FinancialTransactions.Where(n => n.OperationId == 1) != null && bill.FinancialTransactions.Where(n => n.OperationId == 1).Any() ? bill.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault().SettlementTypeId : 0;

            var billTransaction = await _db.Cu_FinancialTransactions
               .Where(n => n.BillOfLadingId == id && n.OperationId == 1).FirstOrDefaultAsync();

            if (billTransaction != null)
            {
                model.BillFinancialtransactionId = billTransaction.Id;

                var payments = await _db.TreTransactions.Include(n => n.User)
                    .Where(n => n.BillFinancialtransactionId == billTransaction.Id && n.CreditAmount > 0)
                    .Select(n => new PaymentListViewModel
                    {
                        TransactionId = n.Id,
                        Amount = n.CreditAmount,
                        BankName = n.BankAccount == null ? "" : (n.BankAccount.Bank.Name + "-" + n.BankAccount.AccountNumber),
                        PayedDate = n.TransactionDate,
                        PayedTime = n.TransactionTime,
                        BillId = bill.Id,
                        BillNumber = bill.WaybillNumber,
                        CouierTransactionId = billTransaction.Id,
                        PaymentMethod = n.Operation.OperationName,
                        ReciptNumber = n.TransferNumber,
                        PosName = n.Pos == null ? "" : n.Pos.Name,
                        Cashier = n.User.FName + " " + n.User.Family,
                        GetwayTransactionNumber = n.PaymentGatewayTransactionId,

                    }).ToListAsync();
                model.PaymentList = payments;

                if (!payments.Any())
                {
                    model.CreditAmount = 0;
                    model.DebitAmount = billPrice;
                }
                else
                {
                    model.CreditAmount = payments.Sum(n => n.Amount);
                    model.DebitAmount = billPrice - model.CreditAmount;

                }
            }
            return model;
        }
        public async Task<clsResult> DeleteBillAsync(BillDataViewModel bill)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (bill.StatusId >= 3)
            {
                result.Message = "صدور بارنامه نهایی شده و امکان حذف آن وجود ندارد.";
                return result;
            }
            if (bill.HasTreasuryRecord)
            {
                result.Message = " بارنامه دارای اطلاعات مالی  می باشد و امکان حذف آن وجود ندارد.";
                return result;
            }

            var biilRecord = await _db.Cu_BillOfLadings.FindAsync(bill.Id);
            biilRecord.IsDeleted = true;
            biilRecord.UpdatedBy = bill.UpdatedBy;
            biilRecord.UpdatedDate = DateTime.Now;
            _db.Cu_BillOfLadings.Update(biilRecord);
            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "حذف بارنامه با موفقیت انجام شد";

            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "خطایی در حذف بارنامه رخ داده است";
            }

            return result;
        }
        public async Task<clsResult> RemoveBillAsync(BillDataViewModel bill)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (bill.StatusId >= 3)
            {
                result.Message = "صدور بارنامه نهایی شده و امکان حذف آن وجود ندارد.";
                return result;
            }
            if (bill.HasTreasuryRecord)
            {
                result.Message = " بارنامه دارای اطلاعات مالی  می باشد و امکان حذف آن وجود ندارد.";
                return result;
            }

            var transactions = await _db.Cu_FinancialTransactions.Where(n => n.BillOfLadingId == bill.Id).ToListAsync();
            var costData = await _db.Cu_BillCosts.Where(n => n.BillOfLadingId == bill.Id).ToListAsync();
            var Parcels = await _db.Cu_Consignments.Where(n => n.BillOfLadingId == bill.Id).ToListAsync();
            var biilRecord = await _db.Cu_BillOfLadings.FindAsync(bill.Id);

            _db.Cu_FinancialTransactions.RemoveRange(transactions);
            _db.Cu_BillCosts.RemoveRange(costData);
            _db.Cu_Consignments.RemoveRange(Parcels);
            _db.Cu_BillOfLadings.Remove(biilRecord);
            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "حذف بارنامه با موفقیت انجام شد";

            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "خطایی در حذف بارنامه رخ داده است";
            }

            return result
                ;
        }
        public async Task<clsResult> BillCancelationAsync(BillDataViewModel bill)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            if (bill.StatusId > 3)
            {
                result.Message = "بعداز رهسپاری امکال ابطال بارنامه وجود ندارد";
                return result;
            }

            if (bill.HasTreasuryRecord)
            {
                result.Message = " بارنامه دارای اطلاعات مالی  می باشد جهت ابطال آن از طریق مدیریت یا واحد حسابداری اقدام نمائید  .";
                return result;
            }

            var billRecord = await _db.Cu_BillOfLadings.FindAsync(bill.Id);
            billRecord.UpdatedBy = bill.UpdatedBy;
            billRecord.UpdatedDate = DateTime.Now;
            billRecord.BillOfLadingStatusId = 15;
            billRecord.LastStatusDescription = $" بارنامه در تاریخ {DateTime.Now.LatinToPersian()} توسط {bill.UpdatedBy} باطل گردید";
            _db.Cu_BillOfLadings.Update(billRecord);
            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "ابطال بارنامه با موفقیت انجام شد";

            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "خطایی در ابطال بارنامه رخ داده است";
            }

            return result;
        }

        // Get Bills 
        public IQueryable<ViewBillOfLadings> GetIssuindBillsAsQuery(BillOfLadingFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.IssuingBranch)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.FinancialTransactions)
                .Include(n => n.DistributerBranch)
                .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (filter.OriginBranchId.HasValue)  // بر اساس شعبه صادرکننده
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);
            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.RouteId == filter.RoutId);
            if (filter.OriginCityId.HasValue)  // بر ساس مبدأ
                query = query.Where(n => n.Route.OriginCityId == filter.OriginCityId.Value);
            if (filter.DestinationCityId.HasValue)   // مقصد
                query = query.Where(n => n.Route.DestinationCityId == filter.DestinationCityId.Value);
            if (!string.IsNullOrEmpty(filter.IssuerUserName))  // کاربر صادرکننده
                query = query.Where(n => n.CreatedBy == filter.IssuerUserName);

            if (!string.IsNullOrEmpty(filter.CustomerKeyword))
                query = query.Where(n => n.CustomerKeyword.Contains(filter.CustomerKeyword));


            if (!string.IsNullOrEmpty(filter.strWayBillDate))
            {
                DateTime date = filter.strWayBillDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date == date.Date);
            }

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

            if (filter.BillStatus?.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLadingStatusId));


            if (filter.SenderId.HasValue)
                query = query.Where(n => n.SenderId == filter.SenderId.Value);
            if (filter.ReciverId.HasValue)
                query = query.Where(n => n.ReceiverId == filter.ReciverId.Value);


            var result = query.Select(n => new ViewBillOfLadings
            {
                Id = n.Id,
                SellerId = n.SellerId,

                WaybillNumber = n.WaybillNumber,
                IssuanceDate = n.IssuanceDate,
                IssuanceTime = n.IssuanceTime,
                OriginBranchName = n.IssuingBranch.BranchName,
                SenderId = n.SenderId,
                SenderName = n.Sender.Name,
                SenderAddress = n.SenderAddress,
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
                SettelmentTypeId = n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId : null,
                IsSetteled = n.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any(),
                CreatedBy = n.CreatedBy,
                UpdatedBy = n.UpdatedBy,
                UpdatedDate = n.UpdatedDate,
                IsDeleted = n.IsDeleted,
                DistributerBranchId = n.DistributerBranchId,
                DistributerBranch = n.DistributerBranch != null ? n.DistributerBranch.BranchName : "",

            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }
        public IQueryable<ViewBillOfLadings> GetPendingDistributionAsQuery(BillOfLadingFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments)
                .Include(n => n.IssuingBranch)
                .Include(n => n.OriginHub)
                .Include(n => n.DestinationHub)
                .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.DistributerBranch)
                .Include(n => n.FinancialTransactions).ThenInclude(n => n.MoneyTransactions)
                .Where(n =>
                n.SellerId == filter.SellerId && !n.IsDeleted
                && (n.BillOfLadingStatusId >= 2 && n.BillOfLadingStatusId < 11))
                .AsQueryable();

            if (filter.branchIsOwner)
            {
                query = query.Where(n =>
                (n.DistributerBranchId == filter.DestinationBranchId || n.DistributerBranchId == null)
                && n.Route.DestinationCityId == filter.DestinationCityId.Value);
            }
            else
            {
                query = query.Where(n => n.DistributerBranchId == filter.DestinationBranchId && n.DistributerBranchId != null);
            }

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));

            if (!string.IsNullOrEmpty(filter.ReferenceNumber)) // فیلتر بر اساس بحشی از شماره بارنامه وارده
                query = query.Where(n => n.ReferenceCode.Contains(filter.ReferenceNumber));

            if (filter.OriginBranchId.HasValue)  // بر اساس شعبه صادرکننده
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);

            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.RouteId == filter.RoutId);


            if (filter.OriginCityId.HasValue)  // بر ساس مبدأ
                query = query.Where(n => n.Route.OriginCityId == filter.OriginCityId.Value);


            if (filter.Partner.HasValue)
            {
                query = query.Where(n => filter.Partner == 0 ? n.BusinessPartnerId == null : n.BusinessPartnerId == filter.Partner);
            }

            if (!string.IsNullOrEmpty(filter.IssuerUserName))  // کاربر صادرکننده
                query = query.Where(n => n.CreatedBy == filter.IssuerUserName);


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

            if (filter.BillStatus?.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLadingStatusId));

            if (!string.IsNullOrEmpty(filter.CustomerKeyword))
                query = query.Where(n => n.CustomerKeyword.Contains(filter.CustomerKeyword));

            if (filter.SettelmentType.HasValue)
                query = query
                    .Where(n => n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1)
                    .FirstOrDefault().SettlementTypeId == filter.SettelmentType : false);

            if (filter.PaymentStatus.HasValue)
            {
                bool IsPayed = filter.PaymentStatus.Value == 1 ? true : false;
                query = query
                   .Where(n => n.FinancialTransactions != null ? n.FinancialTransactions.Where(f => f.OperationId == 1)
                   .FirstOrDefault().IsSettled == IsPayed : false);
            }

            if (filter.SenderId.HasValue)
                query = query.Where(n => n.SenderId == filter.SenderId.Value);

            if (filter.ReciverId.HasValue)
                query = query.Where(n => n.ReceiverId == filter.ReciverId.Value);

            var result = query.Select(n => new ViewBillOfLadings
            {
                Id = n.Id,
                SellerId = n.SellerId,
                WaybillNumber = n.WaybillNumber,
                ReferenceNumber = n.ReferenceCode,
                IssuanceDate = n.IssuanceDate,
                IssuanceTime = n.IssuanceTime,
                OriginBranchName = n.IssuingBranch.BranchName,
                SenderId = n.SenderId,
                SenderName = n.Sender.Name,
                SenderAddress = n.SenderAddress,
                ReceiverId = n.ReceiverId,
                ReceiverName = n.Receiver.Name,
                ReceiverAddress = n.ReceiverAddress,
                ReceiverPhone = string.IsNullOrEmpty(n.ReceiverPhone) ? n.Receiver.MobilePhone : n.ReceiverPhone,
                OriginHubId = n.OriginHubId,
                OriginHubName = n.OriginHub.HubName,
                Description = n.Description,
                RouteName = n.Route.RouteName,
                ConsigmentCount = n.Consignments.Count,

                BillOfLadingStatusId = n.BillOfLadingStatusId,
                LastStatusDescription = n.BillOfLadingStatus.Name,

                TotalCost = n.BillCosts.Sum(s => s.Amount),
                TotalDiscount = n.Consignments.Sum(s => s.Discount),
                SettelmentTypeId = n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId : null,
                IsSetteled = n.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any(),
                CreatedBy = n.CreatedBy,
                UpdatedBy = n.UpdatedBy,
                UpdatedDate = n.UpdatedDate,
                IsDeleted = n.IsDeleted,
                PayedAmount = n.FinancialTransactions != null ? n.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault() != null ? (n.FinancialTransactions.Where(n => n.OperationId == 1).FirstOrDefault().MoneyTransactions.Sum(n => n.CreditAmount)) : 0 : 0,
                tg_CourierManUserName = !string.IsNullOrEmpty(n.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).Family : "",
                tg_DeliveryDate = n.tg_DeliveryDate,
                tg_Description = n.tg_Description,
                tg_Name = n.tg_Name,
                tg_NationalityCode = n.tg_NationalityCode,
                tg_Phone = n.tg_Phone,
                tg_SignatureData = n.tg_SignatureData,
                Delivered = n.Delivered,
                TotalWeight = n.Consignments.Sum(x => x.Weight),
                DistributerBranchId = n.DistributerBranchId,
                DistributerBranch = n.DistributerBranch != null ? n.DistributerBranch.BranchName : "",

            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }
        public IQueryable<ViewBillOfLadings> GetNoSetteledBranchBillsAsQuery(BillOfLadingFilterDto filter, int destinationCityId)
        {

            var query = _db.Cu_FinancialTransactions.AsNoTracking()
                .Include(n => n.BillOfLading).ThenInclude(n => n.Consignments)
                .Include(n => n.BillOfLading).ThenInclude(n => n.Route)
                .Include(n => n.BillOfLading).ThenInclude(n => n.BillOfLadingStatus)
                .Include(n => n.BillOfLading).ThenInclude(n => n.BillCosts)
                .Include(n => n.BillOfLading.DistributerBranch)
                .Where(n =>
                       (n.SellerId == filter.SellerId && !n.IsDeleted && !n.IsSettled && n.OperationId == 1)
                       && (n.BillOfLading.BillOfLadingStatusId == 11)
                       && ((n.BillOfLading.OriginBranchId == filter.OriginBranchId && n.SettlementTypeId == 1)
                       || (n.BillOfLading.Route.DestinationCityId == destinationCityId && n.SettlementTypeId == 2))
                )
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.BillOfLading.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.BillOfLading.RouteId == filter.RoutId);

            if (!string.IsNullOrEmpty(filter.IssuerUserName))  // کاربر صادرکننده
                query = query.Where(n => n.BillOfLading.CreatedBy == filter.IssuerUserName);

            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                DateTime date = filter.strFromDate.PersianToLatin();
                query = query.Where(n => n.BillOfLading.IssuanceDate.Date >= date.Date);
            }
            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                DateTime date = filter.strUntilDate.PersianToLatin();
                query = query.Where(n => n.BillOfLading.IssuanceDate.Date <= date.Date);
            }

            if (filter.BillStatus?.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLading.BillOfLadingStatusId));

            if (filter.SenderId.HasValue)
                query = query.Where(n => n.BillOfLading.SenderId == filter.SenderId.Value);

            if (filter.ReciverId.HasValue)
                query = query.Where(n => n.BillOfLading.ReceiverId == filter.ReciverId.Value);

            if (filter.SettelmentType.HasValue)
                query = query.Where(n => n.SettlementTypeId == filter.SettelmentType.Value);


            var result = query.GroupBy(n => new { n.BillOfLadingId, n.OperationId }).Select(n => new ViewBillOfLadings
            {

                Id = n.Key.BillOfLadingId.Value,
                SellerId = n.FirstOrDefault().SellerId,

                WaybillNumber = n.FirstOrDefault().BillOfLading.WaybillNumber,
                IssuanceDate = n.FirstOrDefault().BillOfLading.IssuanceDate,
                IssuanceTime = n.FirstOrDefault().BillOfLading.IssuanceTime,
                OriginBranchName = n.FirstOrDefault().BillOfLading.IssuingBranch.BranchName,
                SenderId = n.FirstOrDefault().BillOfLading.SenderId,
                SenderName = n.FirstOrDefault().BillOfLading.Sender.Name,
                SenderAddress = n.FirstOrDefault().BillOfLading.SenderAddress,
                ReceiverId = n.FirstOrDefault().BillOfLading.ReceiverId,
                ReceiverName = n.FirstOrDefault().BillOfLading.Receiver.Name,
                ReceiverAddress = n.FirstOrDefault().BillOfLading.ReceiverAddress,
                ReceiverPhone = string.IsNullOrEmpty(n.FirstOrDefault().BillOfLading.ReceiverPhone) ? n.FirstOrDefault().BillOfLading.Receiver.MobilePhone : n.FirstOrDefault().BillOfLading.ReceiverPhone,
                OriginHubId = n.FirstOrDefault().BillOfLading.OriginHubId,
                OriginHubName = n.FirstOrDefault().BillOfLading.OriginHub.HubName,
                Description = n.FirstOrDefault().BillOfLading.Description,
                RouteName = n.FirstOrDefault().BillOfLading.Route.RouteName,
                ConsigmentCount = n.FirstOrDefault().BillOfLading.Consignments.Count,

                BillOfLadingStatusId = n.FirstOrDefault().BillOfLading.BillOfLadingStatusId,
                LastStatusDescription = n.FirstOrDefault().BillOfLading.BillOfLadingStatus.Name,

                TotalCost = n.FirstOrDefault().BillOfLading.BillCosts.Sum(s => s.Amount),
                TotalDiscount = n.FirstOrDefault().BillOfLading.Consignments.Sum(s => s.Discount),
                SettelmentTypeId = (short)n.FirstOrDefault().SettlementTypeId,
                CreatedBy = n.FirstOrDefault().BillOfLading.CreatedBy,
                tg_CourierManUserName = !string.IsNullOrEmpty(n.FirstOrDefault().BillOfLading.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == n.FirstOrDefault().BillOfLading.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == n.FirstOrDefault().BillOfLading.tg_CourierManUserName).Family : "",
                tg_DeliveryDate = n.FirstOrDefault().BillOfLading.tg_DeliveryDate,
                tg_Description = n.FirstOrDefault().BillOfLading.tg_Description,
                tg_Name = n.FirstOrDefault().BillOfLading.tg_Name,
                tg_NationalityCode = n.FirstOrDefault().BillOfLading.tg_NationalityCode,
                tg_Phone = n.FirstOrDefault().BillOfLading.tg_Phone,
                tg_SignatureData = n.FirstOrDefault().BillOfLading.tg_SignatureData,
                Delivered = n.FirstOrDefault().BillOfLading.Delivered,
                TotalWeight = n.FirstOrDefault().BillOfLading.Consignments.Sum(x => x.Weight),
                DistributerBranch = n.FirstOrDefault().BillOfLading.DistributerBranch != null ? n.FirstOrDefault().BillOfLading.DistributerBranch.BranchName : "",

            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }
        public IQueryable<ViewBillOfLadings> GetWaybillsAsQuery(BillOfLadingFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments).ThenInclude(n => n.BillCosts)
                .Include(n => n.IssuingBranch)
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts)
                .Include(n => n.FinancialTransactions)
                .Include(n => n.DistributerBranch)
                .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted)
                .AsQueryable();

            if (!filter.branchIsOwner)
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);

            if (!string.IsNullOrEmpty(filter.IssuerUserName))
                query = query.Where(n => n.CreatedBy == filter.IssuerUserName);

            if (filter.Sectiontype == 2)
            {
                query = query.Where(n => n.Route.DestinationCityId == filter.DestinationCityId);
            }
            if (filter.Sectiontype == 3)
            {
                if (filter.IsBranchManager)
                    query = query.Where(n => n.Route.OriginCityId == filter.BranchCityId);
                else
                    query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);
            }

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
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
            if (!string.IsNullOrEmpty(filter.CustomerKeyword))
                query = query.Where(n => n.CustomerKeyword.Contains(filter.CustomerKeyword));

            if (filter.RoutId.HasValue) // بر اساس مسیر
                query = query.Where(n => n.RouteId == filter.RoutId);

            if (filter.SenderId.HasValue)
            {
                if (filter.personSearchtype == 1)
                {
                    query = query.Where(n => n.SenderId == filter.SenderId.Value || n.ReceiverId == filter.SenderId.Value);
                }
                else if (filter.personSearchtype == 2)
                {
                    query = query.Where(n => n.ReceiverId == filter.SenderId.Value);
                }
                else if (filter.personSearchtype == 3)
                {
                    query = query.Where(n => n.SenderId == filter.SenderId.Value);
                }
            }

            if (filter.SettelmentType.HasValue)
                query = query
                    .Where(n => n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1)
                    .FirstOrDefault().SettlementTypeId == filter.SettelmentType : false);
            if (filter.PaymentStatus.HasValue)
            {
                bool IsPayed = filter.PaymentStatus.Value == 1 ? true : false;
                query = query
                   .Where(n => n.FinancialTransactions != null ? n.FinancialTransactions.Where(f => f.OperationId == 1)
                   .FirstOrDefault().IsSettled == IsPayed : false);
            }


            if (!filter.ShowCancelation)
                query = query.Where(n => n.BillOfLadingStatusId != 15 && n.BillOfLadingStatusId != 16);

            if (filter.BillStatus != null && filter.BillStatus.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLadingStatusId));

            if (filter.Distributer.HasValue)
                query = query.Where(n => n.DistributerBranchId == filter.Distributer);

            var result = query.Select(n => new ViewBillOfLadings
            {
                Id = n.Id,
                SellerId = n.SellerId,

                WaybillNumber = n.WaybillNumber,
                IssuanceDate = n.IssuanceDate,
                IssuanceTime = n.IssuanceTime,
                OriginBranchName = n.IssuingBranch.BranchName,
                SenderId = n.SenderId,
                SenderName = n.Sender.Name,
                SenderAddress = n.SenderAddress,
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
                SettelmentTypeId = n.FinancialTransactions != null ? (short)n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId : null,
                IsSetteled = n.FinancialTransactions.Where(n => n.IsSettled && n.OperationId == 1).Any(),
                CreatedBy = n.CreatedBy,
                UpdatedBy = n.UpdatedBy,
                UpdatedDate = n.UpdatedDate,
                IsDeleted = n.IsDeleted,

                tg_DeliveryDate = n.tg_DeliveryDate,
                tg_Description = n.tg_Description,
                tg_CourierManUserName = !string.IsNullOrEmpty(n.tg_CourierManUserName) ? _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).FName + " " + _db.Users.FirstOrDefault(u => u.UserName == n.tg_CourierManUserName).Family : "",
                DistributerBranchId = n.DistributerBranchId,
                DistributerBranch = n.DistributerBranch != null ? n.DistributerBranch.BranchName : "",

            }).OrderByDescending(n => n.IssuanceDate).ThenByDescending(n => n.IssuanceTime).AsQueryable();

            return result;
        }

        public async Task<List<WaybillLabelDto>> WaybillLabelsAsync(Guid id)
        {
            var data = await _db.Cu_BillOfLadings.AsNoTracking()
                .Include(n => n.Consignments)
                .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .Include(n => n.Receiver)
                .Where(n => n.Id == id)
                .Select(n => new
                {
                    billId = n.Id,
                    reciver = n.Receiver.Name,
                    reciverphone = n.ReceiverPhone,
                    reciverAddress = n.ReceiverAddress,
                    number = n.WaybillNumber,
                    origin = n.Route.OriginCity.PersianName,
                    destination = n.Route.DestinationCity.PersianName,
                    parcelscount = n.Consignments.Count(),
                    weight = n.Consignments.Sum(n => n.Weight)
                }).FirstOrDefaultAsync();

            var billLabales = new List<WaybillLabelDto>();
            if (data != null && data.parcelscount > 0)
            {
                for (int i = 1; i <= data.parcelscount; i++)
                {
                    WaybillLabelDto label = new WaybillLabelDto();
                    label.BillId = data.billId;
                    label.ReciverName = data.reciver;
                    label.ReciverAddress = data.reciverAddress;
                    label.ReciverTel = data.reciverphone;
                    label.WaybillNimber = data.number;
                    label.OriginCity = data.origin;
                    label.Destination = data.destination;
                    label.TotalCountParcel = data.parcelscount;
                    label.ParcelNumber = i;
                    label.weight = data.weight.ToString("0.##");
                    billLabales.Add(label);
                }
            }
            else
            {
                WaybillLabelDto label = new WaybillLabelDto();
                label.BillId = data.billId;
                label.ReciverName = data.reciver;
                label.ReciverAddress = data.reciverAddress;
                label.ReciverTel = data.reciverphone;
                label.WaybillNimber = data.number;
                label.OriginCity = data.origin;
                label.Destination = data.destination;
                label.TotalCountParcel = 1;
                label.ParcelNumber = 1;
                label.weight = data.weight.ToString("0.##");
                billLabales.Add(label);
            }

            return billLabales;

        }

        //=============================================================== Parcel
        public async Task<clsResult> AddNewParcelAsync(ConsigmentDto dto)
        {

            clsResult result = new clsResult();

            var costItem = await _db.Cu_BillOfLadingCostItems.Where(n => n.SellerId == dto.SellerId).ToListAsync();
            if (costItem.Count == 0)
            {
                result.Message = "آیتم های هزینه برای مرسوله شناسایی نشد با مدیرسیستم تماس بگیرید";
                return result;
            }
            int? parcelCostId = costItem.Where(n => n.CostCode == "101").FirstOrDefault().Id;
            if (parcelCostId == null)
            {
                result.Message = "کد هزینه 101 مربوط به هزینه حمل بار در سیستم تعریف نشده است";
                return result;
            }
            int? packageCostId = costItem.Where(n => n.CostCode == "102").FirstOrDefault().Id;
            if (packageCostId == null && dto.PackagingCost > 0)
            {
                result.Message = "کد هزینه 102 مربوط به هزینه بسته بندی بار در سیستم تعریف نشده است";
                return result;
            }
            int? InsouranceCostId = costItem.Where(n => n.CostCode == "103").FirstOrDefault().Id;
            if (InsouranceCostId == null && dto.InsuranceCost > 0)
            {
                result.Message = "کد هزینه 103 مربوط به هزینه بیمه در سیستم تعریف نشده است";
                return result;
            }
            int? vatCostId = costItem.Where(n => n.CostCode == "105").FirstOrDefault().Id;
            if (vatCostId == null && dto.VatPrice > 0)
            {
                result.Message = "کد هزینه 105 مربوط به مبلغ ارزش افزوده در سیستم تعریف نشده است";
                return result;

            }
            int? otherCostId = costItem.Where(n => n.CostCode == "106").FirstOrDefault().Id;
            if (otherCostId == null && dto.VatPrice > 0)
            {
                result.Message = "کد هزینه 106 مربوط به سایر هزینه های بارنامه در سیستم تعریف نشده است";
                return result;

            }


            List<Cu_BillCost> costs = new List<Cu_BillCost>();

            // 101 Parcel Cost
            Cu_BillCost parcelCost = new Cu_BillCost();
            parcelCost.BillOfLadingId = dto.BillOfLadingId;
            parcelCost.ConsignmentId = dto.Id;
            parcelCost.SellerId = dto.SellerId;
            parcelCost.CostTypeId = parcelCostId.Value;
            parcelCost.Amount = dto.CargoFare;
            parcelCost.CreatedAt = DateTime.Now;
            costs.Add(parcelCost);

            // 102 
            if (dto.PackagingCost > 0)
            {
                Cu_BillCost packageCost = new Cu_BillCost();
                packageCost.BillOfLadingId = dto.BillOfLadingId;
                packageCost.ConsignmentId = dto.Id;
                packageCost.SellerId = dto.SellerId;
                packageCost.CostTypeId = packageCostId.Value;
                packageCost.Amount = dto.PackagingCost;
                packageCost.CreatedAt = DateTime.Now;
                costs.Add(packageCost);
            }
            // 103 
            if (dto.InsuranceCost > 0)
            {
                Cu_BillCost insuranceCost = new Cu_BillCost();
                insuranceCost.BillOfLadingId = dto.BillOfLadingId;
                insuranceCost.ConsignmentId = dto.Id;
                insuranceCost.SellerId = dto.SellerId;
                insuranceCost.CostTypeId = InsouranceCostId.Value;
                insuranceCost.Amount = dto.InsuranceCost;
                insuranceCost.CreatedAt = DateTime.Now;
                costs.Add(insuranceCost);
            }
            // 105 
            if (dto.VatPrice > 0)
            {
                Cu_BillCost vat = new Cu_BillCost();
                vat.BillOfLadingId = dto.BillOfLadingId;
                vat.ConsignmentId = dto.Id;
                vat.SellerId = dto.SellerId;
                vat.CostTypeId = vatCostId.Value;
                vat.Amount = dto.VatPrice;
                vat.CreatedAt = DateTime.Now;
                costs.Add(vat);
            }
            // 106 
            if (dto.OtherCost > 0)
            {
                Cu_BillCost otherCost = new Cu_BillCost();
                otherCost.BillOfLadingId = dto.BillOfLadingId;
                otherCost.ConsignmentId = dto.Id;
                otherCost.SellerId = dto.SellerId;
                otherCost.CostTypeId = otherCostId.Value;
                otherCost.Amount = dto.OtherCost;
                otherCost.CreatedAt = DateTime.Now;
                costs.Add(otherCost);
            }

            //..........................................................
            Cu_Consignment n = new Cu_Consignment();
            n.Id = dto.Id;
            n.BillOfLadingId = dto.BillOfLadingId;
            n.SellerId = dto.SellerId;
            n.Code = dto.Code;
            n.NatureTypeId = dto.NatureTypeId;
            n.Value = dto.Value;
            n.PackagetypeId = dto.PackageTypeId;
            n.ContentDescription = dto.ContentDescription;

            n.Volume = dto.Volume;
            n.Width = dto.Width;
            n.Length = dto.Length;
            n.Height = dto.Height;
            n.Weight = dto.Weight;

            n.CargoFare = dto.CargoFare;

            n.CargoFare = dto.CargoFare;
            n.Discount = dto.Discount;
            n.TotalCostPrice = costs.Sum(n => n.Amount);
            n.VatPrice = dto.VatPrice;
            n.VatRate = dto.VatRate;
            n.TotalPrice = n.TotalCostPrice - n.Discount;
            n.CreatedBy = dto.CreatedBy;

            _db.Cu_Consignments.Add(n);
            _db.Cu_BillCosts.AddRange(costs);
            try
            {
                await _db.SaveChangesAsync();

                result.Success = true;
                result.ShowMessage = false;
                result.updateType = 1;
            }
            catch (Exception x)
            {
                result.Message = "خطایی در قبول مرسوله رخ داده است";
                result.Message += $"\n {x.Message}";
            }
            return result;
        }
        public async Task<ConsigmentDto> GetParcelByIdAsync(Guid ParcelId)
        {
            var parcel = await _db.Cu_Consignments.AsNoTracking()
                .Include(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.NatureType)
                .SingleOrDefaultAsync(n => n.Id == ParcelId);
            if (parcel == null)
            {
                return null;
            }

            ConsigmentDto dto = new ConsigmentDto();
            dto.Id = parcel.Id;
            dto.BillOfLadingId = parcel.BillOfLadingId;
            dto.Code = parcel.Code;
            dto.NatureTypeId = parcel.NatureTypeId;
            dto.RecipientName = parcel.NatureType.Name;
            dto.ContentDescription = parcel.ContentDescription;
            dto.CargoFare = parcel.CargoFare;
            dto.PackageTypeId = parcel.PackagetypeId;
            dto.Length = parcel.Length;
            dto.Width = parcel.Width;
            dto.Height = parcel.Height;
            dto.Weight = parcel.Weight;
            dto.Volume = parcel.Volume;
            dto.Discount = parcel.Discount;
            dto.strDiscount = parcel.Discount.ToPrice();
            if (parcel.BillCosts != null && parcel.BillCosts.Count > 0)
            {
                foreach (var item in parcel.BillCosts)
                {
                    if (item.CostType != null)
                    {
                        if (item.CostType.CostCode == "101")
                        {
                            dto.strCargoFare = item.Amount.ToPrice();
                        }
                        else if (item.CostType.CostCode == "102")
                        {
                            dto.strPackagingCost = item.Amount.ToPrice();
                        }
                        else if (item.CostType.CostCode == "103")
                        {
                            dto.strInsuranceCost = item.Amount.ToPrice();
                        }
                        else if (item.CostType.CostCode == "105")
                        {
                            dto.strVatPrice = item.Amount.ToPrice();
                        }
                        else if (item.CostType.CostCode == "106")
                        {
                            dto.strOtherCost = item.Amount.ToPrice();
                        }
                    }
                }
            }
            return dto;
        }
        public async Task<clsResult> SaveParcelDeliveryAsync(ParcelDeliveryDto dto)
        {
            var bill = await _db.Cu_BillOfLadings.FindAsync(dto.BillOfLadingId);
            if (bill == null)
            {
                return new clsResult { Success = false, Message = "بارنامه مورد نظر یافت نشد" };
            }

            if (!string.IsNullOrEmpty(dto.SignatureImage))
            {
                var imageData = Regex.Match(dto.SignatureImage, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                byte[] signatureBytes = Convert.FromBase64String(imageData);
                bill.tg_SignatureData = signatureBytes;
            }


            bill.tg_Phone = dto.ReceiverMobile;
            bill.tg_Name = dto.ReceiverName;
            bill.tg_NationalityCode = dto.ReceiverNationalCode;
            bill.tg_DeliveryDate = dto.DeliveryDateTime;
            bill.tg_Description = dto.Description;

            bill.tg_CourierManUserName = dto.SenderUserName;
            bill.Delivered = true;
            bill.BillOfLadingStatusId = 11;

            _db.Cu_BillOfLadings.Update(bill);
            try
            {
                bool updateResult = Convert.ToBoolean(await _db.SaveChangesAsync());
                if (updateResult)
                {
                    if (await _tracking.SetBillOfLadingTrackingAsync(dto.BillOfLadingId, bill.WaybillNumber, dto.UserId, 11, "مرسوله تحویل گیرنده شد.", false, true))
                    {
                        return new clsResult { Success = true, Message = "تحویل مرسوله با موفقیت ثبت شد" };
                    }
                }
                else
                {
                    return new clsResult { Success = false, Message = "خطایی در ثبت تحویل مرسوله رخ داده است" };
                }

                return new clsResult { Success = true, Message = "تحویل مرسوله با موفقیت ثبت شد" };
            }
            catch (Exception ex)
            {
                return new clsResult { Success = false, Message = ex.Message };
            }
        }
        public async Task<clsResult> DeleteParcelAsync(Guid ParcelId, short? StatusId = null)
        {
            clsResult result = new clsResult();
            result.Success = false;
            result.ShowMessage = true;

            var parcel = await _db.Cu_Consignments.FindAsync(ParcelId);
            if (parcel == null)
            {
                result.Message = "اطلاعات مرسوله یافت نشد";
                return result;
            }

            var bill = await _db.Cu_BillOfLadings
                .Include(n => n.TreTransactions)
                .Include(n => n.Consignments)
                .Include(n => n.BillCosts)
                .SingleOrDefaultAsync(n => n.Id == parcel.BillOfLadingId);

            if (bill == null)
            {
                result.Message = "اطلاعات بارنامه مربوط به مرسوله یافت نشد";
                return result;
            }
            if (StatusId.HasValue)
            {
                if (StatusId > 2)
                {
                    result.Message = "صدور بارنامه نهایی شده و امکان حذف و یا ویرایش اطلاعات امکان پذیر نمی باشد.";
                    return result;
                }
            }

            if (bill.TreTransactions.Any())
            {
                result.Message = "بارنامه دارای اطلاعات مالی است جهت ویرایش یا حذف آن با واحد مالی ارتباط بگیرید";
                return result;
            }


            var transaction = await _db.Cu_FinancialTransactions.Where(n => n.BillOfLadingId == bill.Id && n.OperationId == 1).FirstOrDefaultAsync();
            var costData = await _db.Cu_BillCosts.Where(n => n.ConsignmentId == parcel.Id).ToListAsync();

            _db.Cu_BillCosts.RemoveRange(costData);
            _db.Cu_Consignments.Remove(parcel);

            //بروزرسانی قیمت بارنامه بعد از حذف مرسوله
            long totalCost = bill.BillCosts.Sum(c => c.Amount);
            long totaldiscount = bill.Consignments.Sum(n => n.Discount);
            long NewPrice = totalCost - totaldiscount;

            // اگر محاسبه مالی مرسوله در جدول تراکنش های مالی بارنامه درج شده باشد
            if (transaction != null)
            {
                if (NewPrice > 0)
                {
                    transaction.Amount = NewPrice;
                    _db.Cu_FinancialTransactions.Update(transaction);

                }
                else
                {
                    _db.Cu_FinancialTransactions.Remove(transaction);
                }
            }
            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "حذف مرسوله با موفقیت انجام شد";

            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "خطایی در حذف مرسوله رخ داده است";
            }

            return result
                ;

        }
        public async Task<clsResult> SendPaymentLinkAsync(Guid id, string trackingNumber, string reciver)
        {
            clsResult result = new clsResult();
            result.Success = false;

            if (id == Guid.Empty)
            {
                result.Message = "شناسه بارنامه معتبر نیست";
                return result;
            }

            if (string.IsNullOrEmpty(reciver))
            {
                result.Message = "شماره موبایل وارد نشده است";
                return result;
            }

            if (!Regex.IsMatch(reciver, "^09\\d{9}$"))
            {
                result.Message = "شماره موبایل وارد شده معتبر نیست";
                return result;
            }

            try
            {
                string message = _smsService.GenerateSendPaymentLinkMessage(id, trackingNumber);
                var send = await _smsSender.SendSmsAsync(reciver, message);
                result.Success = true;
                result.Message = "لینک پرداخت با موفقیت ارسال شد";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطایی در ارسال لینک پرداخت رخ داده است";
                // جهت عیب‌یابی بهتر می‌توانید خطا را لاگ کنید:
                // _logger.LogError(ex, "Error sending payment link for id {Id}", id);
            }

            return result;
        }
        public async Task<clsResult> ChangeSenderOrReciverAsync(ChangeSenderOrReciverDto dto)
        {

            clsResult result = new clsResult();
            result.Success = false;

            var bill = await _db.Cu_BillOfLadings
               .Include(n => n.Sender)
               .Include(n => n.Receiver)
               .SingleOrDefaultAsync(n => n.Id == dto.BillId);

            if (bill == null)
            {
                result.Message = "اطلاعات بارنامه یافت نشد";
                return result;
            }
            if (bill.BillOfLadingStatusId >= 11)
            {
                result.Message = "بارنامه از چرخه عملیاتی خارج شده و امکان تغییرات وجود ندارد";
                return result;
            }

            //Update Billway
            if (dto.IsSender)
            {
                bill.SenderId = dto.PersonId;
                bill.SenderPhone = dto.Phone;
                bill.SenderAddress = dto.Address;
            }
            else
            {
                bill.ReceiverId = dto.PersonId;
                bill.ReceiverPhone = dto.Phone;
                bill.ReceiverAddress = dto.Address;
            }
            _db.Cu_BillOfLadings.Update(bill);

            try
            {
                await _db.SaveChangesAsync();

                string description = $"";
                if (dto.IsSender)
                    description = $"اطلاعات فرستنده ویرایش شد";
                else
                    description = $"اطلاعات گیرنده ویرایش شد";

                //======== Tracking
                if (bill.BillOfLadingStatusId > 2)
                {
                    var tracking = await _tracking.SetBillOfLadingTrackingAsync(bill.Id, bill.WaybillNumber, dto.UserId, 3, description, true, true);
                }
                result.Success = true;
                result.Message = description;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطایی در بروزرسانی اطلاعات رخ داده است";

            }

            return result;
        }
        public async Task<ChangeSenderOrReciverDto> GetSenderOrResiverData(Guid billId, bool isSender)
        {
            ChangeSenderOrReciverDto data = new ChangeSenderOrReciverDto();

            var bill = await _db.Cu_BillOfLadings.FindAsync(billId);
            if (bill == null) return null;

            data.BillId = bill.Id;
            data.WaybillNumber = bill.WaybillNumber;
            if (isSender)
            {
                data.IsSender = true;
                data.PersonId = bill.SenderId;
                data.Phone = bill.SenderPhone;
                data.Address = bill.SenderAddress;
            }
            else
            {
                data.IsSender = false;
                data.PersonId = bill.ReceiverId;
                data.Phone = bill.ReceiverPhone;
                data.Address = bill.ReceiverAddress;
            }
            return data;
        }
        public async Task<clsResult> UpdateParcelAsync(ConsigmentDto dto)
        {

            clsResult result = new clsResult();

            var parcel = await _db.Cu_Consignments.Include(n => n.BillCosts).SingleOrDefaultAsync(n => n.Id == dto.Id);
            var parcelCosts = await _db.Cu_BillCosts.Where(n => n.ConsignmentId == dto.Id).ToListAsync();
            //=================
            var costItem = await _db.Cu_BillOfLadingCostItems.Where(n => n.SellerId == dto.SellerId).ToListAsync();
            if (costItem.Count == 0)
            {
                result.Message = "آیتم های هزینه برای مرسوله شناسایی نشد با مدیرسیستم تماس بگیرید";
                return result;
            }
            int? parcelCostId = costItem.Where(n => n.CostCode == "101").FirstOrDefault().Id;
            if (parcelCostId == null)
            {
                result.Message = "کد هزینه 101 مربوط به هزینه حمل بار در سیستم تعریف نشده است";
                return result;
            }
            int? packageCostId = costItem.Where(n => n.CostCode == "102").FirstOrDefault().Id;
            if (packageCostId == null && dto.PackagingCost > 0)
            {
                result.Message = "کد هزینه 102 مربوط به هزینه بسته بندی بار در سیستم تعریف نشده است";
                return result;
            }
            int? InsouranceCostId = costItem.Where(n => n.CostCode == "103").FirstOrDefault().Id;
            if (InsouranceCostId == null && dto.InsuranceCost > 0)
            {
                result.Message = "کد هزینه 103 مربوط به هزینه بیمه در سیستم تعریف نشده است";
                return result;
            }
            int? vatCostId = costItem.Where(n => n.CostCode == "105").FirstOrDefault().Id;
            if (vatCostId == null && dto.VatPrice > 0)
            {
                result.Message = "کد هزینه 105 مربوط به مبلغ ارزش افز.ده در سیستم تعریف نشده است";
                return result;

            }
            int? otherCostId = costItem.Where(n => n.CostCode == "106").FirstOrDefault().Id;
            if (otherCostId == null && dto.VatPrice > 0)
            {
                result.Message = "کد هزینه 106 مربوط به سایر هزینه های بارنامه در سیستم تعریف نشده است";
                return result;

            }


            List<Cu_BillCost> costs = new List<Cu_BillCost>();


            // 101 Parcel Cost
            Cu_BillCost parcelCost = await _db.Cu_BillCosts.Where(n => n.CostTypeId == parcelCostId.Value && n.ConsignmentId == dto.Id).FirstOrDefaultAsync();
            if (parcelCost != null)
            {
                parcelCost.Amount = dto.CargoFare;
                _db.Cu_BillCosts.Update(parcelCost);
            }
            else
            {
                parcelCost = new Cu_BillCost();
                parcelCost.BillOfLadingId = dto.BillOfLadingId;
                parcelCost.ConsignmentId = dto.Id;
                parcelCost.SellerId = dto.SellerId;
                parcelCost.CostTypeId = parcelCostId.Value;
                parcelCost.Amount = dto.CargoFare;
                parcelCost.CreatedAt = DateTime.Now;
                costs.Add(parcelCost);
            }

            // 102 
            Cu_BillCost packageCost = await _db.Cu_BillCosts.Where(n => n.CostTypeId == packageCostId.Value && n.ConsignmentId == dto.Id).FirstOrDefaultAsync();
            if (dto.PackagingCost > 0)
            {
                if (packageCost != null)
                {
                    packageCost.Amount = dto.PackagingCost;
                    _db.Cu_BillCosts.Update(parcelCost);
                }
                else
                {
                    packageCost = new Cu_BillCost();
                    packageCost.BillOfLadingId = dto.BillOfLadingId;
                    packageCost.ConsignmentId = dto.Id;
                    packageCost.SellerId = dto.SellerId;
                    packageCost.CostTypeId = packageCostId.Value;
                    packageCost.Amount = dto.PackagingCost;
                    packageCost.CreatedAt = DateTime.Now;
                    costs.Add(packageCost);
                }

            }
            else
            {
                if (packageCost != null)
                {
                    _db.Cu_BillCosts.Remove(packageCost);
                }
            }


            // 103 
            Cu_BillCost insuranceCost = await _db.Cu_BillCosts.Where(n => n.CostTypeId == InsouranceCostId.Value && n.ConsignmentId == dto.Id).FirstOrDefaultAsync();
            if (dto.InsuranceCost > 0)
            {
                if (insuranceCost != null)
                {
                    insuranceCost.Amount = dto.InsuranceCost;
                    _db.Cu_BillCosts.Update(insuranceCost);
                }
                else
                {
                    insuranceCost = new Cu_BillCost();
                    insuranceCost.BillOfLadingId = dto.BillOfLadingId;
                    insuranceCost.ConsignmentId = dto.Id;
                    insuranceCost.SellerId = dto.SellerId;
                    insuranceCost.CostTypeId = InsouranceCostId.Value;
                    insuranceCost.Amount = dto.InsuranceCost;
                    insuranceCost.CreatedAt = DateTime.Now;
                    costs.Add(insuranceCost);
                }

            }
            else
            {
                if (insuranceCost != null)
                {
                    _db.Cu_BillCosts.Remove(insuranceCost);
                }
            }


            // 105 
            Cu_BillCost vat = await _db.Cu_BillCosts.Where(n => n.CostTypeId == vatCostId.Value && n.ConsignmentId == dto.Id).FirstOrDefaultAsync();
            if (dto.VatPrice > 0)
            {
                if (vat != null)
                {
                    vat.Amount = dto.VatPrice;
                    _db.Cu_BillCosts.Update(vat);
                }
                else
                {
                    vat = new Cu_BillCost();
                    vat.BillOfLadingId = dto.BillOfLadingId;
                    vat.ConsignmentId = dto.Id;
                    vat.SellerId = dto.SellerId;
                    vat.CostTypeId = vatCostId.Value;
                    vat.Amount = dto.VatPrice;
                    vat.CreatedAt = DateTime.Now;
                    costs.Add(vat);
                }
            }
            else
            {
                if (vat != null)
                {
                    _db.Cu_BillCosts.Remove(vat);
                }
            }


            // 106 
            Cu_BillCost otherCost = await _db.Cu_BillCosts.Where(n => n.CostTypeId == otherCostId.Value && n.ConsignmentId == dto.Id).FirstOrDefaultAsync();
            if (dto.OtherCost > 0)
            {
                if (otherCost != null)
                {
                    otherCost.Amount = dto.OtherCost;
                    _db.Cu_BillCosts.Update(otherCost);
                }
                else
                {
                    otherCost = new Cu_BillCost();
                    otherCost.BillOfLadingId = dto.BillOfLadingId;
                    otherCost.ConsignmentId = dto.Id;
                    otherCost.SellerId = dto.SellerId;
                    otherCost.CostTypeId = otherCostId.Value;
                    otherCost.Amount = dto.OtherCost;
                    otherCost.CreatedAt = DateTime.Now;
                    costs.Add(otherCost);
                }

            }
            else
            {
                if (otherCost != null)
                {
                    _db.Cu_BillCosts.Remove(otherCost);
                }
            }

            //..........................................................

            parcel.Code = dto.Code;
            parcel.NatureTypeId = dto.NatureTypeId;
            parcel.Value = dto.Value;
            parcel.PackagetypeId = dto.PackageTypeId;
            parcel.ContentDescription = dto.ContentDescription;

            parcel.Volume = dto.Volume;
            parcel.Width = dto.Width;
            parcel.Length = dto.Length;
            parcel.Height = dto.Height;
            parcel.Weight = dto.Weight;

            parcel.CargoFare = dto.CargoFare;

            parcel.CargoFare = dto.CargoFare;
            parcel.Discount = dto.Discount;
            parcel.TotalCostPrice = costs.Sum(n => n.Amount);
            parcel.VatPrice = dto.VatPrice;
            parcel.VatRate = dto.VatRate;
            parcel.TotalPrice = parcel.TotalCostPrice - parcel.Discount;


            _db.Cu_Consignments.Update(parcel);
            _db.Cu_BillCosts.AddRange(costs);
            try
            {
                var finance = await _db.Cu_FinancialTransactions
                    .Include(n => n.MoneyTransactions).Where(n => n.BillOfLadingId == dto.BillOfLadingId && n.OperationId == 6).FirstOrDefaultAsync();

                if (finance != null)
                {
                    var bill = await _db.Cu_BillOfLadings.Include(n => n.Consignments).Include(n => n.BillCosts).FirstOrDefaultAsync();
                    finance.Amount = bill.BillCosts.Sum(n => n.Amount) - bill.Consignments.Sum(n => n.Discount);
                    long paymentsAmount = finance.MoneyTransactions.Sum(n => n.Amount);
                    if (finance.Amount > paymentsAmount)
                        finance.IsSettled = false;

                    _db.Cu_FinancialTransactions.Update(finance);
                }
                await _db.SaveChangesAsync();

                result.Success = true;
                result.ShowMessage = false;
                result.updateType = 1;
            }
            catch (Exception x)
            {
                result.Message = "خطایی در ویرایش مرسوله رخ داده است";
                result.Message += $"\n {x.Message}";
            }
            return result;
        }

        //==================================================================== 
        public async Task<WaybillDitributerUpdateDto> GetWaybillDistributerByIdAsync(Guid billId)
        {
            var waybill = await _db.Cu_BillOfLadings.SingleOrDefaultAsync(n => n.Id == billId);
            if (waybill == null) return null;

            return new WaybillDitributerUpdateDto
            {
                Id = waybill.Id,
                DistributerId = waybill.DistributerBranchId,
                WaybillNumber = waybill.WaybillNumber,
            };
        }

        public async Task<clsResult> SetWaybillDistributerAsync(WaybillDitributerUpdateDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            var waybill = await _db.Cu_BillOfLadings.FindAsync(dto.Id);
            if (waybill == null)
            {
                result.Message = "بارنامه یافت نشد";
                return result;
            }

            waybill.DistributerBranchId = dto.DistributerId;
            _db.Cu_BillOfLadings.Update(waybill);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطایی در بروزرسانی اطلاعات رخ داده است";
            }

            return result;

        }
        public async Task<WayBillsStatusCheckDto> getDashboardDataAsync(BillOfLadingFilterDto filter, int destinationCityId)
        {
            var waybillsStatus = _db.Cu_BillOfLadings.AsNoTracking()
                .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted && n.OriginBranchId == filter.OriginBranchId)
                .Select(n => new { issuingBranch = n.OriginBranchId, status = n.BillOfLadingStatusId }).AsQueryable();

            int pendingDestributions = await _db.Cu_BillOfLadings.AsNoTracking().Include(n => n.Route)
                 .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted
                 && (n.BillOfLadingStatusId >= 2 && n.BillOfLadingStatusId < 11)
                 && (filter.branchIsOwner ? (n.DistributerBranchId == filter.DestinationBranchId || n.DistributerBranchId == null) : n.DistributerBranchId == filter.DestinationBranchId))
                 .CountAsync();

            int noSetteled = await _db.Cu_FinancialTransactions.AsNoTracking().Include(n => n.BillOfLading).ThenInclude(n => n.Route)
                .Where(n =>
                       (n.SellerId == filter.SellerId && !n.IsDeleted && !n.IsSettled)
                       && (n.BillOfLading.BillOfLadingStatusId == 11)
                       && ((n.BillOfLading.OriginBranchId == filter.OriginBranchId && n.SettlementTypeId == 1)
                       || (n.BillOfLading.Route.DestinationCityId == destinationCityId && n.SettlementTypeId == 2))
                ).Select(n => n.Id).CountAsync();

            WayBillsStatusCheckDto model = new WayBillsStatusCheckDto();
            model.IssuingWaybillsCount = await waybillsStatus.Where(n => n.issuingBranch == filter.OriginBranchId && n.status <= 2).CountAsync();
            model.AwaitingCollectionWaybillsCount = await waybillsStatus.Where(n => n.issuingBranch == filter.OriginBranchId && n.status == 3).CountAsync();
            model.ReadyForDistributionWaybillsCount = pendingDestributions;
            model.NoSetteledCount = noSetteled;

            return model;
        }

        public async Task<clsResult> ChangeSettelmentAsync(ChangeSettelmentDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            var waybill = await _db.Cu_BillOfLadings.FindAsync(dto.BillId);
            if (waybill == null)
            {
                result.Message = "بارنامه یافت نشد";
                return result;
            }
            var financialRecord = await _db.Cu_FinancialTransactions.FirstOrDefaultAsync(n => n.BillOfLadingId == dto.BillId && n.OperationId == 1);
            if (financialRecord == null)
            {
                result.Message = "بارنامه مورنظر درحال صدور می باشد و امکان تغییر یا تعیین نوع تسویه حساب از این طریق وجود ندارد";
                return result;
            }
            if (dto.settelmentTypeId == 3 && dto.PartyId == 0)
            {
                result.Message = "برای ثبت تسویه حساب اعتباری انتخاب طرف حساب الزامی می باشد.";
                return result;
            }

            waybill.SettelmentType = dto.settelmentTypeId;
            _db.Cu_BillOfLadings.Update(waybill);

            financialRecord.SettlementTypeId = dto.settelmentTypeId;
            if (dto.settelmentTypeId == 3)
            {
                financialRecord.AccountPartyId = dto.PartyId;
            }
            _db.Cu_FinancialTransactions.Update(financialRecord);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطایی در بروزرسانی اطلاعات رخ داده است";
            }

            return result;
        }

        public async Task<clsResult> ChangeDestributerBulkAsync(ChangeDestribiuterDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;


            try
            {
                int update = await _db.Cu_BillOfLadings.Where(n => dto.BillsId.Contains(n.Id))
                .ExecuteUpdateAsync(n => n.SetProperty(p => p.DistributerBranchId, dto.BranchId));
                if (update > 0)
                {
                    result.Success = true;
                    result.Message = $"عملیات اختصاص نماینده با موفقیت انجام شد  <br> تعداد {update} روکورد بروزرسانی شد";
                }
                else
                {
                    result.Success = false;
                    result.Message = "رکوردی جهت بروزرسانی یافت نشد";
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطایی در بروزرسانی اطلاعات رخ داده است";
            }

            return result;
        }


        //========== Distribution Bills ========================================================== 

        public async Task<clsResult> CreateDistributionBillAsync(AddDistriutionDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;

            if (dto == null)
            {
                result.Message = "اطلاعاتی جهت صدور دریافت نشد";
                return result;
            }
            DateTime issuanceDate = DateTime.Now;
            try
            {
                issuanceDate = dto.strIssuanceDate.PersianToLatin();
            }
            catch
            {
                result.Message = "تاریخ بارنامه معتبر نیست";
                return result;
            }

            string billNumber = await GenerateBillNumberAsync(dto.SellerId, "9990");
            result.Success = false;
            result.ShowMessage = true;

            // ------------------------------------------------------ Create Header
            // ------------------------------------------------------ Create Header
            // ------------------------------------------------------ Create Header
            Cu_BillOfLading bill = new Cu_BillOfLading();
            bill.Id = Guid.NewGuid();
            bill.SellerId = dto.SellerId;
            bill.OriginHubId = dto.OriginHubId;
            bill.OriginBranchId = dto.OriginBranchId;
            bill.RouteId = dto.RouteId.Value;
            bill.ServiceId = dto.ServiceId;

            bill.WayBillType = 2;
            bill.WaybillNumber = billNumber;
            bill.IssuanceDate = issuanceDate;
            bill.ReferenceCode = dto.ReferenceCode;
            bill.BillOfLadingStatusId = 7;

            bill.SenderId = dto.SenderId;
            bill.SenderAddress = dto.SenderAddress;
            bill.SenderPhone = dto.SenderPhone;
            bill.ReceiverId = dto.ReceiverId;
            bill.ReceiverAddress = dto.ReceiverAddress;
            bill.ReceiverPhone = dto.ReciverPhone;
            bill.Description = dto.Description;
            bill.BusinessPartnerId = dto.BusinessPartnerId;

            // bill.BillOfLadingStatusId = dto.BillOfLadingStatusId;
            bill.LastStatusDescription = dto.LastStatusDescription;
            bill.CreatedBy = dto.CreatedBy;

            bill.DistributerBranchId = dto.DistributerRepresentativeId;
            if (dto.DistributerRepresentativeId == Guid.Empty)
                bill.DistributerBranchId = null;

            _db.Cu_BillOfLadings.Add(bill);


            //----------------------------------------------------- Add Parcel
            //----------------------------------------------------- Add Parcel
            //----------------------------------------------------- Add Parcel
            Cu_Consignment parcel = new Cu_Consignment();
            parcel.Id = Guid.NewGuid();
            parcel.BillOfLadingId = bill.Id;
            parcel.SellerId = dto.SenderId;

            parcel.Code = "9990";
            parcel.CreatedAt = DateTime.Now;
            parcel.CreatedAtTime = new TimeSpan(parcel.CreatedAt.Hour, parcel.CreatedAt.Minute, parcel.CreatedAt.Second);
            parcel.CreatedBy = dto.CreatedBy;

            parcel.ContentDescription = dto.Description;
            parcel.NatureTypeId = dto.NatureId;
            parcel.Weight = dto.Weight;
            var dimin = ParseDimensions(dto.Dimensions);
            parcel.Height = dimin.Height;
            parcel.Width = dimin.Width;
            parcel.Length = dimin.Length;
            parcel.Value = 0;

            parcel.CargoFare = dto.BillPrice;
            parcel.TotalCostPrice = dto.SharePrice;
            parcel.TotalPrice = dto.BillPrice + dto.SharePrice;
            parcel.Discount = 0;

            _db.Cu_Consignments.Add(parcel);

            // ------------------------------------------------------ Add Cost
            // ------------------------------------------------------ Add Cost
            // ------------------------------------------------------ Add Cost
            List<Cu_BillCost> costs = new List<Cu_BillCost>();
            var costItem = await _db.Cu_BillOfLadingCostItems.Where(n => n.SellerId == dto.SellerId).ToListAsync();
            if (costItem.Count == 0)
            {
                result.Message = "آیتم های هزینه برای مرسوله شناسایی نشد با مدیرسیستم تماس بگیرید";
                return result;
            }

            int? parcelCostId = costItem.Where(n => n.CostCode == "101").FirstOrDefault().Id;
            if (parcelCostId == null)
            {
                result.Message = "کد هزینه 101 مربوط به هزینه حمل بار در سیستم تعریف نشده است";
                return result;
            }
            //--
            Cu_BillCost parcelCost = new Cu_BillCost();
            parcelCost.BillOfLadingId = bill.Id;
            parcelCost.ConsignmentId = parcel.Id;
            parcelCost.SellerId = dto.SellerId;
            parcelCost.CostTypeId = parcelCostId.Value;
            parcelCost.Amount = dto.BillPrice;
            parcelCost.CreatedAt = DateTime.Now;
            costs.Add(parcelCost);
            //--
            int? DestinationCostId = costItem.Where(n => n.CostCode == "107").FirstOrDefault().Id;
            if (DestinationCostId == null)
            {
                result.Message = "کد هزینه 107 مربوط به سهم نماینده از توزیع سیستم تعریف نشده است";
                return result;
            }
            //--
            Cu_BillCost DestinationCost = new Cu_BillCost();
            DestinationCost.BillOfLadingId = bill.Id;
            DestinationCost.ConsignmentId = parcel.Id;
            DestinationCost.SellerId = dto.SellerId;
            DestinationCost.CostTypeId = DestinationCostId.Value;
            DestinationCost.Amount = dto.SharePrice;
            DestinationCost.CreatedAt = DateTime.Now;
            costs.Add(DestinationCost);

            // 9990 Extra Cost 
            if (dto.OtherCost > 0)
            {
                int? otherCostId = costItem.Where(n => n.CostCode == "9990").FirstOrDefault().Id;
                if (otherCostId == null)
                {
                    result.Message = "کد هزینه 9990 مربوط به هزینه خدمات توزیع خارج از بارنامه در سیستم تعریف نشده است";
                    return result;
                }

                Cu_BillCost otherCost = new Cu_BillCost();
                otherCost.BillOfLadingId = bill.Id;
                otherCost.ConsignmentId = null;
                otherCost.SellerId = dto.SellerId;
                otherCost.CostTypeId = otherCostId.Value;
                otherCost.Amount = dto.OtherCost;
                otherCost.CreatedAt = DateTime.Now;
                costs.Add(otherCost);
            }

            _db.Cu_BillCosts.AddRange(costs);

            //-----------------------------------------------------
            //----------------------------------------------------- Set Financial Records
            //----------------------------------------------------- 

            List<Cu_FinancialTransaction> financialTransactions = new List<Cu_FinancialTransaction>();
            if (dto.SettelmentType == 1)
            {
                // ثبت بدهکاری برای صادر کننده بارنامه به مبلغ سهم توزیع
                Cu_FinancialTransaction t20 = new Cu_FinancialTransaction();
                t20.OperationId = 5;
                t20.SellerId = bill.SellerId;
                t20.BillOfLadingId = bill.Id;
                t20.AccountPartyId = dto.BusinessPartyPersonId.Value;
                t20.SettlementTypeId = 3;
                t20.TransactionDate = issuanceDate;
                t20.TransactionTime = new TimeSpan(issuanceDate.Hour, issuanceDate.Minute, issuanceDate.Second);
                t20.Description = $"ثبت بدهکاری بابت سهم توزیع از بارنامه نقدی شماره {dto.ReferenceCode} به تاریخ {issuanceDate.LatinToPersian()}";
                t20.UserId = dto.userId;
                t20.Amount = dto.SharePrice;
                t20.Bed = dto.SharePrice;
                financialTransactions.Add(t20);

                if (dto.OtherCost > 0)
                {
                    Cu_FinancialTransaction t1 = new Cu_FinancialTransaction();
                    t1.OperationId = 1;
                    t1.SellerId = bill.SellerId;
                    t1.BillOfLadingId = bill.Id;
                    t1.AccountPartyId = dto.PartyId.Value;
                    t1.SettlementTypeId = (int)dto.SettelmentType;
                    t1.TransactionDate = issuanceDate;
                    t1.TransactionTime = new TimeSpan(issuanceDate.Hour, issuanceDate.Minute, issuanceDate.Second);
                    t1.Description = $"بابت هزینه پیک و خدمات مبدأ بارنامه {dto.ReferenceCode} به تاریخ {issuanceDate.LatinToPersian()}";
                    t1.UserId = dto.userId;
                    t1.Amount = dto.OtherCost;
                    t1.Bed = dto.OtherCost;
                    financialTransactions.Add(t1);
                }
            }
            else
            {
                // ثبت بستانکاری برای صادر کننده بارنامه به مبلغ هزینه حمل بارنامه
                Cu_FinancialTransaction t20 = new Cu_FinancialTransaction();
                t20.OperationId = 5;
                t20.SellerId = bill.SellerId;
                t20.BillOfLadingId = bill.Id;
                t20.AccountPartyId = dto.BusinessPartyPersonId.Value;
                t20.SettlementTypeId = 3;
                t20.TransactionDate = issuanceDate;
                t20.TransactionTime = new TimeSpan(issuanceDate.Hour, issuanceDate.Minute, issuanceDate.Second);
                t20.Description = $"ثبت بستانکاری بابت مبلغ بارنامه پسکرایه به شماره {dto.ReferenceCode}  تاریخ {issuanceDate.LatinToPersian()}";
                t20.UserId = dto.userId;
                t20.Amount = dto.BillPrice;
                t20.Bes = dto.BillPrice;
                financialTransactions.Add(t20);


                Cu_FinancialTransaction t1 = new Cu_FinancialTransaction();
                t1.OperationId = 1;
                t1.SellerId = bill.SellerId;
                t1.BillOfLadingId = bill.Id;
                t1.AccountPartyId = dto.PartyId.Value;
                t1.SettlementTypeId = (int)dto.SettelmentType;
                t1.TransactionDate = issuanceDate;
                t1.TransactionTime = new TimeSpan(issuanceDate.Hour, issuanceDate.Minute, issuanceDate.Second);
                t1.Description = $"بابت بارنامه {dto.ReferenceCode} به تاریخ {issuanceDate.LatinToPersian()}";
                t1.UserId = dto.userId;
                t1.Amount = dto.TotalPrice;
                t1.Bed = dto.TotalPrice;
                financialTransactions.Add(t1);
            }

            if (!financialTransactions.Any())
            {
                result.Success = false;
                result.Message = "مشکلی در ثبت اطلاعات مالی بارنامه رخ داده است، مجددا تلاش کنید";
                return result;
            }
            _db.Cu_FinancialTransactions.AddRange(financialTransactions);

            try
            {
                await _db.SaveChangesAsync();

                result.Success = true;
                result.Message = "بارنامه با موفقیت ثبت شد";
            }
            catch (Exception x)
            {
                result.Message = "در عملیات صدور بارنامه خطایی رخ داده است";
            }
            return result;
        }


        public class Dimensions
        {
            public float Length { get; set; }  // طول
            public float Width { get; set; }   // عرض
            public float Height { get; set; }  // ارتفاع
            public float Volume { get; set; }  // حجم
        }

        public Dimensions ParseDimensions(string input)
        {
            var dimensions = new Dimensions();

            if (string.IsNullOrWhiteSpace(input))
                return dimensions;

            var parts = input.Split('-').Take(3).ToArray();

            // استفاده از متغیرهای موقت برای ذخیره مقادیر
            float tempLength = 0;
            float tempWidth = 0;
            float tempHeight = 0;

            if (parts.Length > 0) float.TryParse(parts[0], out tempLength);
            if (parts.Length > 1) float.TryParse(parts[1], out tempWidth);
            if (parts.Length > 2) float.TryParse(parts[2], out tempHeight);

            // اختصاص مقادیر موقت به پراپرتی‌ها
            dimensions.Length = tempLength;
            dimensions.Width = tempWidth;
            dimensions.Height = tempHeight;

            dimensions.Volume = (dimensions.Length * dimensions.Width * dimensions.Height) / 6000;

            return dimensions;
        }

    }
}
