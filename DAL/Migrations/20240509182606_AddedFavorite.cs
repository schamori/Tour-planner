using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddedFavorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Routes");

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "Routes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
