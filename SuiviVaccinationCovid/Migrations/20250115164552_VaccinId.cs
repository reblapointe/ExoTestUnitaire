using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuiviVaccinationCovid.Migrations
{
    /// <inheritdoc />
    public partial class VaccinId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doses_Vaccins_VaccinId",
                table: "Doses");

            migrationBuilder.AlterColumn<int>(
                name: "VaccinId",
                table: "Doses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doses_Vaccins_VaccinId",
                table: "Doses",
                column: "VaccinId",
                principalTable: "Vaccins",
                principalColumn: "VaccinId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doses_Vaccins_VaccinId",
                table: "Doses");

            migrationBuilder.AlterColumn<int>(
                name: "VaccinId",
                table: "Doses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Doses_Vaccins_VaccinId",
                table: "Doses",
                column: "VaccinId",
                principalTable: "Vaccins",
                principalColumn: "VaccinId");
        }
    }
}
