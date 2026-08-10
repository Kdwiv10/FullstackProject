using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullStack_Shangri.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipIdToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adding the MembershipID column to the AspNetUsers table
            migrationBuilder.AddColumn<int>(
                name: "MembershipId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            // Adding foreign key relationship between AspNetUsers and Memberships
            migrationBuilder.AddForeignKey(
                name: "FK__AspNetUse__Membe__73852659",
                table: "AspNetUsers",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "MembershipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the foreign key
            migrationBuilder.DropForeignKey(
                name: "FK__AspNetUse__Membe__73852659",
                table: "AspNetUsers");

            // Remove the MembershipID column
            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "AspNetUsers");
        }
    }
}
