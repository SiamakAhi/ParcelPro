using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cu_FinancialTransactionTypes");

            migrationBuilder.DropTable(
                name: "Cu_PaymentTypes");

            migrationBuilder.RenameColumn(
                name: "TransactionTypeId",
                table: "Cu_FinancialTransactions",
                newName: "OperationId");

            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Cu_FinancialTransactions",
                newName: "SettlementTypeId");

            migrationBuilder.AddColumn<long>(
                name: "AccountPartyId",
                table: "Cu_FinancialTransactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "Cu_FinancialTransactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Bed",
                table: "Cu_FinancialTransactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Bes",
                table: "Cu_FinancialTransactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "BillOfLadingId",
                table: "Cu_FinancialTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "Cu_FinancialTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cu_FinancialTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cu_FinancialTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "SellerId",
                table: "Cu_FinancialTransactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "Cu_FinancialTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TransactionTime",
                table: "Cu_FinancialTransactions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Cu_FinancialTransactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "Code",
                table: "Cu_FinancialTransactionOperations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cu_FinancialTransactionOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TreOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPOSTransaction = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPay = table.Column<bool>(type: "bit", nullable: false),
                    UserAlowSelect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BillOfLadingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountPartyId = table.Column<long>(type: "bigint", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    DebitAmount = table.Column<long>(type: "bigint", nullable: false),
                    CreditAmount = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: true),
                    TransferNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentGatewayTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreTransactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TreTransactions_Cu_BillOfLadings_BillOfLadingId",
                        column: x => x.BillOfLadingId,
                        principalTable: "Cu_BillOfLadings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TreTransactions_TreOperations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "TreOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TreTransactions_parties_AccountPartyId",
                        column: x => x.AccountPartyId,
                        principalTable: "parties",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Cu_FinancialTransactionOperations",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { (short)1, (short)1, "فروش بار" },
                    { (short)2, (short)2, "فروش خدمات توزیع بار" },
                    { (short)3, (short)3, "پورسانت فروش" },
                    { (short)4, (short)4, "دریافت خدمات توزیع و جمع آوری بار" }
                });

            migrationBuilder.InsertData(
                table: "TreOperations",
                columns: new[] { "Id", "IsPay", "OperationName", "OperationType", "UserAlowSelect" },
                values: new object[] { 1, false, "دریافت وجه نقد", 1, true });

            migrationBuilder.InsertData(
                table: "TreOperations",
                columns: new[] { "Id", "IsPOSTransaction", "IsPay", "OperationName", "OperationType", "UserAlowSelect" },
                values: new object[] { 2, true, false, "واریز با کارتخوان (POS)", 2, true });

            migrationBuilder.InsertData(
                table: "TreOperations",
                columns: new[] { "Id", "IsPay", "OperationName", "OperationType", "UserAlowSelect" },
                values: new object[,]
                {
                    { 3, false, "حواله بانکی", 3, true },
                    { 4, false, "دریافت چک", 4, true },
                    { 5, false, "تهاتر", 5, false },
                    { 6, false, "واریز از طریق درگاه الکترونیک", 6, false },
                    { 7, true, "پرداخت وجه نقد", 20, true },
                    { 8, true, "واریز به حساب", 21, true },
                    { 9, true, "حواله", 22, true },
                    { 10, true, "پرداخت چک", 23, true },
                    { 11, true, "پرداخت از طریق درگاه الکترونیک", 24, false }
                });

            migrationBuilder.InsertData(
                table: "TreOperations",
                columns: new[] { "Id", "IsPOSTransaction", "IsPay", "OperationName", "OperationType", "UserAlowSelect" },
                values: new object[] { 12, true, false, "پرداخت با کارتخوان (POS)", 25, true });

            migrationBuilder.CreateIndex(
                name: "IX_Cu_FinancialTransactions_AccountPartyId",
                table: "Cu_FinancialTransactions",
                column: "AccountPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_FinancialTransactions_BillOfLadingId",
                table: "Cu_FinancialTransactions",
                column: "BillOfLadingId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_FinancialTransactions_BranchId",
                table: "Cu_FinancialTransactions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_FinancialTransactions_OperationId",
                table: "Cu_FinancialTransactions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_FinancialTransactions_UserId",
                table: "Cu_FinancialTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TreTransactions_AccountPartyId",
                table: "TreTransactions",
                column: "AccountPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_TreTransactions_BillOfLadingId",
                table: "TreTransactions",
                column: "BillOfLadingId");

            migrationBuilder.CreateIndex(
                name: "IX_TreTransactions_OperationId",
                table: "TreTransactions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_TreTransactions_UserId",
                table: "TreTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_FinancialTransactions_AspNetUsers_UserId",
                table: "Cu_FinancialTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_FinancialTransactions_Cu_BillOfLadings_BillOfLadingId",
                table: "Cu_FinancialTransactions",
                column: "BillOfLadingId",
                principalTable: "Cu_BillOfLadings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_FinancialTransactions_Cu_Branch_BranchId",
                table: "Cu_FinancialTransactions",
                column: "BranchId",
                principalTable: "Cu_Branch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_FinancialTransactions_Cu_FinancialTransactionOperations_OperationId",
                table: "Cu_FinancialTransactions",
                column: "OperationId",
                principalTable: "Cu_FinancialTransactionOperations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_FinancialTransactions_parties_AccountPartyId",
                table: "Cu_FinancialTransactions",
                column: "AccountPartyId",
                principalTable: "parties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_FinancialTransactions_AspNetUsers_UserId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_FinancialTransactions_Cu_BillOfLadings_BillOfLadingId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_FinancialTransactions_Cu_Branch_BranchId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_FinancialTransactions_Cu_FinancialTransactionOperations_OperationId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_FinancialTransactions_parties_AccountPartyId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropTable(
                name: "TreTransactions");

            migrationBuilder.DropTable(
                name: "TreOperations");

            migrationBuilder.DropIndex(
                name: "IX_Cu_FinancialTransactions_AccountPartyId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Cu_FinancialTransactions_BillOfLadingId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Cu_FinancialTransactions_BranchId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Cu_FinancialTransactions_OperationId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Cu_FinancialTransactions_UserId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DeleteData(
                table: "Cu_FinancialTransactionOperations",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "Cu_FinancialTransactionOperations",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "Cu_FinancialTransactionOperations",
                keyColumn: "Id",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                table: "Cu_FinancialTransactionOperations",
                keyColumn: "Id",
                keyValue: (short)4);

            migrationBuilder.DropColumn(
                name: "AccountPartyId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "Bed",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "Bes",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "BillOfLadingId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionTime",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Cu_FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Cu_FinancialTransactionOperations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cu_FinancialTransactionOperations");

            migrationBuilder.RenameColumn(
                name: "SettlementTypeId",
                table: "Cu_FinancialTransactions",
                newName: "MyProperty");

            migrationBuilder.RenameColumn(
                name: "OperationId",
                table: "Cu_FinancialTransactions",
                newName: "TransactionTypeId");

            migrationBuilder.CreateTable(
                name: "Cu_FinancialTransactionTypes",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_FinancialTransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cu_PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_PaymentTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cu_PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "نقد" },
                    { (short)2, "واریز به حساب" },
                    { (short)3, "کارتخوان" },
                    { (short)4, "درگاه پرداخت" },
                    { (short)5, "چک" }
                });
        }
    }
}
