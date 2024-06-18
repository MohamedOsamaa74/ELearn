using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Start",
                table: "Assignments");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "UserAnswerAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "UserAnswerAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Assignments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "UserAnswerAssignments");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "UserAnswerAssignments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Assignments");

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
