using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullstackProject.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndGameColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only drop the column if it exists to avoid errors when applying to databases
            migrationBuilder.Sql(@"IF COL_LENGTH('dbo.Games','Image') IS NOT NULL
    BEGIN
        ALTER TABLE [dbo].[Games] DROP COLUMN [Image];
    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
