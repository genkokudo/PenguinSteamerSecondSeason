using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MCurrencies",
                columns: table => new
                {
                    MCurrencyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MCurrencies", x => x.MCurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "MTimeScales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(nullable: true),
                    SecondsValue = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MTimeScales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MBoards",
                columns: table => new
                {
                    MBoardId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MCurrency1MCurrencyId = table.Column<int>(nullable: true),
                    MCurrency2MCurrencyId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBoards", x => x.MBoardId);
                    table.ForeignKey(
                        name: "FK_MBoards_MCurrencies_MCurrency1MCurrencyId",
                        column: x => x.MCurrency1MCurrencyId,
                        principalTable: "MCurrencies",
                        principalColumn: "MCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MBoards_MCurrencies_MCurrency2MCurrencyId",
                        column: x => x.MCurrency2MCurrencyId,
                        principalTable: "MCurrencies",
                        principalColumn: "MCurrencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MBoardId = table.Column<int>(nullable: true),
                    MTimeScaleId = table.Column<int>(nullable: true),
                    BeginTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Min = table.Column<decimal>(nullable: false),
                    Max = table.Column<decimal>(nullable: false),
                    Begin = table.Column<decimal>(nullable: false),
                    End = table.Column<decimal>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candles_MBoards_MBoardId",
                        column: x => x.MBoardId,
                        principalTable: "MBoards",
                        principalColumn: "MBoardId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Candles_MTimeScales_MTimeScaleId",
                        column: x => x.MTimeScaleId,
                        principalTable: "MTimeScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MWebSockets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MBoardId = table.Column<int>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    EndPoint = table.Column<string>(nullable: true),
                    ChannelName = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MWebSockets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MWebSockets_MBoards_MBoardId",
                        column: x => x.MBoardId,
                        principalTable: "MBoards",
                        principalColumn: "MBoardId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candles_MBoardId",
                table: "Candles",
                column: "MBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Candles_MTimeScaleId",
                table: "Candles",
                column: "MTimeScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_MBoards_MCurrency1MCurrencyId",
                table: "MBoards",
                column: "MCurrency1MCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MBoards_MCurrency2MCurrencyId",
                table: "MBoards",
                column: "MCurrency2MCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MWebSockets_MBoardId",
                table: "MWebSockets",
                column: "MBoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candles");

            migrationBuilder.DropTable(
                name: "MWebSockets");

            migrationBuilder.DropTable(
                name: "MTimeScales");

            migrationBuilder.DropTable(
                name: "MBoards");

            migrationBuilder.DropTable(
                name: "MCurrencies");
        }
    }
}
