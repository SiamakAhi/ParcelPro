using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel112 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreditCus_CreditAmount",
                table: "parties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "CreditCus_Email",
                table: "parties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditCus_Mobile",
                table: "parties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCreditCustomer",
                table: "parties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DistributerBranchId",
                table: "Cu_BillOfLadings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BillOfLadings_DistributerBranchId",
                table: "Cu_BillOfLadings",
                column: "DistributerBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_Branch_DistributerBranchId",
                table: "Cu_BillOfLadings",
                column: "DistributerBranchId",
                principalTable: "Cu_Branch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_Branch_DistributerBranchId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropIndex(
                name: "IX_Cu_BillOfLadings_DistributerBranchId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "CreditCus_CreditAmount",
                table: "parties");

            migrationBuilder.DropColumn(
                name: "CreditCus_Email",
                table: "parties");

            migrationBuilder.DropColumn(
                name: "CreditCus_Mobile",
                table: "parties");

            migrationBuilder.DropColumn(
                name: "IsCreditCustomer",
                table: "parties");

            migrationBuilder.DropColumn(
                name: "DistributerBranchId",
                table: "Cu_BillOfLadings");
        }
    }
}
