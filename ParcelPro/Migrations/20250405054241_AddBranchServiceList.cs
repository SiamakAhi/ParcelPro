using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchServiceList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cu_BranchServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Cu_ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_BranchServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cu_BranchServices_Cu_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Cu_Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cu_BranchServices_Cu_Services_Cu_ServiceId",
                        column: x => x.Cu_ServiceId,
                        principalTable: "Cu_Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BranchServices_BranchId",
                table: "Cu_BranchServices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BranchServices_Cu_ServiceId",
                table: "Cu_BranchServices",
                column: "Cu_ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cu_BranchServices");
        }
    }
}
