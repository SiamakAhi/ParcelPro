using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class DistinationServiceMOdel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cu_DistributionServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    PartnerId = table.Column<long>(type: "bigint", nullable: false),
                    Shomare = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mablagh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RouteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Karaye = table.Column<long>(type: "bigint", nullable: true),
                    TambreVaBarnam = table.Column<long>(type: "bigint", nullable: false),
                    BasteBandi = table.Column<long>(type: "bigint", nullable: false),
                    Motafavvete = table.Column<long>(type: "bigint", nullable: false),
                    Towzi = table.Column<long>(type: "bigint", nullable: false),
                    MotafavveteMobad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JamAvary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhabelePardakht = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnvanFerestande = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnvanGirande = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SettelmentTypeId = table.Column<short>(type: "smallint", nullable: false),
                    CreditCustomer = table.Column<long>(type: "bigint", nullable: true),
                    SharePercentage = table.Column<float>(type: "real", nullable: true),
                    tg_DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tg_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tg_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tg_NationalityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tg_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tg_SignatureData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    tg_CourierManUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Delivered = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    DataEntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cu_DistributionServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cu_DistributionServices_Cu_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Cu_Invoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cu_DistributionServices_InvoiceId",
                table: "Cu_DistributionServices",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cu_DistributionServices");
        }
    }
}
