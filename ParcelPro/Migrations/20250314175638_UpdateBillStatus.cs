using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBillStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)1,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در حال صدور", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)2,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در انتظار پرداخت", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)3,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در انتظار جمع آوری", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)4,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در حال جمع آوری", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)5,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "تأیید ورود به هاب مبدأ", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)6,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در حال ارسال به هاب شهر مقصد", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)7,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "تأیید ورود به هاب شهر مقصد", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)8,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "آماده توزیع (در انتظار تحویل به سفیر)", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)9,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "تحویل سفیر جهت تحویل به گیرنده", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)10,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در انتظار پرداخت توسط گیرنده", false, false });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)11,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "مرسوله تحویل گیرنده شد", false, false });

            migrationBuilder.InsertData(
                table: "Cu_BillOfLadingStatuses",
                columns: new[] { "Id", "Code", "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[,]
                {
                    { (short)12, "12", "برگشت به هاب مقصد", false, false },
                    { (short)13, "13", "مفقود شده", false, false },
                    { (short)14, "14", "فاسد شده", false, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)12);

            migrationBuilder.DeleteData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)13);

            migrationBuilder.DeleteData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)14);

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)1,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "یادداشت", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)2,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "جدید", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)3,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در حال جمع آوری", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)4,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "ورود به هاب مبدأ", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)5,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "آماده رهسپاری", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)6,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در حال ارسال به شهر مقصد", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)7,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "ورودد به هاب مقصد", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)8,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در حال توزیع", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)9,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "تحویل شد", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)10,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "در حال برکشت به فرستنده", true, true });

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)11,
                columns: new[] { "Name", "SendNotificationToCustomer", "SendNotificationToOperations" },
                values: new object[] { "برگشت شد", true, true });
        }
    }
}
