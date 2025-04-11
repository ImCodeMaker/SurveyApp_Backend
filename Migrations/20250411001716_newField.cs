using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyApp_Server.Migrations
{
    /// <inheritdoc />
    public partial class newField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Surveys_Survey_Id",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Survey_Id",
                table: "Questions",
                newName: "SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_Survey_Id",
                table: "Questions",
                newName: "IX_Questions_SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                table: "Questions",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Surveys_SurveyId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "Questions",
                newName: "Survey_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_SurveyId",
                table: "Questions",
                newName: "IX_Questions_Survey_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Surveys_Survey_Id",
                table: "Questions",
                column: "Survey_Id",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
