using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScavHunt.Data.Migrations
{
    public partial class Addpointtransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PointTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: true),
                    Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointTransactions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointTransactions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_PlayerId",
                table: "PointTransactions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_QuestionId",
                table: "PointTransactions",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointTransactions");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
