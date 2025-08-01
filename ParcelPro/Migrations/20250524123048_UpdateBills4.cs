using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBills4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "partnerId",
                table: "Cu_BillOfLadings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "partnerId",
                table: "Cu_BillOfLadings");
        }
    }
}
