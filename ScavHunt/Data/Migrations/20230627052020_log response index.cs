using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScavHunt.Data.Migrations
{
    public partial class logresponseindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponseIndex",
                table: "Log",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseIndex",
                table: "Log");
        }
    }
}
