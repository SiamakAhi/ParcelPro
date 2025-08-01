using Microsoft.EntityFrameworkCore;
using ParcelPro.Areas.Courier.CuurierInterfaces;
using ParcelPro.Areas.Courier.Models.Entities;
using ParcelPro.Areas.Treasury.Dto;
using ParcelPro.Areas.Treasury.Models.Entities;
using ParcelPro.Areas.Treasury.TreasuryInterfaces;
using ParcelPro.Interfaces;
using ParcelPro.Models;
using ParcelPro.Services;

namespace ParcelPro.Areas.Treasury.TreasuryServices
{
    public class MoneyTransactionService : IMoneyTransactionService
    {
        private readonly AppDbContext _db;
        private readonly ITrachkingService _tracking;
        private readonly ISMSService _sms;
        private readonly SmsSenderPersiaFava _smsSenter;

        public MoneyTransactionService(AppDbContext appDbContext, ITrachkingService tackingService
            , SmsSenderPersiaFava smsSenter
            , ISMSService smsService)
        {
            _db = appDbContext;
            _tracking = tackingService;
            _smsSenter = smsSenter;
            _sms = smsService;
        }

        public string GenerateTrackingLink(Guid billId)
        {

            string trackingLink = $"https://hub.keyhanpost.ir/Courier/Billoflading/Bill?billOfladingId={billId}";
            return trackingLink;
        }

        public async Task<Guid> getBillIdByNumberAsync(string BillNumber)
        {
            Guid id = await _db.Cu_BillOfLadings.Where(n => n.WaybillNumber == BillNumber).Select(n => n.Id).FirstOrDefaultAsync();
            return id;
        }

        public async Task<bool> CreateTransactionAsync(TransactionDto transactionDto)
        {
            try
            {
                // اعتبارسنجی ورودی‌ها
                if (transactionDto == null)
                {
                    throw new ArgumentNullException(nameof(transactionDto), "اطلاعات تراکنش نمی‌تواند خالی باشد.");
                }

                if (transactionDto.Amount <= 0)
                {
                    throw new ArgumentException("مبلغ تراکنش باید بیشتر از صفر باشد.", nameof(transactionDto.Amount));
                }


                if (transactionDto.TransactionTypeId != 1 && transactionDto.TransactionTypeId != 2)
                {
                    throw new ArgumentException("نوع تراکنش نامعتبر است.", nameof(transactionDto.TransactionTypeId));
                }

                // تعیین بدهکاری و بستانکاری بر اساس نوع تراکنش
                long debitAmount = 0;
                long creditAmount = 0;

                if (transactionDto.TransactionTypeId == 1) // واریز
                {
                    creditAmount = transactionDto.Amount;
                }
                else if (transactionDto.TransactionTypeId == 2) // برداشت
                {
                    debitAmount = transactionDto.Amount;
                }

                // ایجاد موجودیت تراکنش
                var transactionEntity = new TreTransaction();

                transactionEntity.Id = Guid.NewGuid();
                transactionEntity.TransactionTypeId = transactionDto.TransactionTypeId;
                transactionEntity.SellerId = transactionDto.SellerId;
                transactionEntity.BillOfLadingId = transactionDto.BillOfLadingId;
                transactionEntity.BillFinancialtransactionId = transactionDto.BillFinancialRecordId;
                transactionEntity.AccountPartyId = transactionDto.AccountPartyId;
                transactionEntity.OperationId = transactionDto.OperationId;
                transactionEntity.TransactionDate = DateTime.Now;
                transactionEntity.TransactionTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                transactionEntity.Description = transactionDto.Description;
                transactionEntity.BranchId = transactionDto.BranchId;
                transactionEntity.UserId = transactionDto.UserId;
                transactionEntity.Amount = transactionDto.Amount;
                transactionEntity.DebitAmount = debitAmount;
                transactionEntity.CreditAmount = creditAmount;
                transactionEntity.BankAccountId = transactionDto.BankAccountId;
                transactionEntity.TransferNumber = transactionDto.TransferNumber;
                transactionEntity.PaymentGatewayTransactionId = transactionDto.PaymentGatewayTransactionId;
                transactionEntity.CheckNumber = transactionDto.CheckNumber;
                transactionEntity.CheckDueDate = transactionDto.CheckDueDate;
                transactionEntity.CheckOwnerName = transactionDto.CheckOwnerName;
                transactionEntity.OnlineGetwayName = transactionDto.OnlineGetwayName;
                transactionEntity.PosId = transactionDto.PosId;
                transactionEntity.IsDeleted = false;

                await _db.TreTransactions.AddAsync(transactionEntity);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطا در ثبت تراکنش: {ex.Message}");
                throw;
            }
        }

        public async Task<clsResult> BillOfLadingOnlinePaymentAsync(string billNumber, string transId, string idGet, string providerName)
        {
            clsResult result = new clsResult();
            result.Success = false;


            var bill = await _db.Cu_BillOfLadings.SingleOrDefaultAsync(n => n.WaybillNumber == billNumber);
            if (bill == null)
            {
                result.Message = "بارنامه شناسایی نشد";
                return result;
            }

            result.Id = bill.Id;
            var bilFinancialRecord = await _db.Cu_FinancialTransactions
                  .Where(n => n.BillOfLadingId == bill.Id && n.OperationId == 1).FirstOrDefaultAsync();
            if (bilFinancialRecord == null)
            {
                result.Message = "وضعیت مالی بارنامه یافت نشد";
                return result;
            }

            TransactionDto dto = new TransactionDto();
            dto.Id = Guid.NewGuid();
            dto.SellerId = bill.SellerId;
            dto.TransactionTypeId = 1; // واریز
            dto.OperationId = 6;
            dto.BillOfLadingId = bill.Id;
            dto.BillFinancialRecordId = bilFinancialRecord.Id;
            dto.AccountPartyId = bilFinancialRecord.AccountPartyId;
            dto.Amount = bilFinancialRecord.Amount;
            dto.DebitAmount = 0;
            dto.CreditAmount = dto.Amount;
            dto.BranchId = bill.OriginBranchId;
            dto.Description = $" تسویه حساب بارنامه {billNumber} از طریق درگاه پرداخت ";
            dto.TransferNumber = idGet;
            dto.PaymentGatewayTransactionId = transId;
            dto.OnlineGetwayName = providerName;
            dto.BillNumber = billNumber;
            dto.UserId = null;

            if (await CreateTransactionAsync(dto))
            {
                result.Success = true;

                if (bill.BillOfLadingStatusId < 3)
                {
                    bill.BillOfLadingStatusId = 3;
                    _db.Cu_BillOfLadings.Update(bill);
                }

                bilFinancialRecord.IsSettled = true;
                _db.Cu_FinancialTransactions.Update(bilFinancialRecord);

                try
                {
                    await _db.SaveChangesAsync();
                    result.Message = " پرداخت با موفقیت انجام شد";

                }
                catch (Exception)
                {
                    result.Message = "عملیت پرداخت با موفقیت انجام شد اما مشکلی در بروزرسانی وضعیت تسویه حساب بارنامه رخ داده است";
                }
                return result;
            }
            else
            {
                result.Message = ("مشکلی در ثبت تراکنش در خزانه داری رخ داده است");
                return result;
            }
        }

        public async Task<clsResult> BillOfLadingCashPaymentAsync(BillCashPayDto model)
        {
            clsResult result = new clsResult();
            result.Success = false;

            if (model.BillOfLadingId == null)
            {
                result.Message = "بارنامه شناسایی نشد";
                return result;
            }
            var bill = await _db.Cu_BillOfLadings
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .SingleOrDefaultAsync(n => n.Id == model.BillOfLadingId);
            if (bill == null)
            {
                result.Message = "بارنامه شناسایی نشد";
                return result;
            }

            result.Id = model.BillOfLadingId;

            Cu_FinancialTransaction CourierTransaction = new Cu_FinancialTransaction();
            var debtorRecord = await _db.Cu_FinancialTransactions
               .Where(n => n.BillOfLadingId == model.BillOfLadingId && n.OperationId == 1).FirstOrDefaultAsync();

            if (debtorRecord == null)
            {

                CourierTransaction.Id = Guid.NewGuid();
                CourierTransaction.OperationId = 1;
                CourierTransaction.SellerId = model.SellerId;
                CourierTransaction.BillOfLadingId = model.BillOfLadingId;
                CourierTransaction.AccountPartyId = model.AccountPartyId;
                CourierTransaction.SettlementTypeId = model.SettelmentType;
                CourierTransaction.Description = model.Description;
                CourierTransaction.BranchId = model.BranchId;
                CourierTransaction.UserId = model.UserId;
                CourierTransaction.Amount = model.Amount;
                CourierTransaction.Bed = model.Amount;
                CourierTransaction.Bes = 0;
                CourierTransaction.IsDeleted = false;
                CourierTransaction.IsSettled = false;

            }
            else
            {
                CourierTransaction = debtorRecord;

            }

            if (CourierTransaction == null)
            {
                result.Message = "وضعیت مالی بارنامه یافت نشد";
                return result;
            }
            long settelmentAmount = await BillSettelMentPriceAsync(model.Id);
            if (settelmentAmount > model.PayAmount)
            {
                result.Message = "مبلغ وارد شده بیشتر از مبلغ تسویه حساب بارنامه است";
                return result;
            }

            //-------------------------------------------------
            TreTransaction dto = new TreTransaction();
            dto.Id = Guid.NewGuid();
            dto.SellerId = model.SellerId;
            dto.TransactionTypeId = 1; // واریز
            dto.OperationId = model.OperationId;
            dto.BillOfLadingId = model.BillOfLadingId;
            dto.BillFinancialtransactionId = CourierTransaction.Id;
            dto.AccountPartyId = CourierTransaction.AccountPartyId;
            dto.Amount = model.PayAmount;
            dto.DebitAmount = 0;
            dto.CreditAmount = model.PayAmount;
            dto.BranchId = model.BranchId;
            dto.Description = model.Description;
            dto.TransferNumber = model.TransferNumber;
            dto.PaymentGatewayTransactionId = model.TransferNumber;
            dto.BillNumber = model.BillNumber;
            dto.BankAccountId = model.BankAccountId;
            dto.PosId = model.PosId;
            dto.UserId = model.UserId;
            dto.TransactionDate = DateTime.Now;
            dto.TransactionTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            dto.IsDeleted = false;
            //-----------------------------------

            if (model.PayAmount >= model.DebitAmount)
                CourierTransaction.IsSettled = true;

            if (debtorRecord != null)
                _db.Cu_FinancialTransactions.Update(CourierTransaction);
            else
                _db.Cu_FinancialTransactions.Add(CourierTransaction);

            try
            {

                // اگر برنامه جدید باشد  و بارنامه در وضعیت 1 یا 2 باشد یعنی تسویه کامل انجام نشده باشد . 
                if (model.PayAmount < model.DebitAmount && bill.BillOfLadingStatusId < 3)
                    bill.BillOfLadingStatusId = 2;
                else if (model.PayAmount >= model.DebitAmount && bill.BillOfLadingStatusId < 3)
                    bill.BillOfLadingStatusId = 3;

                _db.TreTransactions.Add(dto);
                _db.Cu_BillOfLadings.Update(bill);
                await _db.SaveChangesAsync();

                result.Success = true;
                result.Message = "عملیت پرداخت با موفقیت انجام شد";

                if (bill.BillOfLadingStatusId == 3 || bill.BillOfLadingStatusId == 5)
                {

                    string senderNumber = bill.Sender.MobilePhone;
                    string reciverNumber = bill.Receiver.MobilePhone;
                    if (!string.IsNullOrEmpty(bill.SenderPhone) && bill.SenderPhone.IsMobile())
                        senderNumber = bill.SenderPhone;
                    if (!string.IsNullOrEmpty(bill.ReceiverPhone) && bill.ReceiverPhone.IsMobile())
                        reciverNumber = bill.ReceiverPhone;

                    string trackingLink = GenerateTrackingLink(bill.Id);
                    string senderMessage = _sms.GenerateSenderMessage(bill.Sender.Name, bill.WaybillNumber, trackingLink);
                    string reciverMessage = _sms.GenerateRecipientMessage(bill.Receiver.Name, bill.WaybillNumber, trackingLink);
                    var sendToSender = await _smsSenter.SendSmsAsync(senderNumber, senderMessage);
                    var sendToReciver = await _smsSenter.SendSmsAsync(reciverNumber, reciverMessage);
                }
            }
            catch (Exception x)
            {
                result.Message = ("مشکلی در ثبت تراکنش در خزانه داری رخ داده است");
                result.Message += "\n " + x.Message;

            }

            return result;
        }

        public async Task<clsResult> SendPaymentLinkAsync(BillCashPayDto model, string linkAddress)
        {
            clsResult result = new clsResult();
            result.Success = false;
            var bill = await _db.Cu_BillOfLadings
              .Include(n => n.Sender)
              .Include(n => n.Receiver)
              .SingleOrDefaultAsync(n => n.Id == model.BillOfLadingId);
            if (bill == null)
            {
                result.Message = "بارنامه شناسایی نشد";
                return result;
            }

            Cu_FinancialTransaction CourierTransaction = new Cu_FinancialTransaction();

            var debtorRecord = await _db.Cu_FinancialTransactions
                .Where(n => n.BillOfLadingId == model.BillOfLadingId).FirstOrDefaultAsync();

            if (debtorRecord == null)
            {

                CourierTransaction.Id = Guid.NewGuid();
                CourierTransaction.OperationId = 1;
                CourierTransaction.SellerId = model.SellerId;
                CourierTransaction.BillOfLadingId = model.BillOfLadingId;
                CourierTransaction.AccountPartyId = model.AccountPartyId;
                CourierTransaction.SettlementTypeId = model.SettelmentType;
                CourierTransaction.Description = model.Description;
                CourierTransaction.BranchId = model.BranchId;
                CourierTransaction.UserId = model.UserId;
                CourierTransaction.Amount = model.Amount;
                CourierTransaction.Bed = model.Amount;
                CourierTransaction.Bes = 0;
                CourierTransaction.IsDeleted = false;
                CourierTransaction.IsSettled = false;
                _db.Cu_FinancialTransactions.Add(CourierTransaction);

            }
            else
                CourierTransaction = debtorRecord;

            try
            {
                await _db.SaveChangesAsync();
                short statusId = 2;
                bool setStatus = await _tracking.SetStatusAsync(model.BillOfLadingId.Value, statusId);
                string number = string.IsNullOrEmpty(bill.SenderPhone) ? bill.Sender.MobilePhone : bill.SenderPhone;
                string trackingLink = linkAddress;
                string Message = _sms.GenerateSendPaymentLinkMessage(bill.WaybillNumber, linkAddress);
                var sendToSender = await _smsSenter.SendSmsAsync(number, Message);
                result.Success = true;
                result.Message = "لینک پرداخت با موفقیت ارسال شد";

            }
            catch (Exception x)
            {
                result.Message = "خطایی در ارسال پیام رخ داده است";
                result.Message += "\n " + x.Message;

            }

            return result;

        }

        public async Task<long> BillSettelMentPriceAsync(Guid id)
        {
            var Bill = await _db.Cu_FinancialTransactions.Include(n => n.MoneyTransactions)
                .Where(n => n.BillOfLadingId == id && n.OperationId == 1).FirstOrDefaultAsync();
            if (Bill == null) return 0;

            long BillPrice = Bill.Amount;
            long payed = Bill.MoneyTransactions.Sum(n => n.CreditAmount);

            return (BillPrice - payed);

        }

    }
}