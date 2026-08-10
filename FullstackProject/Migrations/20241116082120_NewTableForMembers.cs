using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullStack_Shangri.Migrations
{
    public partial class NewTableForMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMembership",
                columns: table => new
                {
                    UserMembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMembership", x => x.UserMembershipId);
                    table.ForeignKey(
                        name: "FK_UserMembership_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMembership_MembershipId",
                table: "UserMembership",
                column: "MembershipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMembership");
        }
    }
}
