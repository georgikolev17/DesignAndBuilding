using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAndBuilding.Data.Migrations
{
    public partial class AddAssignmentIdColumnToNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AssignmentId",
                table: "Notifications",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Assignments_AssignmentId",
                table: "Notifications",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Assignments_AssignmentId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AssignmentId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Notifications");
        }
    }
}
