using Microsoft.EntityFrameworkCore.Migrations;

namespace DesignAndBuilding.Data.Migrations
{
    public partial class AddIsNewPropertyToNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Notifications");
        }
    }
}
