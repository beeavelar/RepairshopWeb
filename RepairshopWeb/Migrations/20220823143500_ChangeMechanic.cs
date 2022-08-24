using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ChangeMechanic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecialityId",
                table: "Mechanics",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_MechanicSpecialities_SpecialityId",
                table: "Mechanics");

            migrationBuilder.DropIndex(
                name: "IX_Mechanics_SpecialityId",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "Mechanics");
        }
    }
}
