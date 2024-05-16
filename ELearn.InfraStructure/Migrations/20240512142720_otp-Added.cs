using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearn.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class otpAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "ApplicationUser",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPExpiry",
                table: "ApplicationUser",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTP",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "OTPExpiry",
                table: "ApplicationUser");
        }
    }
}
