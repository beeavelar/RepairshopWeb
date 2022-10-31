using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ChangeAppointmentAndRepairOrderAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairOrders_Appointments_AppointmentId",
                table: "RepairOrders");

            migrationBuilder.DropIndex(
                name: "IX_RepairOrders_AppointmentId",
                table: "RepairOrders");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "RepairOrders");

            migrationBuilder.AddColumn<int>(
                name: "RepairOrderId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RepairOrderId",
                table: "Appointments",
                column: "RepairOrderId",
                unique: true,
                filter: "[RepairOrderId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_RepairOrders_RepairOrderId",
                table: "Appointments",
                column: "RepairOrderId",
                principalTable: "RepairOrders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_RepairOrders_RepairOrderId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_RepairOrderId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RepairOrderId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "RepairOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrders_AppointmentId",
                table: "RepairOrders",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairOrders_Appointments_AppointmentId",
                table: "RepairOrders",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
