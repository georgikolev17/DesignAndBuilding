using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DesignAndBuilding.Data.Migrations
{
    public partial class AddContentTypeToDescFileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "DescriptionFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "DescriptionFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "DescriptionFiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DescriptionFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "DescriptionFiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DescriptionFiles_IsDeleted",
                table: "DescriptionFiles",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DescriptionFiles_IsDeleted",
                table: "DescriptionFiles");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "DescriptionFiles");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "DescriptionFiles");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "DescriptionFiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DescriptionFiles");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "DescriptionFiles");
        }
    }
}
