using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SuiviVaccinCovid.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeVaccins",
                columns: table => new
                {
                    TypeVaccinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeVaccins", x => x.TypeVaccinId);
                });

            migrationBuilder.CreateTable(
                name: "Vaccins",
                columns: table => new
                {
                    VaccinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NAMPatient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeVaccinId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccins", x => x.VaccinId);
                    table.ForeignKey(
                        name: "FK_Vaccins_TypeVaccins_TypeVaccinId",
                        column: x => x.TypeVaccinId,
                        principalTable: "TypeVaccins",
                        principalColumn: "TypeVaccinId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vaccins_TypeVaccinId",
                table: "Vaccins",
                column: "TypeVaccinId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vaccins");

            migrationBuilder.DropTable(
                name: "TypeVaccins");
        }
    }
}
