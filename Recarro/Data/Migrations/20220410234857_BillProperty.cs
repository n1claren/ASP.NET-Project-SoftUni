using Microsoft.EntityFrameworkCore.Migrations;

namespace Recarro.Data.Migrations
{
    public partial class BillProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_AspNetUsers_UserId1",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_UserId",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_UserId1",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Rents");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_UserId",
                table: "Rents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_UserId",
                table: "Rents");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Rents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rents_UserId",
                table: "Rents",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rents_UserId1",
                table: "Rents",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_AspNetUsers_UserId1",
                table: "Rents",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
