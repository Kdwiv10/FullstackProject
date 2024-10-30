using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FullstackProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    DeveloperID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Develope__DE084CD149B01792", x => x.DeveloperID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    RoleType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__8AFACE3A139672AF", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Genre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DeveloperID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Games__2AB897DDDAE95024", x => x.GameID);
                    table.ForeignKey(
                        name: "FK__Games__Developer__52593CB8",
                        column: x => x.DeveloperID,
                        principalTable: "Developers",
                        principalColumn: "DeveloperID");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    ProfileInfo = table.Column<string>(type: "text", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC0F3D84C5", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__Users__RoleID__4CA06362",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    GameID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__51BCD797A3AEE056", x => x.CartID);
                    table.ForeignKey(
                        name: "FK__Cart__GameID__693CA210",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "GameID");
                    table.ForeignKey(
                        name: "FK__Cart__UserID__68487DD7",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "GameLibrary",
                columns: table => new
                {
                    LibraryID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    GameID = table.Column<int>(type: "int", nullable: true),
                    DateAdded = table.Column<DateOnly>(type: "date", nullable: true),
                    PricePaid = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GameLibr__A13647BF43F008E5", x => x.LibraryID);
                    table.ForeignKey(
                        name: "FK__GameLibra__GameI__5629CD9C",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "GameID");
                    table.ForeignKey(
                        name: "FK__GameLibra__UserI__5535A963",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MembershipID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__92A785990BEA4EC2", x => x.MembershipID);
                    table.ForeignKey(
                        name: "FK__Membershi__UserI__59FA5E80",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    GameID = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PricePaid = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__6B0A6BDE536E93D6", x => x.PurchaseID);
                    table.ForeignKey(
                        name: "FK__Purchases__GameI__628FA481",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "GameID");
                    table.ForeignKey(
                        name: "FK__Purchases__UserI__619B8048",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false),
                    GameID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reviews__74BC79AEB172D10E", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK__Reviews__GameID__5DCAEF64",
                        column: x => x.GameID,
                        principalTable: "Games",
                        principalColumn: "GameID");
                    table.ForeignKey(
                        name: "FK__Reviews__UserID__5EBF139D",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    PurchaseID = table.Column<int>(type: "int", nullable: true),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AmountPaid = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__9B556A5810BBE4EE", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK__Payments__Purcha__656C112C",
                        column: x => x.PurchaseID,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_GameID",
                table: "Cart",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserID",
                table: "Cart",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UQ__Develope__A9D10534268086DC",
                table: "Developers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameLibrary_GameID",
                table: "GameLibrary",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_GameLibrary_UserID",
                table: "GameLibrary",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Games_DeveloperID",
                table: "Games",
                column: "DeveloperID");

            migrationBuilder.CreateIndex(
                name: "UQ__Membersh__1788CCADDB5B030F",
                table: "Memberships",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PurchaseID",
                table: "Payments",
                column: "PurchaseID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_GameID",
                table: "Purchases",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserID",
                table: "Purchases",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_GameID",
                table: "Reviews",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105349FA7B6FF",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "GameLibrary");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Developers");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
