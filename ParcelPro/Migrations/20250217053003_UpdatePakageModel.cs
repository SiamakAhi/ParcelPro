using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePakageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "Wh_Warehouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WarehouseProductCategoryId",
                table: "Cu_Packagings",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Cu_Packagings",
                keyColumn: "Id",
                keyValue: 1,
                column: "WarehouseProductCategoryId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Wh_Warehouses_BranchId",
                table: "Wh_Warehouses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_Packagings_WarehouseProductCategoryId",
                table: "Cu_Packagings",
                column: "WarehouseProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_Packagings_Wh_ProductCategories_WarehouseProductCategoryId",
                table: "Cu_Packagings",
                column: "WarehouseProductCategoryId",
                principalTable: "Wh_ProductCategories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wh_Warehouses_Cu_Branch_BranchId",
                table: "Wh_Warehouses",
                column: "BranchId",
                principalTable: "Cu_Branch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_Packagings_Wh_ProductCategories_WarehouseProductCategoryId",
                table: "Cu_Packagings");

            migrationBuilder.DropForeignKey(
                name: "FK_Wh_Warehouses_Cu_Branch_BranchId",
                table: "Wh_Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Wh_Warehouses_BranchId",
                table: "Wh_Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Cu_Packagings_WarehouseProductCategoryId",
                table: "Cu_Packagings");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Wh_Warehouses");

            migrationBuilder.DropColumn(
                name: "WarehouseProductCategoryId",
                table: "Cu_Packagings");

            migrationBuilder.InsertData(
                table: "Cu_Packagings",
                columns: new[] { "Id", "ForExport", "Name", "PackageCode", "Price", "SellerId" },
                values: new object[,]
                {
                    { 2, false, "کارتن", "81", 0L, 3L },
                    { 3, false, "یونولیت", "82", 0L, 3L },
                    { 4, false, "کلد باکس", "83", 0L, 3L },
                    { 5, false, "جعبه چوبی", "84", 0L, 3L },
                    { 6, false, "جعبه فلزی", "85", 0L, 3L },
                    { 7, false, "پالت", "86", 0L, 3L },
                    { 8, false, "چمدان", "87", 0L, 3L },
                    { 9, false, "نایلون پیچ", "88", 0L, 3L },
                    { 10, false, "پاکت و کارتن", "89", 0L, 3L },
                    { 11, false, "پاکت و نایلون", "90", 0L, 3L },
                    { 12, false, "پاکت و یونولیت", "91", 0L, 3L },
                    { 13, false, "پاکت و کارتن و نایلون", "92", 0L, 3L },
                    { 14, false, "پاکت و نایلون و کارتن و یونولیت", "93", 0L, 3L },
                    { 15, false, "کارتن و جعبه چوبی", "94", 0L, 3L },
                    { 16, false, "کارتن و جعبه چوبی و نایلون", "95", 0L, 3L },
                    { 17, false, "جعبه چوبی و نایلون", "96", 0L, 3L },
                    { 18, false, "باکس پزشکی", "97", 0L, 3L },
                    { 19, false, "پالت و کارتن", "98", 0L, 3L },
                    { 20, false, "کارتن و نایلون پیچ", "99", 0L, 3L },
                    { 21, true, "box", "100", 0L, 3L },
                    { 22, false, "کیسه", "101", 0L, 3L }
                });
        }
    }
}
