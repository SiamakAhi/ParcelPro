using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class banktransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoeinId",
                table: "BankAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TreBankTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    HasDoc = table.Column<bool>(type: "bit", nullable: false),
                    AccountHolderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Row = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debtor = table.Column<long>(type: "bigint", nullable: true),
                    Creditor = table.Column<long>(type: "bigint", nullable: true),
                    Balance = table.Column<long>(type: "bigint", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    DocumentOrCheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedCustomerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedCustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositorOrBeneficiary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreBankTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_MoeinId",
                table: "BankAccounts",
                column: "MoeinId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Acc_Coding_Moeins_MoeinId",
                table: "BankAccounts",
                column: "MoeinId",
                principalTable: "Acc_Coding_Moeins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Acc_Coding_Moeins_MoeinId",
                table: "BankAccounts");

            migrationBuilder.DropTable(
                name: "TreBankTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_MoeinId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "MoeinId",
                table: "BankAccounts");
        }
    }
}
