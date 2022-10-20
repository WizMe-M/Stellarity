using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stellarity.Migrations
{
    public partial class AddedActivated_RenamedDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "deleted",
                table: "users",
                newName: "banned");

            migrationBuilder.AddColumn<bool>(
                name: "activated",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activated",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "banned",
                table: "users",
                newName: "deleted");
        }
    }
}
