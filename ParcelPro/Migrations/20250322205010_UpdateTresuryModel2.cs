using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTresuryModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreBankPosUc_BankAccounts_BankAccountId",
                table: "TreBankPosUc");

            migrationBuilder.DropForeignKey(
                name: "FK_TreBankPosUc_TreCarrencies_CurrencyId",
                table: "TreBankPosUc");

            migrationBuilder.DropForeignKey(
                name: "FK_TreCashBox_TreCarrencies_CurrencyId",
                table: "TreCashBox");

            migrationBuilder.DropForeignKey(
                name: "FK_TreCheckbook_BankAccounts_BankAccountId",
                table: "TreCheckbook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreCheckbook",
                table: "TreCheckbook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreCashBox",
                table: "TreCashBox");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreCarrencies",
                table: "TreCarrencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreBankPosUc",
                table: "TreBankPosUc");

            migrationBuilder.RenameTable(
                name: "TreCheckbook",
                newName: "TreCheckbooks");

            migrationBuilder.RenameTable(
                name: "TreCashBox",
                newName: "TreCashBoxes");

            migrationBuilder.RenameTable(
                name: "TreCarrencies",
                newName: "TreCurrencies");

            migrationBuilder.RenameTable(
                name: "TreBankPosUc",
                newName: "BankPosUcs");

            migrationBuilder.RenameIndex(
                name: "IX_TreCheckbook_BankAccountId",
                table: "TreCheckbooks",
                newName: "IX_TreCheckbooks_BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_TreCashBox_CurrencyId",
                table: "TreCashBoxes",
                newName: "IX_TreCashBoxes_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_TreBankPosUc_CurrencyId",
                table: "BankPosUcs",
                newName: "IX_BankPosUcs_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_TreBankPosUc_BankAccountId",
                table: "BankPosUcs",
                newName: "IX_BankPosUcs_BankAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreCheckbooks",
                table: "TreCheckbooks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreCashBoxes",
                table: "TreCashBoxes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreCurrencies",
                table: "TreCurrencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankPosUcs",
                table: "BankPosUcs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TreCashiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationUnit = table.Column<int>(type: "int", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    TafsilId = table.Column<long>(type: "bigint", nullable: false),
                    HasReceiptPermission = table.Column<bool>(type: "bit", nullable: false),
                    HasPaymentPermission = table.Column<bool>(type: "bit", nullable: false),
                    HasCheckIssuancePermission = table.Column<bool>(type: "bit", nullable: false),
                    HasCashboxEditPermission = table.Column<bool>(type: "bit", nullable: false),
                    CachboxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreCashiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreCashiers_TreCashBoxes_CashBoxId",
                        column: x => x.CashBoxId,
                        principalTable: "TreCashBoxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TreCashiers_parties_PersonId",
                        column: x => x.PersonId,
                        principalTable: "parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreChequeOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreChequeOperations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TreChequeOperations",
                columns: new[] { "Id", "Description", "OperationType" },
                values: new object[,]
                {
                    { 1, "دریافت چک", "چک دریافتی" },
                    { 2, "پرداخت چک", "چک پرداختی" },
                    { 3, "چک در جریان وصول", "در جریان وصول" },
                    { 4, "پاس چک", "پاس چک" },
                    { 5, "برگشت چک", "برگشت چک" },
                    { 6, "عودت چک برگشتی", "عودت چک برگشتی" },
                    { 7, "واخواست چک", "واخواست" },
                    { 8, "دریافت چک پاس نشده یا برگشتی", "دریافت چک پاس نشده یا برگشتی" },
                    { 9, "دریافت سفته", "سفته دریافتی" },
                    { 10, "پرداخت سفته", "سفته پرداختی" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreCashiers_CashBoxId",
                table: "TreCashiers",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_TreCashiers_PersonId",
                table: "TreCashiers",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankPosUcs_BankAccounts_BankAccountId",
                table: "BankPosUcs",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankPosUcs_TreCurrencies_CurrencyId",
                table: "BankPosUcs",
                column: "CurrencyId",
                principalTable: "TreCurrencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreCashBoxes_TreCurrencies_CurrencyId",
                table: "TreCashBoxes",
                column: "CurrencyId",
                principalTable: "TreCurrencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreCheckbooks_BankAccounts_BankAccountId",
                table: "TreCheckbooks",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankPosUcs_BankAccounts_BankAccountId",
                table: "BankPosUcs");

            migrationBuilder.DropForeignKey(
                name: "FK_BankPosUcs_TreCurrencies_CurrencyId",
                table: "BankPosUcs");

            migrationBuilder.DropForeignKey(
                name: "FK_TreCashBoxes_TreCurrencies_CurrencyId",
                table: "TreCashBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_TreCheckbooks_BankAccounts_BankAccountId",
                table: "TreCheckbooks");

            migrationBuilder.DropTable(
                name: "TreCashiers");

            migrationBuilder.DropTable(
                name: "TreChequeOperations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreCurrencies",
                table: "TreCurrencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreCheckbooks",
                table: "TreCheckbooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreCashBoxes",
                table: "TreCashBoxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankPosUcs",
                table: "BankPosUcs");

            migrationBuilder.RenameTable(
                name: "TreCurrencies",
                newName: "TreCarrencies");

            migrationBuilder.RenameTable(
                name: "TreCheckbooks",
                newName: "TreCheckbook");

            migrationBuilder.RenameTable(
                name: "TreCashBoxes",
                newName: "TreCashBox");

            migrationBuilder.RenameTable(
                name: "BankPosUcs",
                newName: "TreBankPosUc");

            migrationBuilder.RenameIndex(
                name: "IX_TreCheckbooks_BankAccountId",
                table: "TreCheckbook",
                newName: "IX_TreCheckbook_BankAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_TreCashBoxes_CurrencyId",
                table: "TreCashBox",
                newName: "IX_TreCashBox_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_BankPosUcs_CurrencyId",
                table: "TreBankPosUc",
                newName: "IX_TreBankPosUc_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_BankPosUcs_BankAccountId",
                table: "TreBankPosUc",
                newName: "IX_TreBankPosUc_BankAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreCarrencies",
                table: "TreCarrencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreCheckbook",
                table: "TreCheckbook",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreCashBox",
                table: "TreCashBox",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreBankPosUc",
                table: "TreBankPosUc",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreBankPosUc_BankAccounts_BankAccountId",
                table: "TreBankPosUc",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreBankPosUc_TreCarrencies_CurrencyId",
                table: "TreBankPosUc",
                column: "CurrencyId",
                principalTable: "TreCarrencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreCashBox_TreCarrencies_CurrencyId",
                table: "TreCashBox",
                column: "CurrencyId",
                principalTable: "TreCarrencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreCheckbook_BankAccounts_BankAccountId",
                table: "TreCheckbook",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
