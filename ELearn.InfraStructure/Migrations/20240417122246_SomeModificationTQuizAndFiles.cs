using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class SomeModificationTQuizAndFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQuestions");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Files",
                newName: "ViewUrl");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectOption",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserAnswerQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswerQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswerQuestions_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAnswerQuestions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswerQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswerQuestions_ApplicationUserId",
                table: "UserAnswerQuestions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswerQuestions_OptionId",
                table: "UserAnswerQuestions",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswerQuestions_QuestionId",
                table: "UserAnswerQuestions",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAnswerQuestions");

            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "ViewUrl",
                table: "Files",
                newName: "Url");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Quizzes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "CorrectOption",
                table: "Questions",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestions_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserQuestions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestions_ApplicationUserId",
                table: "UserQuestions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestions_OptionId",
                table: "UserQuestions",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestions_QuestionId",
                table: "UserQuestions",
                column: "QuestionId");
        }
    }
}
