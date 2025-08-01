using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class AddManifest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ManifestId",
                table: "Cu_Consignments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "Cu_BillOfLadings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequesterPerson",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "WayBillType",
                table: "Cu_BillOfLadings",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "Cu_CargoManifests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Transport = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransportType = table.Column<int>(type: "int", nullable: false),
                    OriginHub = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationHub = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPackages = table.Column<int>(type: "int", nullable: false),
                    NumberOfWaybills = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AirlineCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirWaybillNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirWaybillPrice = table.Column<long>(type: "bigint", nullable: true),
                    TahvilDahandeh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_CargoManifests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cu_Invoices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    PartyId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceDate = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Payed = table.Column<long>(type: "bigint", nullable: false),
                    IsSetteled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_Invoices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cu_Consignments_ManifestId",
                table: "Cu_Consignments",
                column: "ManifestId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BillOfLadings_InvoiceId",
                table: "Cu_BillOfLadings",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_Invoices_InvoiceId",
                table: "Cu_BillOfLadings",
                column: "InvoiceId",
                principalTable: "Cu_Invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_Consignments_Cu_CargoManifests_ManifestId",
                table: "Cu_Consignments",
                column: "ManifestId",
                principalTable: "Cu_CargoManifests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_Invoices_InvoiceId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_Consignments_Cu_CargoManifests_ManifestId",
                table: "Cu_Consignments");

            migrationBuilder.DropTable(
                name: "Cu_CargoManifests");

            migrationBuilder.DropTable(
                name: "Cu_Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Cu_Consignments_ManifestId",
                table: "Cu_Consignments");

            migrationBuilder.DropIndex(
                name: "IX_Cu_BillOfLadings_InvoiceId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "ManifestId",
                table: "Cu_Consignments");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "RequesterPerson",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "WayBillType",
                table: "Cu_BillOfLadings");
        }
    }
}
