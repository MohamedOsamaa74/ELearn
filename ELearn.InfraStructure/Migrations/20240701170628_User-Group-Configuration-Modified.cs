using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class UserGroupConfigurationModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_ApplicationUser_CreatorId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_ApplicationUser_UserId",
                table: "UserGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_ApplicationUser_CreatorId",
                table: "Groups",
                column: "CreatorId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_ApplicationUser_UserId",
                table: "UserGroups",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_ApplicationUser_CreatorId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_ApplicationUser_UserId",
                table: "UserGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_ApplicationUser_CreatorId",
                table: "Groups",
                column: "CreatorId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_ApplicationUser_UserId",
                table: "UserGroups",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
