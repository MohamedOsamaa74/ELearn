using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class EditMatrial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Materials",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "link",
                table: "Materials",
                newName: "Link");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Materials",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Materials",
                newName: "link");
        }
    }
}
