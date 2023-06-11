using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScavHunt.Data.Migrations
{
    public partial class prizes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrizeTransaction_AspNetUsers_UserId",
                table: "PrizeTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_PrizeTransaction_Prizes_PrizeId",
                table: "PrizeTransaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrizeTransaction",
                table: "PrizeTransaction");

            migrationBuilder.RenameTable(
                name: "PrizeTransaction",
                newName: "PrizeTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_PrizeTransaction_UserId",
                table: "PrizeTransactions",
                newName: "IX_PrizeTransactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PrizeTransaction_PrizeId",
                table: "PrizeTransactions",
                newName: "IX_PrizeTransactions_PrizeId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PrizeTransactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrizeTransactions",
                table: "PrizeTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeTransactions_AspNetUsers_UserId",
                table: "PrizeTransactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeTransactions_Prizes_PrizeId",
                table: "PrizeTransactions",
                column: "PrizeId",
                principalTable: "Prizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrizeTransactions_AspNetUsers_UserId",
                table: "PrizeTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PrizeTransactions_Prizes_PrizeId",
                table: "PrizeTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrizeTransactions",
                table: "PrizeTransactions");

            migrationBuilder.RenameTable(
                name: "PrizeTransactions",
                newName: "PrizeTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_PrizeTransactions_UserId",
                table: "PrizeTransaction",
                newName: "IX_PrizeTransaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PrizeTransactions_PrizeId",
                table: "PrizeTransaction",
                newName: "IX_PrizeTransaction_PrizeId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PrizeTransaction",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrizeTransaction",
                table: "PrizeTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeTransaction_AspNetUsers_UserId",
                table: "PrizeTransaction",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeTransaction_Prizes_PrizeId",
                table: "PrizeTransaction",
                column: "PrizeId",
                principalTable: "Prizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
