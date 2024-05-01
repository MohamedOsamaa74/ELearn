using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class CreationDateandIsActiveAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Votings",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "DateJoined",
                table: "UserGroups",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "RefreshToken",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Reacts",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Posts",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Creation",
                table: "Files",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Departments",
                newName: "Title");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "UserAnswerVotings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "UserAnswerSurveys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "UserAnswerQuizziz",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "UserAnswerQuestions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "UserAnswerAssignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Surveys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "UserAnswerVotings");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "UserAnswerSurveys");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "UserAnswerQuizziz");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "UserAnswerQuestions");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "UserAnswerAssignments");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Votings",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "UserGroups",
                newName: "DateJoined");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "RefreshToken",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Reacts",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Posts",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Files",
                newName: "Creation");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Departments",
                newName: "title");
        }
    }
}
