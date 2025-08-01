using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_BillOfLadingStatus_Config : IEntityTypeConfiguration<Cu_BillOfLadingStatus>
    {
        public void Configure(EntityTypeBuilder<Cu_BillOfLadingStatus> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasData(
                   new Cu_BillOfLadingStatus
                   {
                       Id = 1,
                       Name = "در حال صدور",
                       Code = "1",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 2,
                       Name = "در انتظار پرداخت",
                       Code = "2",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 3,
                       Name = "در انتظار جمع آوری",
                       Code = "3",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 4,
                       Name = "در حال جمع آوری",
                       Code = "4",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 5,
                       Name = "تأیید ورود به هاب مبدأ",
                       Code = "5",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 6,
                       Name = "در حال ارسال به هاب شهر مقصد",
                       Code = "6",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 7,
                       Name = "تأیید ورود به هاب شهر مقصد",
                       Code = "7",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 8,
                       Name = "آماده توزیع (در انتظار تحویل به سفیر)",
                       Code = "8",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 9,
                       Name = "در حال توزیع",
                       Code = "9",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 10,
                       Name = "در انتظار پرداخت توسط گیرنده",
                       Code = "10",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 11,
                       Name = "مرسوله تحویل گیرنده شد",
                       Code = "11",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 12,
                       Name = "برگشت به هاب مقصد",
                       Code = "12",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 13,
                       Name = "مفقود شده",
                       Code = "13",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                   new Cu_BillOfLadingStatus
                   {
                       Id = 14,
                       Name = "فاسد شده",
                       Code = "14",
                       SendNotificationToCustomer = false,
                       SendNotificationToOperations = false
                   },
                 new Cu_BillOfLadingStatus
                 {
                     Id = 15,
                     Name = "باطل شده",
                     Code = "15",
                     SendNotificationToCustomer = false,
                     SendNotificationToOperations = false
                 },
                  new Cu_BillOfLadingStatus
                  {
                      Id = 16,
                      Name = "حذف شده",
                      Code = "16",
                      SendNotificationToCustomer = false,
                      SendNotificationToOperations = false
                  }
            );
        }
    }
}
