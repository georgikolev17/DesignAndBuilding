using Microsoft.EntityFrameworkCore.Migrations;

namespace DesignAndBuilding.Data.Migrations
{
    public partial class MakeDesignerIdString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_DesignerId1",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_AspNetRoles_ArchitectId1",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ArchitectId1",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Bids_DesignerId1",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ArchitectId1",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "DesignerId1",
                table: "Bids");

            migrationBuilder.AlterColumn<string>(
                name: "ArchitectId",
                table: "Buildings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DesignerId",
                table: "Bids",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ArchitectId",
                table: "Buildings",
                column: "ArchitectId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_DesignerId",
                table: "Bids",
                column: "DesignerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_DesignerId",
                table: "Bids",
                column: "DesignerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_AspNetRoles_ArchitectId",
                table: "Buildings",
                column: "ArchitectId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_DesignerId",
                table: "Bids");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_AspNetRoles_ArchitectId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_ArchitectId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Bids_DesignerId",
                table: "Bids");

            migrationBuilder.AlterColumn<int>(
                name: "ArchitectId",
                table: "Buildings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ArchitectId1",
                table: "Buildings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DesignerId",
                table: "Bids",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "DesignerId1",
                table: "Bids",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ArchitectId1",
                table: "Buildings",
                column: "ArchitectId1");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_DesignerId1",
                table: "Bids",
                column: "DesignerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_DesignerId1",
                table: "Bids",
                column: "DesignerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_AspNetRoles_ArchitectId1",
                table: "Buildings",
                column: "ArchitectId1",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
