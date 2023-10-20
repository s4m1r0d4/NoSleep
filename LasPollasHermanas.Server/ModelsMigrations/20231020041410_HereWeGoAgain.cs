using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LasPollasHermanas.Server.ModelsMigrations
{
    /// <inheritdoc />
    public partial class HereWeGoAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MortalUserId",
                table: "Dildos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MortalUserId1",
                table: "Dildos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminUsers_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MortalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MortalUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MortalUsers_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dildos_MortalUserId",
                table: "Dildos",
                column: "MortalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dildos_MortalUserId1",
                table: "Dildos",
                column: "MortalUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_AccountId",
                table: "AdminUsers",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MortalUsers_AccountId",
                table: "MortalUsers",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dildos_MortalUsers_MortalUserId",
                table: "Dildos",
                column: "MortalUserId",
                principalTable: "MortalUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dildos_MortalUsers_MortalUserId1",
                table: "Dildos",
                column: "MortalUserId1",
                principalTable: "MortalUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dildos_MortalUsers_MortalUserId",
                table: "Dildos");

            migrationBuilder.DropForeignKey(
                name: "FK_Dildos_MortalUsers_MortalUserId1",
                table: "Dildos");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "MortalUsers");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Dildos_MortalUserId",
                table: "Dildos");

            migrationBuilder.DropIndex(
                name: "IX_Dildos_MortalUserId1",
                table: "Dildos");

            migrationBuilder.DropColumn(
                name: "MortalUserId",
                table: "Dildos");

            migrationBuilder.DropColumn(
                name: "MortalUserId1",
                table: "Dildos");
        }
    }
}
