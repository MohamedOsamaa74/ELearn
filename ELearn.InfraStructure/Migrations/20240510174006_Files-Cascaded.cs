using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class FilesCascaded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Announcements_AnnouncementId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Assignments_AssignmentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Comments_CommentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_UserID",
                table: "Reacts");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Reacts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_UserID",
                table: "Reacts",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Announcements_AnnouncementId",
                table: "Files",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Assignments_AssignmentId",
                table: "Files",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Comments_CommentId",
                table: "Files",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Announcements_AnnouncementId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Assignments_AssignmentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Comments_CommentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_UserID",
                table: "Reacts");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Reacts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_UserID",
                table: "Reacts",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Announcements_AnnouncementId",
                table: "Files",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Assignments_AssignmentId",
                table: "Files",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Comments_CommentId",
                table: "Files",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Materials_MaterialId",
                table: "Files",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");
        }
    }
}
