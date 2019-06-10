using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class Excange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MCurrencies_Currency1Id",
                table: "MBoards");

            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MCurrencies_Currency2Id",
                table: "MBoards");

            migrationBuilder.DropIndex(
                name: "IX_MBoards_Currency1Id",
                table: "MBoards");

            migrationBuilder.DropColumn(
                name: "Currency1Id",
                table: "MBoards");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MCurrencies",
                newName: "MCurrencyId");

            migrationBuilder.RenameColumn(
                name: "Currency2Id",
                table: "MBoards",
                newName: "MCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_MBoards_Currency2Id",
                table: "MBoards",
                newName: "IX_MBoards_MCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MBoards_MCurrencies_MCurrencyId",
                table: "MBoards",
                column: "MCurrencyId",
                principalTable: "MCurrencies",
                principalColumn: "MCurrencyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MCurrencies_MCurrencyId",
                table: "MBoards");

            migrationBuilder.RenameColumn(
                name: "MCurrencyId",
                table: "MCurrencies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MCurrencyId",
                table: "MBoards",
                newName: "Currency2Id");

            migrationBuilder.RenameIndex(
                name: "IX_MBoards_MCurrencyId",
                table: "MBoards",
                newName: "IX_MBoards_Currency2Id");

            migrationBuilder.AddColumn<int>(
                name: "Currency1Id",
                table: "MBoards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MBoards_Currency1Id",
                table: "MBoards",
                column: "Currency1Id");

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
    }
}
