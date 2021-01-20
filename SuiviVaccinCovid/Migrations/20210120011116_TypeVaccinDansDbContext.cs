using Microsoft.EntityFrameworkCore.Migrations;

namespace SuiviVaccinCovid.Migrations
{
    public partial class TypeVaccinDansDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccins_TypeVaccin_TypeVaccinId",
                table: "Vaccins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeVaccin",
                table: "TypeVaccin");

            migrationBuilder.RenameTable(
                name: "TypeVaccin",
                newName: "TypesVaccin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypesVaccin",
                table: "TypesVaccin",
                column: "TypeVaccinId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccins_TypesVaccin_TypeVaccinId",
                table: "Vaccins",
                column: "TypeVaccinId",
                principalTable: "TypesVaccin",
                principalColumn: "TypeVaccinId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccins_TypesVaccin_TypeVaccinId",
                table: "Vaccins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypesVaccin",
                table: "TypesVaccin");

            migrationBuilder.RenameTable(
                name: "TypesVaccin",
                newName: "TypeVaccin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeVaccin",
                table: "TypeVaccin",
                column: "TypeVaccinId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccins_TypeVaccin_TypeVaccinId",
                table: "Vaccins",
                column: "TypeVaccinId",
                principalTable: "TypeVaccin",
                principalColumn: "TypeVaccinId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
