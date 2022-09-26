using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ModifyVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Vehicles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
