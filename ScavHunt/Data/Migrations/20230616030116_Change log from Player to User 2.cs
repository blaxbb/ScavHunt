using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScavHunt.Data.Migrations
{
    public partial class ChangelogfromPlayertoUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Players_PlayerId",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_PlayerId",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Log");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlayerId",
                table: "Log",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_PlayerId",
                table: "Log",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Players_PlayerId",
                table: "Log",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
