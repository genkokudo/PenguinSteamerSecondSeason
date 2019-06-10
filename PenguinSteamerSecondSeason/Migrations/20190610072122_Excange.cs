using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class Excange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MBoards_MExchanges_ExchangeId",
                table: "MBoards");

            migrationBuilder.DropTable(
                name: "MExchanges");

            migrationBuilder.DropIndex(
                name: "IX_MBoards_ExchangeId",
                table: "MBoards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MTimeScales");

            migrationBuilder.DropColumn(
                name: "ExchangeId",
                table: "MBoards");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MCurrencies",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MCurrencies",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MCurrencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MCurrencies",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MCurrencies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MCurrencies");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MCurrencies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MCurrencies");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MCurrencies");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MCurrencies");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MTimeScales",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeId",
                table: "MBoards",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MExchanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MExchanges", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MBoards_ExchangeId",
                table: "MBoards",
                column: "ExchangeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MBoards_MExchanges_ExchangeId",
                table: "MBoards",
                column: "ExchangeId",
                principalTable: "MExchanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
