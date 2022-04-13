using Microsoft.EntityFrameworkCore.Migrations;

namespace Recarro.Data.Migrations
{
    public partial class SwapIsAvailableWithCurrentUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "CurrentUser",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentUser",
                table: "Vehicles");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
