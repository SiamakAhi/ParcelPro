using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTreasuryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreBankPosUc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    TerminalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TafsilId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreBankPosUc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreBankPosUc_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreBankPosUc_TreCarrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TreCarrencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TreCashBox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicalLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    DetailedAccountId = table.Column<long>(type: "bigint", nullable: true),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    CurrencyRate = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreCashBox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreCashBox_TreCarrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TreCarrencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TreCheckbook",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    CheckbookNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    FirstCheckSerial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastCheckSerial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfChecks = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreCheckbook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreCheckbook_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreBankPosUc_BankAccountId",
                table: "TreBankPosUc",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TreBankPosUc_CurrencyId",
                table: "TreBankPosUc",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TreCashBox_CurrencyId",
                table: "TreCashBox",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TreCheckbook_BankAccountId",
                table: "TreCheckbook",
                column: "BankAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreBankPosUc");

            migrationBuilder.DropTable(
                name: "TreCashBox");

            migrationBuilder.DropTable(
                name: "TreCheckbook");
        }
    }
}
