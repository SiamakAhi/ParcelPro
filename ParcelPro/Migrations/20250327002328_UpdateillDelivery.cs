using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateillDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Delivered",
                table: "Cu_BillOfLadings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "tg_CourierManUserName",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "tg_DeliveryDate",
                table: "Cu_BillOfLadings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tg_Description",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tg_Name",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tg_NationalityCode",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tg_Phone",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tg_Signature",
                table: "Cu_BillOfLadings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivered",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "tg_CourierManUserName",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "tg_DeliveryDate",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "tg_Description",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "tg_Name",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "tg_NationalityCode",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "tg_Phone",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "tg_Signature",
                table: "Cu_BillOfLadings");
        }
    }
}
