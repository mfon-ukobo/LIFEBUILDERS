using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LB.Migrations
{
    public partial class CreatedSermon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropColumn(
                name: "RequiresLogin",
                table: "Resources");*/

            migrationBuilder.CreateTable(
                name: "Sermons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    SiteImageId = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    FileType = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sermons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sermons_SiteImages_SiteImageId",
                        column: x => x.SiteImageId,
                        principalTable: "SiteImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sermons_SiteImageId",
                table: "Sermons",
                column: "SiteImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sermons");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresLogin",
                table: "Resources",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
