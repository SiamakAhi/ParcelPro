using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class BusinessPartnerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerId",
                table: "Cu_BillOfLadings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Cu_BillOfLadings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostDescription",
                table: "Cu_BillCosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cu_BusinessPartners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EconomicCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_BusinessPartners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BillOfLadings_BusinessPartnerId",
                table: "Cu_BillOfLadings",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cu_BillOfLadings_ContractId",
                table: "Cu_BillOfLadings",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_BusinessPartners_BusinessPartnerId",
                table: "Cu_BillOfLadings",
                column: "BusinessPartnerId",
                principalTable: "Cu_BusinessPartners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_SaleContracts_ContractId",
                table: "Cu_BillOfLadings",
                column: "ContractId",
                principalTable: "Cu_SaleContracts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_BusinessPartners_BusinessPartnerId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropForeignKey(
                name: "FK_Cu_BillOfLadings_Cu_SaleContracts_ContractId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropTable(
                name: "Cu_BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_Cu_BillOfLadings_BusinessPartnerId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropIndex(
                name: "IX_Cu_BillOfLadings_ContractId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "BusinessPartnerId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Cu_BillOfLadings");

            migrationBuilder.DropColumn(
                name: "CostDescription",
                table: "Cu_BillCosts");
        }
    }
}
