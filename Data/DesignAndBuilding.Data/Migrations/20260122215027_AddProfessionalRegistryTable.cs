using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesignAndBuilding.Data.Migrations
{
    public partial class AddProfessionalRegistryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfessionalRegistries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Discipline = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalRegistries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalRegistries_RegistrationNumber",
                table: "ProfessionalRegistries",
                column: "RegistrationNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessionalRegistries");
        }
    }
}
