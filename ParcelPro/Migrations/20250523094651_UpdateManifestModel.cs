using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManifestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierId",
                table: "Cu_CargoManifests");

            migrationBuilder.RenameColumn(
                name: "Transport",
                table: "Cu_CargoManifests",
                newName: "TransportDate");

            migrationBuilder.RenameColumn(
                name: "OriginHub",
                table: "Cu_CargoManifests",
                newName: "OriginHubId");

            migrationBuilder.RenameColumn(
                name: "DestinationHub",
                table: "Cu_CargoManifests",
                newName: "DestinationHubId");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Cu_CargoManifests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeight",
                table: "Cu_CargoManifests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Cu_Couriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CourierPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoeinId = table.Column<int>(type: "int", nullable: true),
                    TafsilId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_Couriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cu_Couriers_Cu_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Cu_Branch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cu_Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DriverPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cu_Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoadCapacityKg = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MoeinId = table.Column<int>(type: "int", nullable: true),
                    TafsilId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cu_CargoManifests_DestinationHubId",
                table: "Cu_CargoManifests",
                column: "DestinationHubId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_CargoManifests_DriverId",
                table: "Cu_CargoManifests",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_CargoManifests_OriginHubId",
                table: "Cu_CargoManifests",
                column: "OriginHubId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_CargoManifests_VehicleId",
                table: "Cu_CargoManifests",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_Couriers_BranchId",
                table: "Cu_Couriers",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Drivers_DriverId",
                table: "Cu_CargoManifests",
                column: "DriverId",
                principalTable: "Cu_Drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Hubs_DestinationHubId",
                table: "Cu_CargoManifests",
                column: "DestinationHubId",
                principalTable: "Cu_Hubs",
                principalColumn: "HubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Hubs_OriginHubId",
                table: "Cu_CargoManifests",
                column: "OriginHubId",
                principalTable: "Cu_Hubs",
                principalColumn: "HubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Vehicles_VehicleId",
                table: "Cu_CargoManifests",
                column: "VehicleId",
                principalTable: "Cu_Vehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Drivers_DriverId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Hubs_DestinationHubId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Hubs_OriginHubId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_CargoManifests_Cu_Vehicles_VehicleId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropTable(
                name: "Cu_Couriers");

            migrationBuilder.DropTable(
                name: "Cu_Drivers");

            migrationBuilder.DropTable(
                name: "Cu_Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Cu_CargoManifests_DestinationHubId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropIndex(
                name: "IX_Cu_CargoManifests_DriverId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropIndex(
                name: "IX_Cu_CargoManifests_OriginHubId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropIndex(
                name: "IX_Cu_CargoManifests_VehicleId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Cu_CargoManifests");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "Cu_CargoManifests");

            migrationBuilder.RenameColumn(
                name: "TransportDate",
                table: "Cu_CargoManifests",
                newName: "Transport");

            migrationBuilder.RenameColumn(
                name: "OriginHubId",
                table: "Cu_CargoManifests",
                newName: "OriginHub");

            migrationBuilder.RenameColumn(
                name: "DestinationHubId",
                table: "Cu_CargoManifests",
                newName: "DestinationHub");

            migrationBuilder.AddColumn<Guid>(
                name: "CarrierId",
                table: "Cu_CargoManifests",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
