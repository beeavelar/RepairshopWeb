using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ModifyBilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billings_RepairOrders_TotalToPayId",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_TotalToPayId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "TotalToPayId",
                table: "Billings");

            migrationBuilder.AddColumn<int>(
                name: "Nif",
                table: "Billings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalToPay",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nif",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "TotalToPay",
                table: "Billings");

            migrationBuilder.AddColumn<int>(
                name: "TotalToPayId",
                table: "Billings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Billings_TotalToPayId",
                table: "Billings",
                column: "TotalToPayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_RepairOrders_TotalToPayId",
                table: "Billings",
                column: "TotalToPayId",
                principalTable: "RepairOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
