using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel140404123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_Branch_Acc_Coding_Tafsils_TafsilId",
                table: "Cu_Branch");

            migrationBuilder.DropIndex(
                name: "IX_Cu_Branch_TafsilId",
                table: "Cu_Branch");

            migrationBuilder.AddColumn<Guid>(
                name: "DocId",
                table: "TreTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserRemart",
                table: "TreTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Acc_Coding_TafsilId",
                table: "Cu_Branch",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PartyId",
                table: "Cu_Branch",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)9,
                column: "Name",
                value: "در حال توزیع");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_Branch_Acc_Coding_TafsilId",
                table: "Cu_Branch",
                column: "Acc_Coding_TafsilId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_Branch_PartyId",
                table: "Cu_Branch",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_Branch_Acc_Coding_Tafsils_Acc_Coding_TafsilId",
                table: "Cu_Branch",
                column: "Acc_Coding_TafsilId",
                principalTable: "Acc_Coding_Tafsils",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_Branch_parties_PartyId",
                table: "Cu_Branch",
                column: "PartyId",
                principalTable: "parties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_Branch_Acc_Coding_Tafsils_Acc_Coding_TafsilId",
                table: "Cu_Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_Branch_parties_PartyId",
                table: "Cu_Branch");

            migrationBuilder.DropIndex(
                name: "IX_Cu_Branch_Acc_Coding_TafsilId",
                table: "Cu_Branch");

            migrationBuilder.DropIndex(
                name: "IX_Cu_Branch_PartyId",
                table: "Cu_Branch");

            migrationBuilder.DropColumn(
                name: "DocId",
                table: "TreTransactions");

            migrationBuilder.DropColumn(
                name: "UserRemart",
                table: "TreTransactions");

            migrationBuilder.DropColumn(
                name: "Acc_Coding_TafsilId",
                table: "Cu_Branch");

            migrationBuilder.DropColumn(
                name: "PartyId",
                table: "Cu_Branch");

            migrationBuilder.UpdateData(
                table: "Cu_BillOfLadingStatuses",
                keyColumn: "Id",
                keyValue: (short)9,
                column: "Name",
                value: "تحویل سفیر جهت تحویل به گیرنده");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_Branch_TafsilId",
                table: "Cu_Branch",
                column: "TafsilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_Branch_Acc_Coding_Tafsils_TafsilId",
                table: "Cu_Branch",
                column: "TafsilId",
                principalTable: "Acc_Coding_Tafsils",
                principalColumn: "Id");
        }
    }
}
