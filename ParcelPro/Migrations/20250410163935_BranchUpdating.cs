using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class BranchUpdating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AllowdDiscountRate",
                table: "Cu_Branch",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DistShare",
                table: "Cu_Branch",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDistShareFixed",
                table: "Cu_Branch",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIssueShareFixed",
                table: "Cu_Branch",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "IssueShare",
                table: "Cu_Branch",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowdDiscountRate",
                table: "Cu_Branch");

            migrationBuilder.DropColumn(
                name: "DistShare",
                table: "Cu_Branch");

            migrationBuilder.DropColumn(
                name: "IsDistShareFixed",
                table: "Cu_Branch");

            migrationBuilder.DropColumn(
                name: "IsIssueShareFixed",
                table: "Cu_Branch");

            migrationBuilder.DropColumn(
                name: "IssueShare",
                table: "Cu_Branch");
        }
    }
}
