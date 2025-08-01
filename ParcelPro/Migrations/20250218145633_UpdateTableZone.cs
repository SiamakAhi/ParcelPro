using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceBaseFactor",
                table: "Cu_RateZones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cu_RateZones",
                keyColumn: "ZoneId",
                keyValue: 1,
                column: "PriceBaseFactor",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Cu_RateZones",
                keyColumn: "ZoneId",
                keyValue: 2,
                column: "PriceBaseFactor",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Cu_RateZones",
                keyColumn: "ZoneId",
                keyValue: 3,
                column: "PriceBaseFactor",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceBaseFactor",
                table: "Cu_RateZones");
        }
    }
}
