using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ModifyMechanicAddSpeciality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Speciality",
                table: "Mechanics",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speciality",
                table: "Mechanics");
        }
    }
}
