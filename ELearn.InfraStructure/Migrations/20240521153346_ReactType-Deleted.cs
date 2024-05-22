using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class ReactTypeDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Reacts",
                newName: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_CommentId",
                table: "Reacts",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_Comments_CommentId",
                table: "Reacts",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_Comments_CommentId",
                table: "Reacts");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_CommentId",
                table: "Reacts");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Reacts",
                newName: "Type");
        }
    }
}
