using Microsoft.EntityFrameworkCore.Migrations;

namespace DesignAndBuilding.Data.Migrations
{
    public partial class AddDifferentUserTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DesignerType",
                table: "Assignments",
                newName: "UserType");

            migrationBuilder.RenameColumn(
                name: "DesignerType",
                table: "AspNetUsers",
                newName: "UserType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Assignments",
                newName: "DesignerType");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "AspNetUsers",
                newName: "DesignerType");
        }
    }
}
