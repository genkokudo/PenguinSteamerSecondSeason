using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class CandleAndSoOn3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeScaleId",
                table: "Candles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candles_TimeScaleId",
                table: "Candles",
                column: "TimeScaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candles_MTimeScales_TimeScaleId",
                table: "Candles",
                column: "TimeScaleId",
                principalTable: "MTimeScales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candles_MTimeScales_TimeScaleId",
                table: "Candles");

            migrationBuilder.DropIndex(
                name: "IX_Candles_TimeScaleId",
                table: "Candles");

            migrationBuilder.DropColumn(
                name: "TimeScaleId",
                table: "Candles");
        }
    }
}
