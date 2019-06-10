using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class CandleAndSoOn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Candles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candles_BoardId",
                table: "Candles",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candles_MBoards_BoardId",
                table: "Candles",
                column: "BoardId",
                principalTable: "MBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candles_MBoards_BoardId",
                table: "Candles");

            migrationBuilder.DropIndex(
                name: "IX_Candles_BoardId",
                table: "Candles");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Candles");
        }
    }
}
