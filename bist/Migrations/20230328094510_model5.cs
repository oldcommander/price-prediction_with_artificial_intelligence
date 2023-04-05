﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bist.Migrations
{
    /// <inheritdoc />
    public partial class model5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kap",
                table: "hisses");

            migrationBuilder.DropColumn(
                name: "Olay",
                table: "hisses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Kap",
                table: "hisses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Olay",
                table: "hisses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
