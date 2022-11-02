using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class AddAppointmentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentStatus",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentStatus",
                table: "Appointments");
        }
    }
}
