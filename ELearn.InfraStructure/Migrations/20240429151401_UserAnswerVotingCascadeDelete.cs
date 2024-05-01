using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class UserAnswerVotingCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswerVotings_Votings_VotingId",
                table: "UserAnswerVotings");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswerVotings_Votings_VotingId",
                table: "UserAnswerVotings",
                column: "VotingId",
                principalTable: "Votings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswerVotings_Votings_VotingId",
                table: "UserAnswerVotings");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswerVotings_Votings_VotingId",
                table: "UserAnswerVotings",
                column: "VotingId",
                principalTable: "Votings",
                principalColumn: "Id");
        }
    }
}
