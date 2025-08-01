﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPro.Migrations
{
    /// <inheritdoc />
    public partial class UpdataBranchEntity3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SellerId",
                table: "Cu_Branch",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Cu_Branch");
        }
    }
}
