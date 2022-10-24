using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ModifyAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AlertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetailTemps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AlertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetailTemps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentDetailTemps_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentDetailTemps_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentDetailTemps_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RepairOrderId",
                table: "Appointments",
                column: "RepairOrderId",
                unique: true,
                filter: "[RepairOrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_ClientId",
                table: "AppointmentDetails",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_VehicleId",
                table: "AppointmentDetails",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetailTemps_ClientId",
                table: "AppointmentDetailTemps",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetailTemps_UserId",
                table: "AppointmentDetailTemps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetailTemps_VehicleId",
                table: "AppointmentDetailTemps",
                column: "VehicleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_RepairOrders_RepairOrderId",
                table: "Appointments",
                column: "RepairOrderId",
                principalTable: "RepairOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_RepairOrders_RepairOrderId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "AppointmentDetails");

            migrationBuilder.DropTable(
                name: "AppointmentDetailTemps");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_RepairOrderId",
                table: "Appointments");
        }
    }
}
