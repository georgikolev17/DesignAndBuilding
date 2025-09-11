using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAndBuilding.Data.Migrations
{
    public partial class DescriptionFiletablenowhasonlymetadataofthefiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "DescriptionFiles");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "DescriptionFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "DescriptionFiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "DescriptionFiles",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
