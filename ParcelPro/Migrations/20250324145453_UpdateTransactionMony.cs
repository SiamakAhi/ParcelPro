using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionMony : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OnlineGetwayName",
                table: "TreTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PosId",
                table: "TreTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreTransactions_BankAccountId",
                table: "TreTransactions",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TreTransactions_PosId",
                table: "TreTransactions",
                column: "PosId");

            migrationBuilder.AddForeignKey(
                name: "FK_TreTransactions_BankAccounts_BankAccountId",
                table: "TreTransactions",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreTransactions_BankPosUcs_PosId",
                table: "TreTransactions",
                column: "PosId",
                principalTable: "BankPosUcs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreTransactions_BankAccounts_BankAccountId",
                table: "TreTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TreTransactions_BankPosUcs_PosId",
                table: "TreTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TreTransactions_BankAccountId",
                table: "TreTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TreTransactions_PosId",
                table: "TreTransactions");

            migrationBuilder.DropColumn(
                name: "OnlineGetwayName",
                table: "TreTransactions");

            migrationBuilder.DropColumn(
                name: "PosId",
                table: "TreTransactions");
        }
    }
}
