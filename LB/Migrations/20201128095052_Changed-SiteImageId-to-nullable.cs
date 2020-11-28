using Microsoft.EntityFrameworkCore.Migrations;

namespace LB.Migrations
{
    public partial class ChangedSiteImageIdtonullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_SiteImages_SiteImageId",
                table: "Sermons");

            migrationBuilder.AlterColumn<int>(
                name: "SiteImageId",
                table: "Sermons",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Sermons_SiteImages_SiteImageId",
                table: "Sermons",
                column: "SiteImageId",
                principalTable: "SiteImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sermons_SiteImages_SiteImageId",
                table: "Sermons");

            migrationBuilder.AlterColumn<int>(
                name: "SiteImageId",
                table: "Sermons",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sermons_SiteImages_SiteImageId",
                table: "Sermons",
                column: "SiteImageId",
                principalTable: "SiteImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
