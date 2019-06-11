using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PenguinSteamerSecondSeason.Migrations
{
    public partial class AddWebSocket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MWebSockets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BoardId = table.Column<int>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    EndPoint = table.Column<string>(nullable: true),
                    ChannelName = table.Column<string>(nullable: true),
                    Enabled = table.Column<int>(nullable: false),
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
                        name: "FK_MWebSockets_MBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "MBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MWebSockets_BoardId",
                table: "MWebSockets",
                column: "BoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MWebSockets");
        }
    }
}
