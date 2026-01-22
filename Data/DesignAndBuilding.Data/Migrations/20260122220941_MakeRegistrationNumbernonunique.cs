using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAndBuilding.Data.Migrations
{
    public partial class MakeRegistrationNumbernonunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProfessionalRegistries_RegistrationNumber",
                table: "ProfessionalRegistries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalRegistries_RegistrationNumber",
                table: "ProfessionalRegistries",
                column: "RegistrationNumber",
                unique: true);
        }
    }
}
