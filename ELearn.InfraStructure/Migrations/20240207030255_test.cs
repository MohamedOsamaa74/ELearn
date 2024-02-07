using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSurvey_Surveys_SurveysSurveyId",
                table: "GroupSurvey");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "Surveys",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ReactId",
                table: "Reacts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SurveysSurveyId",
                table: "GroupSurvey",
                newName: "SurveysId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSurvey_SurveysSurveyId",
                table: "GroupSurvey",
                newName: "IX_GroupSurvey_SurveysId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Groups",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "GroupAnnouncmentId",
                table: "GroupAnnouncments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Departments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AnnouncementId",
                table: "Announcements",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSurvey_Surveys_SurveysId",
                table: "GroupSurvey",
                column: "SurveysId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSurvey_Surveys_SurveysId",
                table: "GroupSurvey");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Surveys",
                newName: "SurveyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reacts",
                newName: "ReactId");

            migrationBuilder.RenameColumn(
                name: "SurveysId",
                table: "GroupSurvey",
                newName: "SurveysSurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupSurvey_SurveysId",
                table: "GroupSurvey",
                newName: "IX_GroupSurvey_SurveysSurveyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Groups",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GroupAnnouncments",
                newName: "GroupAnnouncmentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departments",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Announcements",
                newName: "AnnouncementId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSurvey_Surveys_SurveysSurveyId",
                table: "GroupSurvey",
                column: "SurveysSurveyId",
                principalTable: "Surveys",
                principalColumn: "SurveyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
