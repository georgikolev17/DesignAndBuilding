using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAndBuilding.Data.Migrations
{
    public partial class RemoveColumnAssignmentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentType",
                table: "Assignments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentType",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
