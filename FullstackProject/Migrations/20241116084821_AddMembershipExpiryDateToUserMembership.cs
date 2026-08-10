using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullStack_Shangri.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipExpiryDateToUserMembership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add MembershipExpiryDate column to the UserMembership table
            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipExpiryDate",
                table: "UserMembership",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now); // Default to current date/time
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove MembershipExpiryDate column from the UserMembership table
            migrationBuilder.DropColumn(
                name: "MembershipExpiryDate",
                table: "UserMembership");
        }
    }
}
