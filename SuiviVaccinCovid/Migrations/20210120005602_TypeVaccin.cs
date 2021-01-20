using Microsoft.EntityFrameworkCore.Migrations;

namespace SuiviVaccinCovid.Migrations
{
    public partial class TypeVaccin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Vaccins");

            migrationBuilder.AddColumn<int>(
                name: "TypeVaccinId",
                table: "Vaccins",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeVaccin",
                columns: table => new
                {
                    TypeVaccinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeVaccin", x => x.TypeVaccinId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vaccins_TypeVaccinId",
                table: "Vaccins",
                column: "TypeVaccinId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccins_TypeVaccin_TypeVaccinId",
                table: "Vaccins",
                column: "TypeVaccinId",
                principalTable: "TypeVaccin",
                principalColumn: "TypeVaccinId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccins_TypeVaccin_TypeVaccinId",
                table: "Vaccins");

            migrationBuilder.DropTable(
                name: "TypeVaccin");

            migrationBuilder.DropIndex(
                name: "IX_Vaccins_TypeVaccinId",
                table: "Vaccins");

            migrationBuilder.DropColumn(
                name: "TypeVaccinId",
                table: "Vaccins");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Vaccins",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
