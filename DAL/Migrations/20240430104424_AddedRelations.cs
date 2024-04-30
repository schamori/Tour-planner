using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Routes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Routes_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Routes_TourId",
                table: "TourLogs");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Routes");
        }
    }
}
