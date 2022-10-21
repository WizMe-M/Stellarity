using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stellarity.Migrations
{
    public partial class RemovedLibrary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_key_user",
                table: "key");

            migrationBuilder.DropTable(
                name: "library");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "nickname", "password" },
                values: new object[] { "Stellar", "161EBD7D45089B3446EE4E0D86DBCF92" });

            migrationBuilder.AddForeignKey(
                name: "fk_key_user",
                table: "key",
                column: "buyer_id",
                principalTable: "users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_key_user",
                table: "key");

            migrationBuilder.CreateTable(
                name: "library",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    purchase_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_library", x => new { x.user_id, x.game_id });
                    table.ForeignKey(
                        name: "fk_library_game",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_library_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "nickname", "password" },
                values: new object[] { "Stellarity.Desktop", "P@ssw0rd" });

            migrationBuilder.CreateIndex(
                name: "IX_library_game_id",
                table: "library",
                column: "game_id");

            migrationBuilder.AddForeignKey(
                name: "fk_key_user",
                table: "key",
                column: "buyer_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
