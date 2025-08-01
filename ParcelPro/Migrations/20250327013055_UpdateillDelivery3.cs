using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateillDelivery3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tg_Signature",
                table: "Cu_BillOfLadings",
                newName: "DeliveryErrorMessage");

            migrationBuilder.AddColumn<byte[]>(
                name: "tg_SignatureData",
                table: "Cu_BillOfLadings",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tg_SignatureData",
                table: "Cu_BillOfLadings");

            migrationBuilder.RenameColumn(
                name: "DeliveryErrorMessage",
                table: "Cu_BillOfLadings",
                newName: "tg_Signature");
        }
    }
}
