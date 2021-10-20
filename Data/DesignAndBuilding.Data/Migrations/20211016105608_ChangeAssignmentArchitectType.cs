using Microsoft.EntityFrameworkCore.Migrations;

namespace DesignAndBuilding.Data.Migrations
{
    public partial class ChangeAssignmentArchitectType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_AspNetRoles_ArchitectId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_AspNetUsers_ApplicationUserId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ApplicationUserId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Buildings");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_AspNetUsers_ArchitectId",
                table: "Buildings",
                column: "ArchitectId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_AspNetUsers_ArchitectId",
                table: "Buildings");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Buildings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ApplicationUserId",
                table: "Buildings",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_AspNetRoles_ArchitectId",
                table: "Buildings",
                column: "ArchitectId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_AspNetUsers_ApplicationUserId",
                table: "Buildings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
