using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScavHunt.Data.Migrations
{
    public partial class Changequestiontosingleparent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_QuestionId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Questions",
                newName: "ParentQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuestionId",
                table: "Questions",
                newName: "IX_Questions_ParentQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_ParentQuestionId",
                table: "Questions",
                column: "ParentQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_ParentQuestionId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "ParentQuestionId",
                table: "Questions",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_ParentQuestionId",
                table: "Questions",
                newName: "IX_Questions_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_QuestionId",
                table: "Questions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
