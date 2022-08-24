using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ChangeMechanic2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_MechanicSpecialities_SpecialityId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_MechanicSpecialities_Mechanics_MechanicId",
                table: "MechanicSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_MechanicSpecialities_MechanicId",
                table: "MechanicSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_SpecialityId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "MechanicId",
                table: "MechanicSpecialities");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "Mechanics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MechanicId",
                table: "MechanicSpecialities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialityId",
                table: "Mechanics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MechanicSpecialities_MechanicId",
                table: "MechanicSpecialities",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_Mechanics_SpecialityId",
                table: "Mechanics",
                column: "SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_MechanicSpecialities_SpecialityId",
                table: "Mechanics",
                column: "SpecialityId",
                principalTable: "MechanicSpecialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicSpecialities_Mechanics_MechanicId",
                table: "MechanicSpecialities",
                column: "MechanicId",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
