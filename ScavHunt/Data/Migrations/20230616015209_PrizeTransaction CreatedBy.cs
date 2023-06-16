using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScavHunt.Data.Migrations
{
    public partial class PrizeTransactionCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "PrizeTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrizeTransactions_CreatedById",
                table: "PrizeTransactions",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeTransactions_AspNetUsers_CreatedById",
                table: "PrizeTransactions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrizeTransactions_AspNetUsers_CreatedById",
                table: "PrizeTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PrizeTransactions_CreatedById",
                table: "PrizeTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "PrizeTransactions");
        }
    }
}
