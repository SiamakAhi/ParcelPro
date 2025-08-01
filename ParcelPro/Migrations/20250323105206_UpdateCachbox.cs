using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCachbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TreCashBoxes_BranchId",
                table: "TreCashBoxes",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TreCashBoxes_DetailedAccountId",
                table: "TreCashBoxes",
                column: "DetailedAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TreCashBoxes_Acc_Coding_Tafsils_DetailedAccountId",
                table: "TreCashBoxes",
                column: "DetailedAccountId",
                principalTable: "Acc_Coding_Tafsils",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TreCashBoxes_Cu_Branch_BranchId",
                table: "TreCashBoxes",
                column: "BranchId",
                principalTable: "Cu_Branch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreCashBoxes_Acc_Coding_Tafsils_DetailedAccountId",
                table: "TreCashBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_TreCashBoxes_Cu_Branch_BranchId",
                table: "TreCashBoxes");

            migrationBuilder.DropIndex(
                name: "IX_TreCashBoxes_BranchId",
                table: "TreCashBoxes");

            migrationBuilder.DropIndex(
                name: "IX_TreCashBoxes_DetailedAccountId",
                table: "TreCashBoxes");
        }
    }
}
