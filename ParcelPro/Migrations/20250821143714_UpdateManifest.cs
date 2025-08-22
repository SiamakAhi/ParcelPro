using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManifest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirWaybillNumber",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "AirWaybillPrice",
                table: "Cu_CargoManifests");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Cu_CargoManifests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "AirlineCompany",
                table: "Cu_CargoManifests",
                newName: "IssuerDescription");

            migrationBuilder.AlterColumn<short>(
                name: "TransportType",
                table: "Cu_CargoManifests",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "BillOfLadingDate",
                table: "Cu_CargoManifests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillOfLadingNumber",
                table: "Cu_CargoManifests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Cu_CargoManifests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreightCompany",
                table: "Cu_CargoManifests",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FreightCost",
                table: "Cu_CargoManifests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "MovementDate",
                table: "Cu_CargoManifests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MovementTime",
                table: "Cu_CargoManifests",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillOfLadingDate",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "BillOfLadingNumber",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "FreightCompany",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "FreightCost",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "MovementDate",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "MovementTime",
                table: "Cu_CargoManifests");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Cu_CargoManifests",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "IssuerDescription",
                table: "Cu_CargoManifests",
                newName: "AirlineCompany");

            migrationBuilder.AlterColumn<int>(
                name: "TransportType",
                table: "Cu_CargoManifests",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<string>(
                name: "AirWaybillNumber",
                table: "Cu_CargoManifests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AirWaybillPrice",
                table: "Cu_CargoManifests",
                type: "bigint",
                nullable: true);
        }
    }
}
