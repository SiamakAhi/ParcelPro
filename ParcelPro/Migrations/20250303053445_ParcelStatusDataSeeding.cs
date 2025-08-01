using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class ParcelStatusDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cu_ParcelStatuses",
                columns: new[] { "Id", "ForReciver", "ForSender", "Message", "Status", "StatusCode" },
                values: new object[,]
                {
                    { 1, false, false, "مرسوله در حال صدور است.", "در حال صدور", "1" },
                    { 2, false, false, "مرسوله در انتظار پرداخت است.", "در انتظار پرداخت", "2" },
                    { 3, false, false, "مرسوله در انتظار جمع آوری است.", "در انتظار جمع آوری", "3" },
                    { 4, false, false, "مرسوله در حال جمع آوری است.", "در حال جمع آوری", "4" },
                    { 5, false, false, "مرسوله به هاب مبدأ وارد شده است.", "تأیید ورود به هاب مبدأ", "5" },
                    { 6, false, false, "مرسوله در حال ارسال به هاب شهر مقصد است.", "در حال ارسال به هاب شهر مقصد", "6" },
                    { 7, false, false, "مرسوله به هاب شهر مقصد وارد شده است.", "تأیید ورود به هاب شهر مقصد", "7" },
                    { 8, false, false, "مرسوله آماده توزیع است و منتظر تحویل به سفیر است.", "آماده توزیع (در انتظار تحویل به سفیر)", "8" },
                    { 9, false, false, "مرسوله به سفیر تحویل شده است و در حال ارسال به گیرنده است.", "تحویل سفیر جهت تحویل به گیرنده", "9" },
                    { 10, false, false, "مرسوله در انتظار پرداخت توسط گیرنده است.", "در انتظار پرداخت توسط گیرنده", "10" },
                    { 11, false, false, "مرسوله با موفقیت به گیرنده تحویل داده شد.", "مرسوله تحویل گیرنده شد", "11" },
                    { 12, false, false, "مرسوله به هاب مقصد برگشت داده شده است.", "برگشت به هاب مقصد", "12" },
                    { 13, false, false, "مرسوله مفقود شده است.", "مفقود شده", "13" },
                    { 14, false, false, "مرسوله فاسد شده است.", "فاسد شده", "14" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cu_ParcelStatuses",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
