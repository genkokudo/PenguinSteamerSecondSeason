using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class AddWebSocket2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "MWebSockets");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "MWebSockets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "MWebSockets");

            migrationBuilder.AddColumn<int>(
                name: "Enabled",
                table: "MWebSockets",
                nullable: false,
                defaultValue: 0);
        }
    }
}
