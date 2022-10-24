using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class AddItemsToAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "AppointmentDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_AppointmentId",
                table: "AppointmentDetails",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetails_Appointments_AppointmentId",
                table: "AppointmentDetails",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetails_Appointments_AppointmentId",
                table: "AppointmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetails_AppointmentId",
                table: "AppointmentDetails");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "AppointmentDetails");
        }
    }
}
