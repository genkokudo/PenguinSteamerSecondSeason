using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class CandleAndSoOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickers");

            migrationBuilder.CreateTable(
                name: "Candles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeStamp = table.Column<string>(nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "MCurrency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MCurrency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MExchanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_MExchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MTimeScales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ExchangeId = table.Column<int>(nullable: true),
                    Currency1Id = table.Column<int>(nullable: true),
                    Currency2Id = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBoards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MBoards_MCurrency_Currency1Id",
                        column: x => x.Currency1Id,
                        principalTable: "MCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MBoards_MCurrency_Currency2Id",
                        column: x => x.Currency2Id,
                        principalTable: "MCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MBoards_MExchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalTable: "MExchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MBoards_Currency1Id",
                table: "MBoards",
                column: "Currency1Id");

            migrationBuilder.CreateIndex(
                name: "IX_MBoards_Currency2Id",
                table: "MBoards",
                column: "Currency2Id");

            migrationBuilder.CreateIndex(
                name: "IX_MBoards_ExchangeId",
                table: "MBoards",
                column: "ExchangeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candles");

            migrationBuilder.DropTable(
                name: "MBoards");

            migrationBuilder.DropTable(
                name: "MTimeScales");

            migrationBuilder.DropTable(
                name: "MCurrency");

            migrationBuilder.DropTable(
                name: "MExchanges");

            migrationBuilder.CreateTable(
                name: "Tickers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BestAsk = table.Column<decimal>(nullable: false),
                    BestAskSize = table.Column<decimal>(nullable: false),
                    BestBid = table.Column<decimal>(nullable: false),
                    BestBidSize = table.Column<decimal>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Exchange = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Ltp = table.Column<decimal>(nullable: false),
                    TickId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    TotalAskDepth = table.Column<decimal>(nullable: false),
                    TotalBidDepth = table.Column<decimal>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Volume = table.Column<decimal>(nullable: false),
                    VolumeByProduct = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickers", x => x.Id);
                });
        }
    }
}
