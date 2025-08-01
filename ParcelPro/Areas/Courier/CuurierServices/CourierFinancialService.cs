using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Accounting.Dto.SaleManagementDtos;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Dto.FinancialDtos;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Interfaces;
using ParcelPro.Models;
using ParcelPro.Services;

namespace ParcelPro.Areas.Courier.CuurierServices
{
    public class CourierFinancialService : ICourierFinancialService
    {

        private readonly AppDbContext _db;
        private readonly UserContextService _userService;
        private readonly ITrachkingService _traking;
        private readonly ISMSService _sms;
        private readonly SmsSenderPersiaFava _smsSenter;

        public CourierFinancialService(AppDbContext dbContext,
            UserContextService userContextService,
            ITrachkingService TrackingService,
            ISMSService sms,
            SmsSenderPersiaFava smsSenter
            )
        {
            _db = dbContext;
            _userService = userContextService;
            _traking = TrackingService;
            _sms = sms;
            _smsSenter = smsSenter;
        }

        public string GenerateTrackingLink(Guid billId)
        {
            string trackingLink = $"https://hub.keyhanpost.ir/Courier/Billoflading/Bill?billOfladingId={billId}";
            return trackingLink;
        }
        public async Task<SelectList> SelectList_CreditCustomersAsync(long SellerId)
        {
            var CreditCustomers = await _db.Cu_FinancialTransactions.Include(n => n.Party)
                .Where(n => n.SellerId == SellerId && n.SettlementTypeId == 3).Select(n => new { Id = n.AccountPartyId, Name = n.Party.Name })
                .Distinct().OrderBy(n => n.Name).ToListAsync();

            return new SelectList(CreditCustomers, "Id", "Name");
        }


        public async Task<FinancialTransactionDto> GetBillDataAsync(Guid BillOdLadingId, int SettelmentType, string UserId)
        {
            var billOfLading = await _db.Cu_BillOfLadings
                .Include(n => n.Consignments)
                .Include(n => n.BillCosts)
                .Include(n => n.Sender).Include(n => n.Receiver).Include(b => b.IssuingBranch)
                .SingleOrDefaultAsync(b => b.Id == BillOdLadingId);

            if (billOfLading == null)
                return null;

            long discount = billOfLading.Consignments.Sum(c => c.Discount);
            long PriceBeforeDiscount = billOfLading.BillCosts.Sum(c => c.Amount);

            long partyId = billOfLading.SenderId;
            if (SettelmentType == 2)
                partyId = billOfLading.ReceiverId;

            var dto = new FinancialTransactionDto
            {
                OperationId = 1, // فروش بار
                SellerId = billOfLading.SellerId,
                BillOfLadingId = billOfLading.Id,
                AccountPartyId = partyId,
                SettlementTypeId = SettelmentType,
                Description = $"بابت تسویه حساب بارنامه {billOfLading.WaybillNumber}",
                BranchId = billOfLading.OriginBranchId,
                UserId = UserId,
                Amount = (PriceBeforeDiscount - discount),
                Bed = (PriceBeforeDiscount - discount),
                Bes = 0,
                BillNumber = billOfLading.WaybillNumber,
                SenderName = billOfLading.Sender.Name,
                PartyName = SettelmentType != 2 ? billOfLading.Sender.Name : billOfLading.Receiver.Name,
                BranchIsHub = billOfLading.IssuingBranch.IsHub.HasValue ? billOfLading.IssuingBranch.IsHub.Value : false,
            };
            return dto;
        }
        public async Task<bool> RegisterFinancialTransactionAsync(FinancialTransactionDto dto)
        {
            try
            {
                var transaction = new Cu_FinancialTransaction
                {
                    Id = Guid.NewGuid(),
                    OperationId = dto.OperationId,
                    SellerId = dto.SellerId,
                    BillOfLadingId = dto.BillOfLadingId,
                    AccountPartyId = dto.AccountPartyId,
                    SettlementTypeId = dto.SettlementTypeId,
                    TransactionDate = dto.TransactionDate,
                    TransactionTime = dto.TransactionTime,
                    Description = dto.Description,
                    BranchId = dto.BranchId,
                    UserId = dto.UserId,
                    Amount = dto.Amount,
                    Bed = dto.Bed,
                    Bes = dto.Bes,
                    IsDeleted = false
                };

                _db.Cu_FinancialTransactions.Add(transaction);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<clsResult> SetSettelmentAsync(FinancialTransactionDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;

            short ststusId = 2;
            if (dto.SettlementTypeId == 1)
                ststusId = 2;
            else if ((dto.SettlementTypeId == 2 || dto.SettlementTypeId == 3) && dto.BranchIsHub)
            {
                ststusId = 5;
            }
            else if ((dto.SettlementTypeId == 2 || dto.SettlementTypeId == 3) && !dto.BranchIsHub)
            {
                ststusId = 3;
            }

            var transaction = new Cu_FinancialTransaction
            {
                Id = Guid.NewGuid(),
                OperationId = dto.OperationId,
                SellerId = dto.SellerId,
                BillOfLadingId = dto.BillOfLadingId,
                AccountPartyId = dto.AccountPartyId,
                SettlementTypeId = dto.SettlementTypeId,
                TransactionDate = dto.TransactionDate,
                TransactionTime = dto.TransactionTime,
                Description = dto.Description,
                BranchId = dto.BranchId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                Bed = dto.Bed,
                Bes = dto.Bes,
                IsDeleted = false
            };


            var bill = await _db.Cu_BillOfLadings
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                .FirstOrDefaultAsync(n => n.Id == dto.BillOfLadingId.Value);
            if (bill == null)
            {
                result.Message = "مشکلی در شناسایی بارنامه رخ داده است. مجددا تلاش کنید";
                return result;
            }
            bill.BillOfLadingStatusId = ststusId;
            bill.SettelmentType = (short)dto.SettlementTypeId;

            _db.Cu_FinancialTransactions.Add(transaction);
            _db.Cu_BillOfLadings.Update(bill);

            try
            {
                await _db.SaveChangesAsync();

                if (ststusId > 2)
                {
                    // Send Sms
                    string senderNumber = bill.Sender.MobilePhone;
                    string reciverNumber = bill.Receiver.MobilePhone;
                    if (!string.IsNullOrEmpty(bill.SenderPhone) && bill.SenderPhone.IsMobile())
                        senderNumber = bill.SenderPhone;
                    if (!string.IsNullOrEmpty(bill.ReceiverPhone) && bill.ReceiverPhone.IsMobile())
                        reciverNumber = bill.ReceiverPhone;

                    string trackingLink = GenerateTrackingLink(bill.Id);
                    string senderMessage = _sms.GenerateSenderMessageAlt(bill.Sender.Name, bill.Receiver.Name, bill.Route.DestinationCity.PersianName, bill.WaybillNumber, trackingLink);
                    string reciverMessage = _sms.GenerateRecipientMessage(bill.Receiver.Name, bill.WaybillNumber, trackingLink);
                    var sendToSender = await _smsSenter.SendSmsAsync(senderNumber, senderMessage);
                    var sendToReciver = await _smsSenter.SendSmsAsync(reciverNumber, reciverMessage);

                }

                result.Success = true;
                result.Message = "ثبت تسویه حساب انجام شد";
                return result;
            }
            catch (Exception ex)
            {
                result.Message = "خطا در ثبت تسویه حساب";
                return result;
            }
        }
        // محاسبه میکند چ مبلغی از بارنامه پرداحت شده است. 
        public async Task<long> GetRemainingAmountByIdAsync(Guid billId)
        {
            var bill = await _db.Cu_BillOfLadings
                .Include(n => n.BillCosts)
                .Include(n => n.Consignments)
                .FirstOrDefaultAsync(n => n.Id == billId);
            if (bill == null)
                return 0;
            long discount = bill.Consignments.Sum(c => c.Discount);
            long PriceBeforeDiscount = bill.BillCosts.Sum(c => c.Amount);
            long billFinalPrice = PriceBeforeDiscount - discount;

            var BillSettelmentRecord = await _db.Cu_FinancialTransactions
                .Where(n => n.BillOfLadingId == billId && n.OperationId == 1).FirstOrDefaultAsync();
            if (BillSettelmentRecord == null)
                return 0;

            long payedAmount = await _db.TreTransactions.Where(n => n.BillFinancialtransactionId == BillSettelmentRecord.Id).SumAsync(n => n.CreditAmount);
            return billFinalPrice - payedAmount;
        }
        public async Task<long> GetRemainingAmountByNumAsync(string billNumber)
        {
            var bill = await _db.Cu_BillOfLadings
                .Include(n => n.BillCosts)
                .Include(n => n.Consignments)
                .FirstOrDefaultAsync(n => n.WaybillNumber == billNumber);
            if (bill == null)
                return 0;
            long discount = bill.Consignments.Sum(c => c.Discount);
            long PriceBeforeDiscount = bill.BillCosts.Sum(c => c.Amount);
            long billFinalPrice = PriceBeforeDiscount - discount;

            var BillSettelmentRecord = await _db.Cu_FinancialTransactions
                .Where(n => n.BillOfLadingId == bill.Id && n.OperationId == 1).FirstOrDefaultAsync();
            if (BillSettelmentRecord == null)
                return 0;

            long payedAmount = await _db.TreTransactions.Where(n => n.BillFinancialtransactionId == BillSettelmentRecord.Id).SumAsync(n => n.CreditAmount);
            return billFinalPrice - payedAmount;
        }
        public async Task<List<Sale_MoneyTransactionDto>> SaleMoneyTransactionAsync(TransactionFilterDto filter)
        {
            var query = _db.TreTransactions.AsNoTracking()
                .Include(n => n.Party)
                .Include(n => n.BankAccount)
                .Include(n => n.Pos)
                .Where(n => n.SellerId == filter.SellerId).AsQueryable();

            if (!string.IsNullOrEmpty(filter.strStartDate))
            {
                DateTime startDate = filter.strStartDate.PersianToLatin();
                query = query.Where(n => n.TransactionDate >= startDate);
            }
            if (!string.IsNullOrEmpty(filter.strEndDate))
            {
                DateTime endDate = filter.strEndDate.PersianToLatin();
                query = query.Where(n => n.TransactionDate <= endDate);
            }

            if (filter.PartyId.HasValue)
                query = query.Where(n => n.AccountPartyId == filter.PartyId);
            if (filter.BranchId.HasValue)
                query = query.Where(n => n.BranchId == filter.BranchId);
            if (filter.BankAccountId.HasValue)
                query = query.Where(n => n.BankAccountId == filter.BankAccountId);
            if (filter.PosId.HasValue)
                query = query.Where(n => n.PosId == filter.PosId);
            if (!string.IsNullOrEmpty(filter.BillNumber))
                query = query.Where(n => n.BillNumber.EndsWith(filter.BillNumber));

            var lst = await query.Select(n => new Sale_MoneyTransactionDto
            {
                BillNumber = n.BillNumber,
                Amount = n.Amount,
                AccountPartyId = n.AccountPartyId,
                BankAccount = n.BankAccount.AccountName ?? "",
                BankAccountId = n.BankAccountId,
                PosId = n.PosId,
                POS = n.Pos.Name ?? "",
                OperationId = n.OperationId,
                OperationName = n.Operation.OperationName,

            }).ToListAsync();

            return lst;

        }
        public IQueryable<AccBillViewModel> GetBillsFinanceAsQuery(AccSalesFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()

                .Include(n => n.IssuingBranch)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.FinancialTransactions).ThenInclude(n => n.MoneyTransactions)
                 .Include(n => n.FinancialTransactions).ThenInclude(n => n.Party)
                 .Include(n => n.TreTransactions)
                 .Include(n => n.Route).ThenInclude(n => n.OriginCity)
                 .Include(n => n.Route).ThenInclude(n => n.DestinationCity)
                 .Include(n => n.Sender)
                 .Include(n => n.Receiver)
                .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted && n.FinancialTransactions.Any())
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (filter.OriginBranchId.HasValue)  // بر اساس شعبه صادرکننده
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);
            if (!string.IsNullOrEmpty(filter.IssuerUserName))  // کاربر صادرکننده
                query = query.Where(n => n.CreatedBy == filter.IssuerUserName);


            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                DateTime date = filter.strFromDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date >= date.Date);
            }
            else if (string.IsNullOrEmpty(filter.strFromDate) && string.IsNullOrEmpty(filter.BiilOdLadingNumber) && !filter.PartyId.HasValue)
            {
                query = query.Where(n => n.BillOfLadingStatusId < 11 || (!n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().IsSettled && n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId != 3));
            }
            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                DateTime date = filter.strUntilDate.PersianToLatin();
                query = query.Where(n => n.IssuanceDate.Date <= date.Date);
            }


            if (filter.BillStatus?.Length > 0)
                query = query.Where(n => filter.BillStatus.Contains(n.BillOfLadingStatusId));

            if (filter.PaymentStatus.HasValue)
            {
                bool IsPayed = filter.PaymentStatus.Value == 1 ? true : false;
                query = query
                   .Where(n => n.FinancialTransactions != null ? n.FinancialTransactions.Where(f => f.OperationId == 1)
                   .FirstOrDefault().IsSettled == IsPayed : false);
            }
            if (filter.PartyId.HasValue)
                query = query.Where(n => n.FinancialTransactions.FirstOrDefault(x => x.OperationId == 1).AccountPartyId == filter.PartyId.Value);

            if (filter.SettelmentType.HasValue)
                query = query.Where(n => n.FinancialTransactions.FirstOrDefault(x => x.OperationId == 1).SettlementTypeId == filter.SettelmentType.Value);

            if (!string.IsNullOrEmpty(filter.Issuer))
                query = query.Where(n => n.CreatedBy == filter.Issuer);

            if (filter.OriginCityId.HasValue)
                query = query.Where(n => n.Route.OriginCityId == filter.OriginCityId);

            if (filter.DestinationCityId.HasValue)
                query = query.Where(n => n.Route.DestinationCityId == filter.DestinationCityId);

            if (filter.PersonId.HasValue)
            {
                if (filter.personSearchtype == 1)
                {
                    query = query.Where(n => n.SenderId == filter.PersonId.Value || n.ReceiverId == filter.PersonId.Value);
                }
                else if (filter.personSearchtype == 2)
                {
                    query = query.Where(n => n.ReceiverId == filter.PersonId.Value);
                }
                else if (filter.personSearchtype == 3)
                {
                    query = query.Where(n => n.SenderId == filter.PersonId.Value);
                }
            }

            var result = query.Select(n => new AccBillViewModel
            {
                Id = n.Id,
                Number = n.WaybillNumber,
                Date = n.IssuanceDate,
                IssuerBranchId = n.OriginBranchId,
                IssuerBranch = n.IssuingBranch.BranchName,
                IssuerUserName = n.CreatedBy,
                PartyId = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().AccountPartyId,
                PartyName = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().Party.Name,
                SettelmentTypeId = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().SettlementTypeId,
                BasePrice = n.BillCosts.Where(n => n.CostType.CostCode == "101").Sum(n => n.Amount),
                TotalCost = n.BillCosts.Where(n => n.CostType.CostCode != "101" && n.CostType.CostCode != "105").Sum(n => n.Amount),
                VatPrice = n.BillCosts.Where(n => n.CostType.CostCode == "105").Sum(n => n.Amount),
                TotalDiscount = n.Consignments.Sum(d => d.Discount),
                Payed = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().MoneyTransactions.Where(x => !x.IsDeleted).Sum(x => x.CreditAmount),
                DestinationCityId = n.Route.DestinationCityId,
                DestinationCityName = n.Route.DestinationCity.PersianName,
                OriginCityId = n.Route.OriginCityId,
                OriginCityName = n.Route.OriginCity.PersianName,

                SenderId = n.SenderId,
                SenderName = n.Sender.Name,
                ReciverId = n.ReceiverId,
                ReciverName = n.Receiver.Name,
                statusId = n.BillOfLadingStatusId,

            }).AsQueryable();

            return result;
        }
        public async Task<List<CreditAccountBill>> FetchCurrentDebtStatusAsync(AccSalesFilterDto filter)
        {

            var query = _db.Cu_BillOfLadings.AsNoTracking()

                .Include(n => n.IssuingBranch)
                .Include(n => n.BillOfLadingStatus)
                .Include(n => n.BillCosts).ThenInclude(n => n.CostType)
                .Include(n => n.FinancialTransactions).ThenInclude(n => n.MoneyTransactions)
                 .Include(n => n.FinancialTransactions).ThenInclude(n => n.Party)
                 .Include(n => n.TreTransactions)
                .Where(n =>
               (n.BillOfLadingStatusId >= 2 && n.BillOfLadingStatusId <= 11)
                && n.SellerId == filter.SellerId
                && !n.IsDeleted
                && n.FinancialTransactions.Any()
                && n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().SettlementTypeId == 3
                && !n.FinancialTransactions.Where(f => f.OperationId == 1).FirstOrDefault().IsSettled
                )
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber)) // فیلتر بر اساس بحشی از شماره بارنامه
                query = query.Where(n => n.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (filter.OriginBranchId.HasValue)  // بر اساس شعبه صادرکننده
                query = query.Where(n => n.OriginBranchId == filter.OriginBranchId);
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

            if (filter.PaymentStatus.HasValue)
            {
                bool IsPayed = filter.PaymentStatus.Value == 1 ? true : false;
                query = query
                   .Where(n => n.FinancialTransactions != null ? n.FinancialTransactions.Where(f => f.OperationId == 1)
                   .FirstOrDefault().IsSettled == IsPayed : false);
            }

            if (filter.PartyId.HasValue)
                query = query.Where(n => n.FinancialTransactions.FirstOrDefault(x => x.OperationId == 1).AccountPartyId == filter.PartyId.Value);


            var AllDate = query.Select(n => new AccBillViewModel
            {
                Id = n.Id,
                Number = n.WaybillNumber,
                Date = n.IssuanceDate,
                IssuerBranchId = n.OriginBranchId,
                IssuerBranch = n.IssuingBranch.BranchName,
                IssuerUserName = n.CreatedBy,
                PartyId = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().AccountPartyId,
                PartyName = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().Party.Name,
                SettelmentTypeId = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().SettlementTypeId,
                BasePrice = n.BillCosts.Where(n => n.CostType.CostCode == "101").Sum(n => n.Amount),
                TotalCost = n.BillCosts.Where(n => n.CostType.CostCode != "101" && n.CostType.CostCode != "105").Sum(n => n.Amount),
                VatPrice = n.BillCosts.Where(n => n.CostType.CostCode == "105").Sum(n => n.Amount),
                TotalDiscount = n.Consignments.Sum(d => d.Discount),
                Payed = n.FinancialTransactions.Where(z => z.OperationId == 1).FirstOrDefault().MoneyTransactions.Where(x => !x.IsDeleted).Sum(x => x.CreditAmount),
            }).AsQueryable();

            var data = await AllDate.ToListAsync();
            var report = data.GroupBy(n => n.PartyId).Select(n => new CreditAccountBill
            {
                PartyId = n.Key,
                PartyName = n.FirstOrDefault().PartyName,
                FirstDate = n.Min(x => x.Date),
                LastDate = n.Max(x => x.Date),
                QtyBill = n.Count(),
                Bed = n.Sum(x => x.FinalPrice),
                Bes = n.Sum(x => x.Payed)

            }).ToList();

            return report;
        }
        public IQueryable<SaleMoneyTransaction> GetSaleMoneyTransactionsAsync(SaleMoneyTransactionFilterDto filter)
        {
            var query = _db.TreTransactions.AsNoTracking()
            .Include(n => n.BillFinancialTransaction)
            .Include(n => n.BillOfLading)
            .Include(n => n.Party)
            .Include(n => n.Operation)
            .Include(n => n.BankAccount)
            .Where(n => n.SellerId == filter.SellerId && !n.IsDeleted && n.BillOfLadingId.HasValue).AsQueryable();

            if (filter.PaymentMethod.HasValue)
                query = query.Where(n => n.OperationId == filter.PaymentMethod.Value);
            if (filter.PartyId.HasValue)
                query = query.Where(n => n.AccountPartyId == filter.PartyId.Value);
            if (filter.SettelmentType.HasValue)
                query = query.Where(x => x.BillFinancialTransaction.SettlementTypeId == filter.SettelmentType.Value);
            if (!string.IsNullOrEmpty(filter.strFromDate))
            {
                DateTime date = filter.strFromDate.PersianToLatin();
                query = query.Where(n => n.TransactionDate.Date >= date.Date);
            }
            if (!string.IsNullOrEmpty(filter.strUntilDate))
            {
                DateTime date = filter.strUntilDate.PersianToLatin();
                query = query.Where(n => n.TransactionDate.Date <= date.Date);
            }
            if (!string.IsNullOrEmpty(filter.BiilOdLadingNumber))
                query = query.Where(n => n.BillOfLading.WaybillNumber.Contains(filter.BiilOdLadingNumber));
            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(n => n.Description.Contains(filter.Description));
            if (filter.BankAccountId.HasValue)
                query = query.Where(n => n.BankAccountId == filter.BankAccountId || n.Pos.BankAccountId == filter.BankAccountId);
            if (filter.PosId.HasValue)
                query = query.Where(n => n.PosId == filter.PosId);

            var report = query.Select(n => new SaleMoneyTransaction
            {
                TransactionDate = n.TransactionDate,
                TransactionTime = n.TransactionTime,
                Amount = n.Amount,
                BankAccountId = n.BankAccountId,
                BankAccountName = n.BankAccount.AccountName ?? "",
                PaymentMethodId = n.OperationId,
                BillId = n.BillOfLadingId,
                BillIssuerName = n.BillOfLading.CreatedBy,
                BranchId = n.BillOfLading.OriginBranchId,
                Cashier = n.User != null ? n.User.FName + n.User.Family : "سیستم",
                PartyId = n.AccountPartyId,
                PartyName = n.Party.Name,
                POSId = n.PosId,
                POSName = n.Pos != null ? n.Pos.Name : "",
                SettelmentTypeId = n.BillFinancialTransaction.SettlementTypeId,
                TrackingNumber = n.TransferNumber,
                WaybillNumber = n.BillOfLading.WaybillNumber,
                Desciption = n.Description,
                PaymentMethodName = n.Operation.OperationName,

            }).OrderBy(n => n.TransactionDate).ThenBy(n => n.TransactionTime).AsQueryable();

            return report;
        }


        public async Task<clsResult> MoveToAnotherAccountAsync(MoveAccountDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            if (!dto.OriginAccounts.Any())
            {
                result.Message = "اطلاعاتی برای انتقال یافت نشد";
                return result;
            }

            var financialData = await _db.Cu_FinancialTransactions.Where(n => dto.OriginAccounts.Contains(n.AccountPartyId)).ToListAsync();
            if (!financialData.Any())
            {
                result.Message = "برای طرف حساب های انتخاب شده، سوابق مالی یافت نشد.";
                return result;
            }
            var transactionTargets = financialData.Select(n => n.Id).ToList<Guid>();
            var moneyTransactions = await _db.TreTransactions.Where(n => transactionTargets.Contains(n.BillFinancialtransactionId.Value)).ToListAsync();

            foreach (var item in financialData)
            {
                item.AccountPartyId = dto.TargetAccount;
            }
            _db.Cu_FinancialTransactions.UpdateRange(financialData);

            foreach (var item in moneyTransactions)
            {
                item.AccountPartyId = dto.TargetAccount;
            }
            _db.TreTransactions.UpdateRange(moneyTransactions);

            try
            {
                await _db.SaveChangesAsync();
                result.Success &= result.Success;
                result.Message = "یکپارچه سازی حساب ها با موفقیت انجام شد.";
            }
            catch
            {
                result.Message = "در عملیات یکپارچه سازی خطایی رخ داده است.";
            }

            return result;
        }

        public async Task<List<AccBillViewModel>> GetPartyBillsAsync(PartyBillsFilterDto filter)
        {

            var query = _db.Cu_FinancialTransactions.AsNoTracking()
                .Where(n => n.AccountPartyId == filter.PartyId.Value && !n.IsDeleted && (n.BillOfLading.BillOfLadingStatusId >= 2))
                .Include(n => n.BillOfLading).ThenInclude(n => n.Consignments)
                .Include(n => n.BillOfLading.BillCosts)
                .Include(n => n.Party)
                .Include(n => n.BillOfLading).ThenInclude(n => n.IssuingBranch)
                .Include(n => n.MoneyTransactions)
                 .Include(n => n.BillOfLading).ThenInclude(n => n.Route).ThenInclude(n => n.OriginCity)
                 .Include(n => n.BillOfLading).ThenInclude(n => n.Route).ThenInclude(n => n.DestinationCity)
                .AsQueryable();


            if (filter.IsPayed.HasValue)
                query = query.Where(n => n.IsSettled == filter.IsPayed);
            if (!string.IsNullOrEmpty(filter.strStartDate))
            {
                DateTime fromDate = filter.strStartDate.PersianToLatin();
                query = query.Where(n => n.BillOfLading.IssuanceDate.Date >= fromDate.Date);
            }
            if (!string.IsNullOrEmpty(filter.strEndDate))
            {
                DateTime endDate = filter.strEndDate.PersianToLatin();
                query = query.Where(n => n.BillOfLading.IssuanceDate.Date <= endDate.Date);
            }

            var result = await query.Select(n => new AccBillViewModel
            {
                Id = n.Id,
                BillId = n.BillOfLadingId ?? Guid.Empty,
                Number = n.BillOfLading.WaybillNumber,
                Date = n.BillOfLading.IssuanceDate,
                PerianDate = n.BillOfLading.IssuanceDate.LatinToPersian(),
                IssuerBranchId = n.BillOfLading.OriginBranchId,
                IssuerBranch = n.BillOfLading.IssuingBranch.BranchName,
                IssuerUserName = n.BillOfLading.CreatedBy,
                PartyId = n.AccountPartyId,
                PartyName = n.Party.Name,
                SettelmentTypeId = n.SettlementTypeId,
                BasePrice = n.BillOfLading.BillCosts.Where(n => n.CostType.CostCode == "101").Sum(n => n.Amount),
                TotalCost = n.BillOfLading.BillCosts.Where(n => n.CostType.CostCode != "101" && n.CostType.CostCode != "105").Sum(n => n.Amount),
                VatPrice = n.BillOfLading.BillCosts.Where(n => n.CostType.CostCode == "105").Sum(n => n.Amount),
                TotalDiscount = n.BillOfLading.Consignments.Sum(d => d.Discount),
                Payed = n.MoneyTransactions.Sum(n => n.CreditAmount),
                statusId = n.BillOfLading.BillOfLadingStatusId,
                DestinationCityId = n.BillOfLading.Route.DestinationCityId,
                DestinationCityName = n.BillOfLading.Route.DestinationCity.PersianName,
                OriginCityId = n.BillOfLading.Route.OriginCityId,
                OriginCityName = n.BillOfLading.Route.OriginCity.PersianName,

            }).OrderBy(n => n.Date).ToListAsync();

            return result;
        }

        public async Task<List<MoadianExportDto>> CreateMoadiansAsync(PartyBillsFilterDto filter)
        {

            var bills = await GetPartyBillsAsync(filter);
            var query = _db.Cu_FinancialTransactions
                .Where(n => n.AccountPartyId == filter.PartyId.Value && !n.IsDeleted && (n.BillOfLading.BillOfLadingStatusId >= 2 && n.BillOfLading.BillOfLadingStatusId <= 11))
                .Include(n => n.BillOfLading)
                .Include(n => n.Party)
                .Include(n => n.BillOfLading)
                .AsQueryable();

            if (filter.IsPayed.HasValue)
                query = query.Where(n => n.IsSettled == filter.IsPayed);
            if (!string.IsNullOrEmpty(filter.strStartDate))
            {
                DateTime fromDate = filter.strStartDate.PersianToLatin();
                query = query.Where(n => n.BillOfLading.IssuanceDate.Date >= fromDate.Date);
            }
            if (!string.IsNullOrEmpty(filter.strEndDate))
            {
                DateTime endDate = filter.strEndDate.PersianToLatin();
                query = query.Where(n => n.BillOfLading.IssuanceDate.Date <= endDate.Date);
            }
            var test = await query.ToListAsync();

            //TotalPriceAfterDiscount + VatPrice

            var result = await query.Select(n => new MoadianExportDto
            {
                AccountingInvoiceCode = n.BillOfLading.WaybillNumber,
                InvoiceNumber = (n.AccountPartyId + DateTime.Now.LatinToPersian().Replace("/", "")),
                BuyerType = n.Party.LegalStatus,
                BuyerFullName = n.Party.Name,
                EconomicCode = n.Party.EconomicCode,
                NationalId = n.Party.NationalId,
                PostalCode = n.Party.PostalCode,
                Address = n.Party.Address,
                Discount = 0,
                BasePrice = n.BillOfLading.BillCosts.Where(n => n.CostType.CostCode == "101").Sum(n => n.Amount),
                TotalCost = n.BillOfLading.BillCosts.Where(n => n.CostType.CostCode != "101" && n.CostType.CostCode != "105").Sum(n => n.Amount),
                VatPrice = n.BillOfLading.BillCosts.Where(n => n.CostType.CostCode == "105").Sum(n => n.Amount),
                TotalDiscount = n.BillOfLading.Consignments.Sum(d => d.Discount),
                statusId = n.BillOfLading.BillOfLadingStatusId,

            }).ToListAsync();

            return result;
        }


        public async Task<clsResult> CHangeBillPartyAsync(ChangePartyDto dto)
        {
            clsResult result = new clsResult();
            result.Success = false;
            var waybill = await _db.Cu_FinancialTransactions.Where(n => n.OperationId == 1 && n.BillOfLadingId == dto.BillId).FirstOrDefaultAsync();
            if (waybill == null)
            {
                result.Message = "اطلاعات تسویه حساب بارنامه یافت نشد";
                return result;
            }

            waybill.AccountPartyId = dto.PartyId;

            _db.Cu_FinancialTransactions.Update(waybill);

            try
            {
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "طرف حساب بارنامه با موفقیت برزورسانی شد";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "خطایی در بروزرسانی اطلاعات رخ داده است";
            }

            return result;


        }
    }
}
