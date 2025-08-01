using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class BranchServiceConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BranchServices_Cu_Branch_BranchId",
                table: "Cu_BranchServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BranchServices_Cu_Services_Cu_ServiceId",
                table: "Cu_BranchServices");

            migrationBuilder.DropIndex(
                name: "IX_Cu_BranchServices_Cu_ServiceId",
                table: "Cu_BranchServices");

            migrationBuilder.DropColumn(
                name: "Cu_ServiceId",
                table: "Cu_BranchServices");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BranchServices_ServiceId",
                table: "Cu_BranchServices",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BranchServices_Cu_Branch_BranchId",
                table: "Cu_BranchServices",
                column: "BranchId",
                principalTable: "Cu_Branch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BranchServices_Cu_Services_ServiceId",
                table: "Cu_BranchServices",
                column: "ServiceId",
                principalTable: "Cu_Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BranchServices_Cu_Branch_BranchId",
                table: "Cu_BranchServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BranchServices_Cu_Services_ServiceId",
                table: "Cu_BranchServices");

            migrationBuilder.DropIndex(
                name: "IX_Cu_BranchServices_ServiceId",
                table: "Cu_BranchServices");

            migrationBuilder.AddColumn<int>(
                name: "Cu_ServiceId",
                table: "Cu_BranchServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BranchServices_Cu_ServiceId",
                table: "Cu_BranchServices",
                column: "Cu_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BranchServices_Cu_Branch_BranchId",
                table: "Cu_BranchServices",
                column: "BranchId",
                principalTable: "Cu_Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BranchServices_Cu_Services_Cu_ServiceId",
                table: "Cu_BranchServices",
                column: "Cu_ServiceId",
                principalTable: "Cu_Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
