using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class Ticker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.CreateTable(
                name: "Tickers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Exchange = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    TickId = table.Column<int>(nullable: false),
                    BestBid = table.Column<decimal>(nullable: false),
                    BestAsk = table.Column<decimal>(nullable: false),
                    BestBidSize = table.Column<decimal>(nullable: false),
                    BestAskSize = table.Column<decimal>(nullable: false),
                    TotalBidDepth = table.Column<decimal>(nullable: false),
                    TotalAskDepth = table.Column<decimal>(nullable: false),
                    Ltp = table.Column<decimal>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false),
                    VolumeByProduct = table.Column<decimal>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickers");

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Answer = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });
        }
    }
}
