using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScavHunt.Data.Migrations
{
    public partial class PlayerUserrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadgeNumber",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Players",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a3258f3-4a3e-40c8-997e-e04421c1210e",
                column: "ConcurrencyStamp",
                value: "f312aca4-e4a9-46fd-bba8-a26916eb5b63");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_AspNetUsers_UserId",
                table: "Players",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_AspNetUsers_UserId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_UserId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "BadgeNumber",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a3258f3-4a3e-40c8-997e-e04421c1210e",
                column: "ConcurrencyStamp",
                value: "4441d3cb-66b0-4efa-83d3-9641a2cee455");
        }
    }
}
