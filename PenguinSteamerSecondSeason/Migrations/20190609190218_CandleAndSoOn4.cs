using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class CandleAndSoOn4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MCurrency_Currency1Id",
                table: "MBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MCurrency_Currency2Id",
                table: "MBoards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MCurrency",
                table: "MCurrency");

            migrationBuilder.RenameTable(
                name: "MCurrency",
                newName: "MCurrencies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MCurrencies",
                table: "MCurrencies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MBoards_MCurrencies_Currency1Id",
                table: "MBoards",
                column: "Currency1Id",
                principalTable: "MCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MBoards_MCurrencies_Currency2Id",
                table: "MBoards",
                column: "Currency2Id",
                principalTable: "MCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MCurrencies_Currency1Id",
                table: "MBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MCurrencies_Currency2Id",
                table: "MBoards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MCurrencies",
                table: "MCurrencies");

            migrationBuilder.RenameTable(
                name: "MCurrencies",
                newName: "MCurrency");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MCurrency",
                table: "MCurrency",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MBoards_MCurrency_Currency1Id",
                table: "MBoards",
                column: "Currency1Id",
                principalTable: "MCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MBoards_MCurrency_Currency2Id",
                table: "MBoards",
                column: "Currency2Id",
                principalTable: "MCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
