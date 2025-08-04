using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerKewordToBiiloflading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerKeyword",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerKeyword",
                table: "Cu_BillOfLadings");
        }
    }
}
