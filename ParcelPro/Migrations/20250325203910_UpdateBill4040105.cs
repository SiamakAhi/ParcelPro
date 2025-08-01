using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBill4040105 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DigitalSignature",
                table: "Cu_BillOfLadings",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverPhone",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderPhone",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DigitalSignature",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "ReceiverPhone",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "SenderPhone",
                table: "Cu_BillOfLadings");
        }
    }
}
