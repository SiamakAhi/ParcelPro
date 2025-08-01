using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFinancialTransactionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BillFinancialtransactionId",
                table: "TreTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreTransactions_BillFinancialtransactionId",
                table: "TreTransactions",
                column: "BillFinancialtransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TreTransactions_Cu_FinancialTransactions_BillFinancialtransactionId",
                table: "TreTransactions",
                column: "BillFinancialtransactionId",
                principalTable: "Cu_FinancialTransactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreTransactions_Cu_FinancialTransactions_BillFinancialtransactionId",
                table: "TreTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TreTransactions_BillFinancialtransactionId",
                table: "TreTransactions");

            migrationBuilder.DropColumn(
                name: "BillFinancialtransactionId",
                table: "TreTransactions");
        }
    }
}
