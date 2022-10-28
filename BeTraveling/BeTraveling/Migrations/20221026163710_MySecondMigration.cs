using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeTraveling.Migrations
{
    public partial class MySecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UsersInfo_UserId",
                table: "UsersInfo",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersInfo_UserId",
                table: "UsersInfo");
        }
    }
}
