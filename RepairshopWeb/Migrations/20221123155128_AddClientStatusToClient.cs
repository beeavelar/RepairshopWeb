using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class AddClientStatusToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientStatus",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientStatus",
                table: "Clients");
        }
    }
}
