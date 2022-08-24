using Microsoft.EntityFrameworkCore.Migrations;

namespace RepairshopWeb.Migrations
{
    public partial class ModifyMechanicAndMechanicSpeciality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicSpecialitys_AspNetUsers_UserId",
                table: "MechanicSpecialitys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MechanicSpecialitys",
                table: "MechanicSpecialitys");

            migrationBuilder.DropIndex(
                name: "IX_MechanicSpecialitys_UserId",
                table: "MechanicSpecialitys");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MechanicSpecialitys");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MechanicSpecialitys");

            migrationBuilder.RenameTable(
                name: "MechanicSpecialitys",
                newName: "MechanicSpecialities");

            migrationBuilder.RenameColumn(
                name: "SpecialityName",
                table: "MechanicSpecialities",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "MechanicId",
                table: "MechanicSpecialities",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MechanicSpecialities",
                table: "MechanicSpecialities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicSpecialities_MechanicId",
                table: "MechanicSpecialities",
                column: "MechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicSpecialities_Mechanics_MechanicId",
                table: "MechanicSpecialities",
                column: "MechanicId",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicSpecialities_Mechanics_MechanicId",
                table: "MechanicSpecialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MechanicSpecialities",
                table: "MechanicSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_MechanicSpecialities_MechanicId",
                table: "MechanicSpecialities");

            migrationBuilder.DropColumn(
                name: "MechanicId",
                table: "MechanicSpecialities");

            migrationBuilder.RenameTable(
                name: "MechanicSpecialities",
                newName: "MechanicSpecialitys");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MechanicSpecialitys",
                newName: "SpecialityName");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MechanicSpecialitys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MechanicSpecialitys",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MechanicSpecialitys",
                table: "MechanicSpecialitys",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicSpecialitys_UserId",
                table: "MechanicSpecialitys",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicSpecialitys_AspNetUsers_UserId",
                table: "MechanicSpecialitys",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
